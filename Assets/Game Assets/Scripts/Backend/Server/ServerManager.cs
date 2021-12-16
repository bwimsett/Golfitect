using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using Backend.Course;
using Backend.Level;
using Backend.Serialization;
using Game_Assets.Scripts.Backend.Server;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour {

	private static Callback<GetAuthSessionTicketResponse_t> ticketCallback;
	private static byte[] ticketData;
	private static string ticketString;
	private const int ticketSize = 1024;
	private static TicketGenerationStatus ticketGenerationStatus = TicketGenerationStatus.Awaiting;
	private static List<UnityAction<string>> onTicketGeneratedCallbacks;
	private static string serverUrl = "https://golfitect.co.uk", scoreUrl = "/scores", ugcUrl = "/ugc";

	private enum TicketGenerationStatus {
		Awaiting, Success, Failure, InProgress
	}
	
	private static void GetAuthTicket(UnityAction<string> onComplete) {
		if (ticketGenerationStatus == TicketGenerationStatus.Failure) {
			Debug.LogError("Failed to generate Steam Authentication ticket. Can't interact with server.");
			return;
		}
		
		if (ticketGenerationStatus == TicketGenerationStatus.Success) {
			onComplete.Invoke(ticketString);
			return;
		}

		if (ticketGenerationStatus == TicketGenerationStatus.InProgress) {
			onTicketGeneratedCallbacks.Add(onComplete);
			return;
		}

		if (onTicketGeneratedCallbacks == null) {
			onTicketGeneratedCallbacks = new List<UnityAction<string>>();
		}

		ticketGenerationStatus = TicketGenerationStatus.InProgress;
		ticketData = new byte[ticketSize];
		HAuthTicket ticket = SteamUser.GetAuthSessionTicket(ticketData, ticketSize, out uint ticketLength);
		onTicketGeneratedCallbacks.Add(onComplete);
		ticketCallback = Callback<GetAuthSessionTicketResponse_t>.Create(OnTicketCreated);
	}

	private static void OnTicketCreated(GetAuthSessionTicketResponse_t response) {
		if (response.m_eResult != EResult.k_EResultOK) {
			ticketGenerationStatus = TicketGenerationStatus.Failure;
			Debug.LogError("Failed to get an auth ticket for the user: "+response.m_eResult);
			return;
		}
		
		ticketGenerationStatus = TicketGenerationStatus.Success;

		ticketString = BitConverter.ToString(ticketData).Replace("-", "");
		
		foreach (UnityAction<string> action in onTicketGeneratedCallbacks) {
			action.Invoke(ticketString);
		}
		
		onTicketGeneratedCallbacks.Clear();
	}

	public void GetUserLevelInfo(UnityAction<DBHoleInfo[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/holes/getinfo?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				DBHoleInfo[] levels = JsonConvert.DeserializeObject <DBHoleInfo[]>(value);
				onComplete.Invoke(levels);
			}));
		});
	}
	
	public void GetUserCourseInfo(UnityAction<DBCourseInfo[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/courses/getinfo?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				DBCourseInfo[] levels = JsonConvert.DeserializeObject <DBCourseInfo[]>(value);
				onComplete.Invoke(levels);
			}));
		});
	}

	public void GetUserCourses(UnityAction<DBCourseInfo[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/courses/getall?ticket=" + ticket;
			StartCoroutine(GetRequest(uri, value => {
				DBCourseInfo[] dataItems = JsonConvert.DeserializeObject<DBCourseInfo[]>(value);
				onComplete.Invoke(dataItems);
			}));
		});
	}

	public void SubmitLevel(ServerSerializable level, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("level", level.GetJson());

			string uri = ugcUrl + "/holes/submit";

			StartCoroutine(PostRequest(uri, form, onComplete));
		});
	}

	public void SubmitCourse(Course course, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("level", course.GetJson());

			string uri = ugcUrl + "/courses/submit";

			StartCoroutine(PostRequest(uri, form, onComplete));
		});
	}

	private static IEnumerator PostRequest(string uri, WWWForm form, UnityAction<string> onComplete) {

		uri = serverUrl + uri;

		using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)){
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string result = "";

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			switch (webRequest.result) {
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					result = pages[page] + ": Error: " + webRequest.error;
					break;
				case UnityWebRequest.Result.ProtocolError:
					result = pages[page] + ": HTTP Error: " + webRequest.error;
					break;
				case UnityWebRequest.Result.Success:
					result = webRequest.downloadHandler.text;
					break;
			}

			if (onComplete != null) {
				onComplete.Invoke(result);
			}
		}
	}

	private static IEnumerator GetRequest(string uri, UnityAction<string> onComplete) {

		uri = serverUrl + uri;
		
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			string result = "";

			switch (webRequest.result) {
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					result = pages[page] + ": Error: " + webRequest.error;
					break;
				case UnityWebRequest.Result.ProtocolError:
					result = pages[page] + ": HTTP Error: " + webRequest.error;
					break;
				case UnityWebRequest.Result.Success:
					result = webRequest.downloadHandler.text;
					break;
			}
			
			if (onComplete != null) {
				onComplete.Invoke(result);
			}
		}
	}

}

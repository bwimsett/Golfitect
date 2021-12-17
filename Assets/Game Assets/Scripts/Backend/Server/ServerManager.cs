using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Resources;
using System.Web.Security;
using Backend.Course;
using Backend.Level;
using Backend.Serialization;
using Game_Assets.Scripts.Backend.Server;
using Newtonsoft.Json;
using Steamworks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour {

	// Steam Ticket Generation
	private static Callback<GetAuthSessionTicketResponse_t> ticketCallback;
	private static byte[] ticketData;
	private static string ticketString;
	private const int ticketSize = 1024;
	private static TicketGenerationStatus ticketGenerationStatus = TicketGenerationStatus.Awaiting;
	private static List<UnityAction<string>> onTicketGeneratedCallbacks;
	private enum TicketGenerationStatus { Awaiting, Success, Failure, InProgress }
	
	// URL
	private static string serverUrl = "https://golfitect.co.uk", scoreUrl = "/scores", ugcUrl = "/ugc", usersUrl = "/users";
	public static string userid { get; private set; }
	private static bool useridInitialised;
	
	void Awake() {
		RefreshUserID();
	}

	//* -------------------------- STEAM TICKET --------------------------  *//
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
		
		ticketString = BitConverter.ToString(ticketData).Replace("-", "");
		
		ticketGenerationStatus = TicketGenerationStatus.Success;
			
		foreach (UnityAction<string> action in onTicketGeneratedCallbacks) { 
			action.Invoke(ticketString);
		}
		
		onTicketGeneratedCallbacks.Clear();
	}
	

	//* ---------------------------- RETRIEVAL ----------------------------  *//

	private void RefreshUserID() {
		if (useridInitialised) {
			return;
		}
		
		GetAuthTicket(ticket => {
			string url = usersUrl + "/userid?ticket=" + ticket;
			StartCoroutine(GetRequest(url, result => {
				useridInitialised = true;
				userid = result.Replace("\"", "");
			}));
		});
	}
	
	public void GetUserLevelInfo(UnityAction<DBHoleInfo[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/holes/getall?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				DBHoleInfo[] levels = JsonConvert.DeserializeObject <DBHoleInfo[]>(value);
				onComplete.Invoke(levels);
			}));
		});
	}
	
	public void GetUserCourseInfo(UnityAction<DBCourseInfo[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/courses/getall?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				DBCourseInfo[] levels = JsonConvert.DeserializeObject <DBCourseInfo[]>(value);
				onComplete.Invoke(levels);
			}));
		});
	}
	
	public void GetHole(string holeID, UnityAction<Level> onComplete) {
		string uri = ugcUrl + "/holes/get?holeID=" + holeID;
		StartCoroutine(GetRequest(uri, result => {
			Level level = JsonConvert.DeserializeObject<Level>(result);
			onComplete.Invoke(level);
		}));
	}

	public void GetCourse(string courseID, UnityAction<Course> onComplete) {
		string uri = ugcUrl + "/courses/get?courseID=" + courseID;
		StartCoroutine(GetRequest(uri, result => {
			Course course = JsonConvert.DeserializeObject<Course>(result);
			onComplete.Invoke(course);
		}));
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

	public void GetUserCourseScores(Course course, UnityAction<HoleScore[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = scoreUrl + "/holes/getmany";
			
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("dat", JsonConvert.SerializeObject(course.holeIDs));

			StartCoroutine(PostRequest(uri, form, result => {
				HoleScore[] scores = JsonConvert.DeserializeObject<HoleScore[]>(result);
				onComplete.Invoke(scores);
			}));
		});
	}
	
	private static IEnumerator GetRequest(string uri, UnityAction<string> onComplete) {

		uri = serverUrl + uri;
		
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;
			
			string result = webRequest.downloadHandler.text;

			if (webRequest.error != null) {
				Debug.LogError("Server Error: "+result);
			}

			if (onComplete != null) {
				onComplete.Invoke(result);
			}
		}
	}
	
	//* ---------------------------- SUBMISSION ----------------------------  *//
	 
	public void SubmitLevel(ServerSerializable level, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("dat", level.GetJson());

			string uri = ugcUrl + "/holes/submit";

			StartCoroutine(PostRequest(uri, form, onComplete));
		});
	}

	public void SubmitCourse(Course course, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("dat", course.GetJson());

			string uri = ugcUrl + "/courses/submit";

			StartCoroutine(PostRequest(uri, form, onComplete));
		});
	}

	public void SubmitScore(Score score, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("dat", score.GetJSON());
			string url = scoreUrl;

			if (score is HoleScore) {
				url += "/holes/submit";
			} else if (score is CourseScore) {
				url += "/courses/submit";
			}

			StartCoroutine(PostRequest(url, form, onComplete));
		});
	}

	private static IEnumerator PostRequest(string uri, WWWForm form, UnityAction<string> onComplete) {

		uri = serverUrl + uri;

		using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)){
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();
			
			string[] pages = uri.Split('/');
			int page = pages.Length - 1;
			
			string result = webRequest.downloadHandler.text;

			if (webRequest.error != null) {
				Debug.LogError("Server Error: "+result);
			}

			if (onComplete != null) {
				onComplete.Invoke(result);
			}
		}
	}
	
}

using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Submittable;
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

	public void SubmitLevel(ServerSubmittable level, UnityAction<string> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("levelData", level.GetJson());

			string uri = ugcUrl + "/hole/submit";

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
					result = "Received: " + webRequest.downloadHandler.text;
					break;
			}
			
			if (onComplete != null) {
				onComplete.Invoke(result);
			}
		}
	}

}

using System;
using System.Collections;
using System.Collections.Generic;
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

	// Steam Ticket Generation
	private static Callback<GetAuthSessionTicketResponse_t> ticketCallback;
	private static byte[] ticketData;
	private static string ticketString;
	private const int ticketSize = 1024;
	private static TicketGenerationStatus ticketGenerationStatus = TicketGenerationStatus.Awaiting;
	private static List<UnityAction<string>> onTicketGeneratedCallbacks;
	private enum TicketGenerationStatus { Awaiting, Success, Failure, InProgress }
	
	// URL
	private static string serverUrl = "https://golfitect.co.uk", scoreUrl = "/scores", ugcUrl = "/ugc", usersUrl = "/users", rankingsUrl = "/rankings";
	public static string userid { get; private set; }
	public static bool useridInitialised { get; private set; }
	
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
	
	public void GetUserLevelIDs(UnityAction<string[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/holes/getuserholes?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				string[] levels = JsonConvert.DeserializeObject <string[]>(value);
				onComplete.Invoke(levels);
			}));
		});
	}
	
	public void GetUserCourseInfo(UnityAction<string[]> onComplete) {
		GetAuthTicket(ticket => {
			string uri = ugcUrl + "/courses/getusercourses?ticket="+ticket;
			StartCoroutine(GetRequest(uri, value => {
				string[] levelIDs = JsonConvert.DeserializeObject <string[]>(value);
				onComplete.Invoke(levelIDs);
			}));
		});
	}

	public void GetUser(UnityAction<DBUser> onComplete) {
		
	}
	
	public void GetHole(string holeID, UnityAction<Level> onComplete) {
		string uri = ugcUrl + "/holes/get?holeID=" + holeID;
		StartCoroutine(GetRequest(uri, result => {
			Level level = JsonConvert.DeserializeObject<Level>(result);
			onComplete.Invoke(level);
		}));
	}

	public void GetHoles(string[] holeIDs, UnityAction<DBHoleInfo[]> onComplete) {
		string uri = ugcUrl + "/holes/getmany?ids=";
		for (int i = 0; i < holeIDs.Length; i++) {
			if (i > 0) {
				uri += ",";
			}

			uri += holeIDs[i];
		}

		StartCoroutine(GetRequest(uri, result => {
			if (result == null) {
				return;
			}
			
			DBHoleInfo[] holes = JsonConvert.DeserializeObject<DBHoleInfo[]>(result);
			onComplete.Invoke(holes);
		}));
	}

	public void GetCourse(DBCourseInfo courseInfo, UnityAction<Course> onComplete) {
		string uri = ugcUrl + "/courses/get?courseID=" + courseInfo._id;
		StartCoroutine(GetRequest(uri, result => {
			Course course = JsonConvert.DeserializeObject<Course>(result);
			course.courseInfo = courseInfo;
			onComplete.Invoke(course);
		}));
	}

	public void GetCourseLeaderboards(DBCourseInfo courseInfo, UnityAction<DBUserScore[]> onComplete) {
		string uri = scoreUrl + "/courses/leaderboard?courseid=" + courseInfo._id;
		StartCoroutine(GetRequest(uri, result => {
			DBUserScore[] leaderboard = JsonConvert.DeserializeObject<DBUserScore[]>(result);
			onComplete.Invoke(leaderboard);
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

	public void GetNewestCourses(UnityAction<string[]> onComplete) {
		string uri = rankingsUrl + "/newest?startpage=0&pages=12";
		StartCoroutine(GetRequest(uri, value => {
			string[] courseList = JsonConvert.DeserializeObject<string[]>(value);
			onComplete.Invoke(courseList);
		}));
	}

	public void GetCourses(string[] courseids, UnityAction<DBCourseInfo[]> onComplete) {
		string idstring = "";
		for (int i = 0; i < courseids.Length; i++) {
			if (i > 0) {
				idstring += ",";
			}

			idstring += courseids[i];
		}

		string uri = ugcUrl + "/courses/getmany?ids=" + idstring;
		StartCoroutine(GetRequest(uri, value => {
			if (value == null) {
				return;
			}
			
			// Convert result to list of dbcourseinfo
			DBCourseInfo[] result = JsonConvert.DeserializeObject<DBCourseInfo[]>(value);
			onComplete.Invoke(result);
		}));
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

	public void GetUserCourseScore(DBCourseInfo course, UnityAction<DBCourseScore> onComplete) {
		string url = scoreUrl + "/courses/getuserhighscore?userid=" + userid + "&courseid=" + course._id;
		StartCoroutine(GetRequest(url, result => {
			DBCourseScore dbCourseScore = null;
			if (result != null) {
				dbCourseScore = JsonConvert.DeserializeObject<DBCourseScore>(result);
			}

			onComplete.Invoke(dbCourseScore);
		}));
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

	public void SubmitAndGetHighScore(Score score, UnityAction<Score> onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();
			form.AddField("ticket", ticket);
			form.AddField("dat", score.GetJSON());
			string url = scoreUrl;

			if (score is HoleScore) {
				url += "/holes/submit";
			} else if (score is DBCourseScore) {
				url += "/courses/submit";
			}

			StartCoroutine(PostRequest(url, form, result => {
				if (score is HoleScore) {
					score = JsonConvert.DeserializeObject<HoleScore>(result);
				}
				else {
					score = JsonConvert.DeserializeObject<DBCourseScore>(result);
				}
				onComplete.Invoke(score);
			}));
		});
	}

	public void SubmitLike(DBCourseInfo course, bool like, UnityAction onComplete) {
		GetAuthTicket(ticket => {
			WWWForm form = new WWWForm();

			var data = new {
				like,
				courseid = course._id
			};
			
			form.AddField("ticket", ticket);
			form.AddField("dat", JsonConvert.SerializeObject(data));

			string uri = scoreUrl + "/courses/like";

			StartCoroutine(PostRequest(uri, form, result => {
				onComplete?.Invoke();
			}));
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

using System;
using System.Collections.Generic;
using System.IO;
using Backend.Course;
using Backend.Enums;
using Backend.Managers;
using Backend.Submittable;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace Backend.Level {
	public class LevelLoader {

		private Dictionary<UGCQueryHandle_t, LevelType> queryLevelTypes;
		
		// Callbacks
		private Action<SteamItemData[]> onUserQueryCompleteCallback;

		public LevelLoader() {
			queryLevelTypes = new Dictionary<UGCQueryHandle_t, LevelType>();
		}
		
		// ---------- LEVEL QUERYING ----------
		public void GetUserLevelInfos(AccountID_t user, uint page, Action<SteamItemData[]> onComplete, LevelType levelType) {
			AppId_t appId = SteamUtils.GetAppID();

			onUserQueryCompleteCallback = onComplete;

			CallResult<SteamUGCQueryCompleted_t> queryCompleted =
				CallResult<SteamUGCQueryCompleted_t>.Create(OnGetUserLevelInfosQueryComplete);

			UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUserUGCRequest(user,
				EUserUGCList.k_EUserUGCList_Published,
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_All,
				EUserUGCListSortOrder.k_EUserUGCListSortOrder_LastUpdatedDesc, appId,
				appId, page);

			queryLevelTypes.Add(queryHandle, levelType);
			
			// Query based on the type of level (hole or course)
			string tag = new Level(Vector3Int.zero).itemTypeTag;
			if (levelType == LevelType.Course) { tag = new Course.Course("","",null).itemTypeTag; }
			SteamUGC.AddRequiredTag(queryHandle, tag);

			SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(queryHandle);
			queryCompleted.Set(call);
		}

		private void OnGetUserLevelInfosQueryComplete(SteamUGCQueryCompleted_t item, bool bIOFailure) {
			if (item.m_eResult != EResult.k_EResultOK) {
				Debug.LogError("Query to steam failed.");
				return;
			}

			uint numResults = item.m_unNumResultsReturned;
			SteamItemData[] results = new SteamItemData[numResults];

			// Get the type of level for the query (hole or course)
			queryLevelTypes.TryGetValue(item.m_handle, out LevelType levelType);
			queryLevelTypes.Remove(item.m_handle);

			for (uint i = 0; i < results.Length; i++) {
				SteamUGCDetails_t details;
				SteamUGC.GetQueryUGCResult(item.m_handle, i, out details);
				//Debug.Log("Found file: "+details.m_nPublishedFileId);
				if (levelType == LevelType.Hole) {
					results[i] = new SteamItemData(details);
				} else if (levelType == LevelType.Course) {
					results[i] = new SteamCourseData(details);
				}
			}

			onUserQueryCompleteCallback.Invoke(results);
		}
	}
}
using System;
using System.IO;
using Backend.Managers;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace Backend.Level {
	public class LevelUploader {

		private Level level;

		// Callbacks
		private Action<LevelInfo[]> onUserQueryCompleteCallback;

		// ---------- LEVEL QUERYING ----------
		public void GetUserLevelInfos(AccountID_t user, uint page, Action<LevelInfo[]> onComplete) {
			AppId_t appId = SteamUtils.GetAppID();

			onUserQueryCompleteCallback = onComplete;

			CallResult<SteamUGCQueryCompleted_t> queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(OnGetUserLevelInfosQueryComplete);
			
			UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUserUGCRequest(user,
				EUserUGCList.k_EUserUGCList_Published,
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_All,
				EUserUGCListSortOrder.k_EUserUGCListSortOrder_LastUpdatedDesc, appId,
				appId, page);
			
			SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(queryHandle);
			queryCompleted.Set(call);
		}

		private void OnGetUserLevelInfosQueryComplete(SteamUGCQueryCompleted_t item, bool bIOFailure) {
			if (item.m_eResult != EResult.k_EResultOK) {
				Debug.LogError("Query to steam failed.");
				return;
			}

			uint numResults = item.m_unNumResultsReturned;
			LevelInfo[] results = new LevelInfo[numResults];

			for (uint i = 0; i < results.Length; i++) {
				SteamUGCDetails_t details;
				SteamUGC.GetQueryUGCResult(item.m_handle, i, out details);
				//Debug.Log("Found file: "+details.m_nPublishedFileId);
				results[i] = new LevelInfo(details);
			}

			onUserQueryCompleteCallback.Invoke(results);
		}
		
		// ---------- LEVEL DOWNLOADING ----------
		public void GetLevelFromID(PublishedFileId_t fileId) {
			Debug.Log("Attempting to download: "+fileId);
			
			if (SteamUGC.DownloadItem(fileId, true)) {
				Callback<DownloadItemResult_t> downloadResult = new Callback<DownloadItemResult_t>(OnDownloadComplete);
			};
		}

		private void OnDownloadComplete(DownloadItemResult_t item) {
			if (item.m_unAppID != SteamUtils.GetAppID()) {
				return;
			}
			
			if (item.m_eResult != EResult.k_EResultOK) {
				Debug.Log("Failed to download level: "+item.m_nPublishedFileId);
				return;
			}
			
			ulong punSizeOnDisk = 0;
			string pchFolder = "";
			uint cchFolderSize = 500000000;
			uint punTimeStamp = 0;

			SteamUGC.GetItemInstallInfo(item.m_nPublishedFileId, out punSizeOnDisk, out pchFolder, cchFolderSize, out punTimeStamp);
			
			Debug.Log($"Item installed: {item.m_nPublishedFileId} at {pchFolder}");
		}
	}
}
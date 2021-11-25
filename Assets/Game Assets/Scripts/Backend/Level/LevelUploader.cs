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
		private const string levelFileName = "level";
		private const string levelFileFolderName = "level";
		private const string levelFileExtension = "golflvl";

		// Callbacks
		private Action<PublishedFileId_t[]> onUserQueryCompleteCallback;

		private void SaveLevelLocal(Level level) {
			level.Save();
			string levelJson = JsonConvert.SerializeObject(level, Formatting.Indented);
			Directory.CreateDirectory(GetLocalLevelSaveFolder());
			File.WriteAllText(GetLocalLevelSaveFolder()+"/"+levelFileName+"."+levelFileExtension, levelJson);
		}

		private string GetLocalLevelSaveFolder() {
			return Application.persistentDataPath + "/" + levelFileFolderName;
		}
		
		public void UploadLevelToSteam(Level level) {
			this.level = level;
			
			SaveLevelLocal(level);
			
			if (!SteamManager.Initialized) {
				return;
			}

			SteamAPICall_t call = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
			
			CallResult<CreateItemResult_t> levelCreationCall = CallResult<CreateItemResult_t>.Create(OnLevelCreation);
			levelCreationCall.Set(call);
			
		}

		private void OnLevelCreation(CreateItemResult_t item, bool bIOFailure) {
			if (item.m_eResult != EResult.k_EResultOK) {
				GameSceneManager.console.Print("Submit level failed with code: "+item.m_eResult);
				return;
			}
			
			GameSceneManager.console.Print("User needs to accept steam workshop agreement: "+item.m_bUserNeedsToAcceptWorkshopLegalAgreement);

			PublishedFileId_t id = item.m_nPublishedFileId;

			UGCUpdateHandle_t updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), id);

			if (level.levelName == null) {
				GameSceneManager.console.Print("Cant save level to steam with Null name.");
				return;
			}
			SteamUGC.SetItemTitle(updateHandle, level.levelName);
			SteamUGC.SetItemContent(updateHandle, GetLocalLevelSaveFolder());
			
			SteamAPICall_t call = SteamUGC.SubmitItemUpdate(updateHandle, "");
			CallResult<SubmitItemUpdateResult_t> updateResult = CallResult<SubmitItemUpdateResult_t>.Create(OnLevelUploadUpdate);
			updateResult.Set(call);
		}

		private void OnLevelUploadUpdate(SubmitItemUpdateResult_t item, bool bIOFailure) {
			GameSceneManager.console.Print("Upload status: " + item.m_eResult);
		}

		public void GetUserLevels(AccountID_t user, uint page, Action<PublishedFileId_t[]> onComplete) {
			AppId_t appId = SteamUtils.GetAppID();

			onUserQueryCompleteCallback = onComplete;

			CallResult<SteamUGCQueryCompleted_t> queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(OnUserLevelsQueryComplete);
			
			UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUserUGCRequest(user,
				EUserUGCList.k_EUserUGCList_Published,
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_All,
				EUserUGCListSortOrder.k_EUserUGCListSortOrder_LastUpdatedDesc, appId,
				appId, page);
			
			SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(queryHandle);
			queryCompleted.Set(call);
		}

		private void OnUserLevelsQueryComplete(SteamUGCQueryCompleted_t item, bool bIOFailure) {
			if (item.m_eResult != EResult.k_EResultOK) {
				Debug.LogError("Query to steam failed.");
				return;
			}

			uint numResults = item.m_unNumResultsReturned;
			PublishedFileId_t[] results = new PublishedFileId_t[numResults];

			for (uint i = 0; i < results.Length; i++) {
				SteamUGCDetails_t details;
				SteamUGC.GetQueryUGCResult(item.m_handle, i, out details);
				Debug.Log("Found file: "+details.m_nPublishedFileId);
				results[i] = details.m_nPublishedFileId;
			}

			onUserQueryCompleteCallback.Invoke(results);
		}
	}
}
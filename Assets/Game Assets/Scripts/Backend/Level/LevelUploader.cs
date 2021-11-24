using System.IO;
using Backend.Managers;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Backend.Level {
	public class LevelUploader {

		private Level level;
		private const string levelFileName = "level";
		private const string levelFileFolderName = "level";
		private const string levelFileExtension = "golflvl";

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
			SteamUGC.SetItemTitle(updateHandle, level.levelName);
			SteamUGC.SetItemContent(updateHandle, GetLocalLevelSaveFolder());
			
			SteamAPICall_t call = SteamUGC.SubmitItemUpdate(updateHandle, "");
			CallResult<SubmitItemUpdateResult_t> updateResult = CallResult<SubmitItemUpdateResult_t>.Create(OnUploadUpdate);
			updateResult.Set(call);
		}

		private void OnUploadUpdate(SubmitItemUpdateResult_t item, bool bIOFailure) {
			GameSceneManager.console.Print("Upload status: " + item.m_eResult);
		}
		
	}
}
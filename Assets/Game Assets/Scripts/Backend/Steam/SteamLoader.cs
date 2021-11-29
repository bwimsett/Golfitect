using System;
using System.IO;
using Backend.Managers;
using Steamworks;
using UnityEngine;

namespace Backend.Submittable {
	public class SteamLoader {

		private SteamSubmittable submittable;

		private Action<string> onDownloadComplete;
		
		// ---------- UPLOADS ----------
		public void UploadToSteam(SteamSubmittable submittable) {
			this.submittable = submittable;
			
			submittable.SaveLocal();
			
			if (!SteamManager.Initialized) {
				return;
			}

			SteamAPICall_t call = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
			
			CallResult<CreateItemResult_t> itemCreationCall = CallResult<CreateItemResult_t>.Create(OnItemCreation);
			itemCreationCall.Set(call);
		}
		
		private void OnItemCreation(CreateItemResult_t item, bool bIOFailure) {
			if (item.m_eResult != EResult.k_EResultOK) {
				GameSceneManager.console.Print("Submit level failed with code: "+item.m_eResult);
				return;
			}
			
			GameSceneManager.console.Print("User needs to accept steam workshop agreement: "+item.m_bUserNeedsToAcceptWorkshopLegalAgreement);

			PublishedFileId_t id = item.m_nPublishedFileId;

			UGCUpdateHandle_t updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), id);

			if (submittable.title == null) {
				GameSceneManager.console.Print("Cant save level to steam with Null name.");
				return;
			}
			
			SteamUGC.SetItemTitle(updateHandle, submittable.title);
			SteamUGC.SetItemDescription(updateHandle, submittable.description);
			SteamUGC.SetItemContent(updateHandle, submittable.savePath);
			
			SteamAPICall_t call = SteamUGC.SubmitItemUpdate(updateHandle, "");
			CallResult<SubmitItemUpdateResult_t> updateResult = CallResult<SubmitItemUpdateResult_t>.Create(OnItemUploadUpdate);
			updateResult.Set(call);
		}
		
		private void OnItemUploadUpdate(SubmitItemUpdateResult_t item, bool bIOFailure) {
			GameSceneManager.console.Print("Upload status: " + item.m_eResult);
		}
		
		// ---------- DOWNLOADS ----------
		public void GetFileFromID(PublishedFileId_t fileId, Action<string> onComplete) {
			Debug.Log("Attempting to download: "+fileId);
			onDownloadComplete = onComplete;
			
			if (SteamUGC.DownloadItem(fileId, true)) {
				Callback<DownloadItemResult_t> downloadResult = new Callback<DownloadItemResult_t>(OnDownloadComplete);
			};
		}

		private void OnDownloadComplete(DownloadItemResult_t item) {
			if (item.m_unAppID != SteamUtils.GetAppID()) {
				return;
			}
			
			if (item.m_eResult != EResult.k_EResultOK) {
				Debug.Log("Failed to download file: "+item.m_nPublishedFileId);
				return;
			}
			
			ulong punSizeOnDisk = 0;
			string pchFolder = "";
			uint cchFolderSize = 500000000;
			uint punTimeStamp = 0;

			SteamUGC.GetItemInstallInfo(item.m_nPublishedFileId, out punSizeOnDisk, out pchFolder, cchFolderSize, out punTimeStamp);
			
			Debug.Log($"Item installed: {item.m_nPublishedFileId} at {pchFolder}");
			
			// Now get the first file in the save directory
			string[] files = Directory.GetFiles(pchFolder);
			string fileContents = "";
			if (files.Length == 0 || files.Length > 1) {
				Debug.LogError("Download folder contains 0 or >1 files. Cannot return a single file: "+pchFolder);
			} else {
				fileContents = File.ReadAllText(files[0]);
			}
			
			onDownloadComplete.Invoke(fileContents);
		}
	}
}
using Backend.Managers;
using Steamworks;

namespace Backend.Submittable {
	public class SteamUploader {

		private SteamSubmittable submittable;
		
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

	}
}
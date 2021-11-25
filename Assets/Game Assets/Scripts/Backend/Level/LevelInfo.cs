using Steamworks;

namespace Backend.Level {
	public class LevelInfo {

		public PublishedFileId_t id;
		public string title;

		public LevelInfo(SteamUGCDetails_t queryDetails) {
			id = queryDetails.m_nPublishedFileId;
			title = queryDetails.m_rgchTitle;
		}

	}
}
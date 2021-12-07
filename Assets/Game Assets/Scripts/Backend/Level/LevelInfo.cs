using Newtonsoft.Json;
using Steamworks;

namespace Backend.Level {
	public class LevelInfo {

		public PublishedFileId_t id;
		public string title;

		[JsonConstructor]
		public LevelInfo() {

		}
		
		public LevelInfo(SteamUGCDetails_t queryDetails) {
			id = queryDetails.m_nPublishedFileId;
			title = queryDetails.m_rgchTitle;
		}

	}
}
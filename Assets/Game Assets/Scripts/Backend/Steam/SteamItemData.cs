using Newtonsoft.Json;
using Steamworks;

namespace Backend.Level {
	public class SteamItemData {

		public PublishedFileId_t id;
		public string title;

		[JsonConstructor]
		public SteamItemData() {

		}
		
		public SteamItemData(SteamUGCDetails_t queryDetails) {
			id = queryDetails.m_nPublishedFileId;
			title = queryDetails.m_rgchTitle;
		}

	}
}
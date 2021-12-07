using Backend.Submittable;

namespace Backend.Serialization {
	public interface ISteamSerializable {
		public void Save();

		public string fileExtension { get; }
		public string saveFolderName { get; }
		public string itemTypeTag { get;  }
		
	}
}
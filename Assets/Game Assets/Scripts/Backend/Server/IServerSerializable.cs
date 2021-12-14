using Backend.Submittable;

namespace Backend.Serialization {
	public interface IServerSerializable {
		public void Save();

		public string title { get;  }
		public string fileExtension { get; }
		public string saveFolderName { get; }

	}
}
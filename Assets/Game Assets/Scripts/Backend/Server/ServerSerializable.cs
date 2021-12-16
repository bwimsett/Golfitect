using Newtonsoft.Json;

namespace Backend.Serialization {
	public abstract class ServerSerializable {
		public abstract void Save();

		public abstract string name { get; }
		public abstract string description { get; }
		public abstract string fileExtension { get; }
		public abstract string saveFolderName { get; }
		
		public string GetJson() {
			Save();
			string json = JsonConvert.SerializeObject(this);
			return json;
		}

	}
}
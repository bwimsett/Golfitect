using Newtonsoft.Json;

namespace Backend.Serialization {
	public abstract class ServerSerializable {
		public abstract void Save();

		public string GetJson() {
			Save();
			string json = JsonConvert.SerializeObject(this);
			return json;
		}

	}
}
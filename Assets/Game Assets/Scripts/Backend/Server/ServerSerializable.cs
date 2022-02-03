using Newtonsoft.Json;

namespace Backend.Serialization {
	public class ServerSerializable {

		public string _id;

		public virtual void Save() {
			
		}

		public string GetJson() {
			Save();
			string json = JsonConvert.SerializeObject(this);
			return json;
		}

	}
}
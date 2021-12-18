using Newtonsoft.Json;

namespace Backend.Serialization {
	public class ServerSerializable {
		
		public string _id { get; private set; }

		public virtual void Save() {
			
		}

		public string GetJson() {
			Save();
			string json = JsonConvert.SerializeObject(this);
			return json;
		}

	}
}
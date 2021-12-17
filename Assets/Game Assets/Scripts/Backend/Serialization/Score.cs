using Newtonsoft.Json;

namespace Backend.Serialization {
	public class Score {
		public float time;
		public int score;

		public string GetJSON() {
			return JsonConvert.SerializeObject(this);
		}
	}
}
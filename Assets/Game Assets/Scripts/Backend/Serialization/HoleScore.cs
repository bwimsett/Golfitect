namespace Backend.Serialization {
	public class HoleScore : Score {
		public string holeid;

		public HoleScore(string holeid) {
			this.holeid = holeid;
		}
	}
}
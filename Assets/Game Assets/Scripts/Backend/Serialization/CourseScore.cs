namespace Backend.Serialization {
	public class CourseScore : Score {
		public string courseid;
		public int timerank, scorerank;

		public CourseScore(string courseid, float time, int score) {
			this.time = time;
			this.score = score;
			this.courseid = courseid;
		}
	}
}
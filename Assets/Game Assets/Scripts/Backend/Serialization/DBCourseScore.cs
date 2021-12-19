namespace Backend.Serialization {
	public class DBCourseScore : Score {
		public string courseid;
		public int timerank, scorerank;

		public DBCourseScore(string courseid, float time, int score) {
			this.time = time;
			this.score = score;
			this.courseid = courseid;
		}
	}
}
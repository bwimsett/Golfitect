namespace Backend.Managers {
	public class GameManager {

		public static Course.Course currentCourse;
		public static int currentCourseHoleIndex;
		public static Level.Level currentLevel { get; private set; }
		
		public static void SetCurrentLevel(Level.Level level) {
			currentLevel = level;
		}

	}
}
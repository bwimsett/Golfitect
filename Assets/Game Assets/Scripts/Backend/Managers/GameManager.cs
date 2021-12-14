using System.Runtime.CompilerServices;
using Backend.Course;
using Backend.Enums;
using Backend.Serialization;

namespace Backend.Managers {
	public class GameManager {

		public static CourseTracker courseTracker;
		
		public static Level.Level currentLevel { get; private set; }
		public static GameMode gameMode;
		private static UserScores userScores;

		public static UserScores GetUserScores() {
			if (userScores == null) {
				// First try and load user scores from steam
			
				// If not, create a new one
				userScores = new UserScores();
			}

			return userScores;
		}
		
		public static void SetCurrentLevel(Level.Level level) {
			currentLevel = level;
		}

		public static void StartCourse(Course.Course course) {
			courseTracker = new CourseTracker(course);
			SetCurrentLevel(courseTracker.GetCurrentLevel());
		}
		
	}
}
using System.Runtime.CompilerServices;
using Backend.Course;
using Backend.Enums;
using Backend.Serialization;
using UnityEngine.Events;

namespace Backend.Managers {
	public class GameManager {

		public static CourseTracker courseTracker;
		
		public static Level.Level currentLevel { get; private set; }
		public static GameMode gameMode;

		public static void SetCurrentLevel(Level.Level level) {
			currentLevel = level;
		}

		public static void StartCourse(Course.Course course, UnityAction onComplete) {
			courseTracker = new CourseTracker(course);
			SetCurrentLevel(courseTracker.GetCurrentLevel());
			courseTracker.LoadHighScores(()=> {
				courseTracker.LoadLeaderboards(onComplete);
			});
		}
		
	}
}
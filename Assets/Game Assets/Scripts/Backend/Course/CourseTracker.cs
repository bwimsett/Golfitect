using UnityEngine.Events;

namespace Backend.Course {
	public class CourseTracker {

		public Course course { get; }
		public int currentHoleIndex { get; private set; }

		private int[] holeScores;

		public UnityAction OnShotTaken;
		public UnityAction OnHoleFinished;

		public CourseTracker(Course course) {
			this.course = course;
			holeScores = new int[course.holes.Length];
			currentHoleIndex = 0;
		}

		public Level.Level GetCurrentLevel() {
			return course.holes[currentHoleIndex];
		}

		public void AddShot() {
			holeScores[currentHoleIndex]++;
			OnShotTaken.Invoke();
		}

		public int GetScoreForHole(int holeIndex) {
			return holeScores[holeIndex];
		}

		public int GetScoreForCurrentHole() {
			return GetScoreForHole(currentHoleIndex);
		}

		public void FinishHole() {
			OnHoleFinished.Invoke();
			currentHoleIndex++;
		}

	}
}
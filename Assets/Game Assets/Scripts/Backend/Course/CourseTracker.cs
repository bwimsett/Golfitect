using Backend.Managers;
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

		public int GetTotalShotsForCourse() {
			int total = 0;

			foreach (int score in holeScores) {
				total += score;
			}

			return total;
		}

		public int GetCurrentScoreForCourse(int holeIndex) {
			int score = 0;
			int par = 0;
			
			for (int i = 0; i <= holeIndex; i++) {
				par += course.holes[i].par;
				score += holeScores[i];
			}

			return score - par;
		}

		public int GetScoreForCurrentHole() {
			return GetScoreForHole(currentHoleIndex);
		}

		public void FinishHole() {
			currentHoleIndex++;

			if (currentHoleIndex >= course.holes.Length) {
				FinishCourse();
				return;
			}
			
			GameManager.SetCurrentLevel(course.holes[currentHoleIndex]);
			
			OnHoleFinished.Invoke();
		}

		public void FinishCourse() {
			
		}

	}
}
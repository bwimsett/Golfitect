using System;
using Backend.Managers;
using Backend.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Course {
	public class CourseTracker {

		public Course course { get; }
		public int currentHoleIndex { get; private set; }

		private HoleScore[] holeScores;
		public HoleScore[] highScores;

		public CourseScore courseScore { get; private set; }
		public CourseScore courseHighScore;
		
		public UnityAction OnShotTaken;
		public UnityAction OnHoleFinished;
		public UnityAction OnCourseFinished;
		public UnityAction OnCourseScoreSubmitted;

		public CourseTracker(Course course) {
			this.course = course;
			holeScores = new HoleScore[course.holes.Length];
			for (int i = 0; i < holeScores.Length; i++) {
				holeScores[i] = new HoleScore(course.holeIDs[i]);
			}
			currentHoleIndex = 0;
		}

		public Level.Level GetCurrentLevel() {
			return course.holes[currentHoleIndex];
		}

		public void AddShot() {
			holeScores[currentHoleIndex].score++;
			OnShotTaken.Invoke();
		}

		public int GetScoreForHole(int holeIndex) {
			return holeScores[holeIndex].score;
		}

		public float GetTimeForHole(int holeIndex) {
			return holeScores[holeIndex].time;
		}

		public int GetTotalShotsForCourse() {
			int total = 0;

			foreach (HoleScore score in holeScores) {
				total += score.score;
			}

			return total;
		}

		public void RefreshCourseScore() {
			float totalTime = 0;
			int coursePar = 0;
			int courseShots = 0;
			
			for (int i = 0; i < holeScores.Length; i++) {
				HoleScore score = holeScores[i];
				totalTime += score.time;
				coursePar += course.holes[i].par;
				courseShots += score.score;
			}

			int courseScore = courseShots - coursePar;

			this.courseScore = new CourseScore(course.courseInfo._id, totalTime, courseScore);
		}

		public int GetCurrentScoreForCourse(int holeIndex) {
			int score = 0;
			int par = 0;
			
			for (int i = 0; i <= holeIndex; i++) {
				par += course.holes[i].par;
				score += holeScores[i].score;
			}

			return score - par;
		}

		public int GetScoreForCurrentHole() {
			return GetScoreForHole(currentHoleIndex);
		}

		public void LoadHighScores(UnityAction onComplete) {
			bool foundHoleScores = false;
			bool foundCourseScore = false;
			
			GameSceneManager.serverManager.GetUserCourseScores(course, scores => {

				highScores = new HoleScore[course.holeIDs.Length];
				Debug.Log("High scores found: "+scores.Length);
				
				// Sort the high scores so they match the hole indices
				foreach (HoleScore score in scores) {
					for (int i = 0; i < course.holeIDs.Length; i++) {
						if (score.holeid.Equals(course.holeIDs[i])) {
							highScores[i] = score;
							break;
						}
					}
				}

				foundHoleScores = true;

				if (foundCourseScore) {
					onComplete.Invoke();
				}
			});
			
			GameSceneManager.serverManager.GetUserCourseScore(course.courseInfo, result => {
				courseHighScore = result;
				foundCourseScore = true;
				if (foundHoleScores) {
					onComplete.Invoke();
				}
			});
		}

		public void FinishHole() { 
			holeScores[currentHoleIndex].time = LevelManager.levelTimer.StopTimer();
			
			// Submit score to server
			GameSceneManager.serverManager.SubmitAndGetHighScore(holeScores[currentHoleIndex], result =>{Debug.Log(result);});
			
			currentHoleIndex++;
			
			if (currentHoleIndex >= course.holes.Length) {
				FinishCourse();
				return;
			}
			
			GameManager.SetCurrentLevel(course.holes[currentHoleIndex]);
			
			OnHoleFinished.Invoke();
		}

		public void FinishCourse() {
			RefreshCourseScore();
			
			// Submit the score
			GameSceneManager.serverManager.SubmitAndGetHighScore(courseScore, result => {
				courseHighScore = (CourseScore)result;
				OnCourseScoreSubmitted.Invoke();
			});
			
			OnCourseFinished.Invoke();
		}

	}
}
using Backend.Course;
using Backend.Managers;
using Backend.Serialization;
using Game;
using Game_Assets.Scripts.Backend.Server;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class CourseScoreSummary : MonoBehaviour {

		private DBCourseInfo courseInfo;
		private DBCourseScore lastScore, highScore;
		private DBUserScore[] timeLeaderboard, scoreLeaderboard;

		[Header("Times")] public TextMeshProUGUI time;
		public TextMeshProUGUI timeRecord;
		public TextMeshProUGUI timeRecordTitle;
		public TextMeshProUGUI timeRank;
		public MPImage recordBackground;

		[Header("Scores")]
		public TextMeshProUGUI score;
		public TextMeshProUGUI scoreRecord;
		public TextMeshProUGUI scoreRecordTitle; 
		public TextMeshProUGUI scoreRank;
		public MPImage scoreBackground;

		[Header("Leaderboards")] 
		public CourseScoreSummary_Leaderboard timeLeaderboardDisplay;
		public CourseScoreSummary_Leaderboard scoreLeaderboardDisplay;

		public void SetCourse(DBCourseInfo courseInfo, bool requestScores = false) {
			this.courseInfo = courseInfo;
		}

		public void Refresh() {
			if (lastScore != null) {
				time.text = LevelTimer.GetTimeString(lastScore.time);
				score.text = Course.GetScoreString(lastScore.score);

				timeRecord.color = timeRecordTitle.color = scoreRecord.color = scoreRecordTitle.color = Color.black;
				recordBackground.enabled = false;
				scoreBackground.enabled = false;

				if (lastScore.time <= highScore.time) {
					recordBackground.enabled = true;
					timeRecord.color = timeRecordTitle.color = Color.white;
					highScore.time = lastScore.time;
				}

				if (lastScore.score <= highScore.score) {
					scoreBackground.enabled = true;
					scoreRecord.color = scoreRecordTitle.color = Color.white;
					highScore.score = lastScore.score;
				}
			}

			if (highScore != null) {
				timeRecord.text = LevelTimer.GetTimeString(highScore.time);
				scoreRecord.text = Course.GetScoreString(highScore.score);

				scoreRank.text = timeRank.text = "...";

				if (highScore.scorerank > 0) {
					scoreRank.text = "#"+highScore.scorerank;
				}

				if (highScore.timerank > 0) {
					timeRank.text = "#"+highScore.timerank;
				}
				
			} else {
				timeRecord.text = "-:-";
				scoreRecord.text = "-";
			}
		}

		public void SetScores(DBCourseScore lastScore, DBCourseScore highScore) {
			this.lastScore = lastScore;
			this.highScore = highScore;

			if (highScore == null) {
				this.highScore = lastScore;
			}
		}

		public void SetLeaderboards(DBUserScore[] timeLeaderboard, DBUserScore[] scoreLeaderboard) {
			this.timeLeaderboard = timeLeaderboard;
			this.scoreLeaderboard = scoreLeaderboard;
			
			timeLeaderboardDisplay.SetLeaderboard(timeLeaderboard, CourseScoreSummary_Leaderboard_Type.Time);
			scoreLeaderboardDisplay.SetLeaderboard(scoreLeaderboard, CourseScoreSummary_Leaderboard_Type.Score);
		}
	}
}
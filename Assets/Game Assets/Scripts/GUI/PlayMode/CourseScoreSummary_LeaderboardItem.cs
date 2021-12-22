using Backend.Course;
using Game;
using Game_Assets.Scripts.Backend.Server;
using Game_Assets.Scripts.GUI.PlayerProfile;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class CourseScoreSummary_LeaderboardItem : MonoBehaviour {

		public TextMeshProUGUI score;
		public TextMeshProUGUI username;
		private DBUserScore userScore;
		public PlayerProfileTrigger profileTrigger;

		public void SetScore(DBUserScore userScore, CourseScoreSummary_Leaderboard_Type type) {
			this.userScore = userScore;
			
			if (userScore == null) {
				score.text = "";
				username.text = "";
				return;
			}
			
			profileTrigger.SetUser(userScore.user);
			
			if (type == CourseScoreSummary_Leaderboard_Type.Time) {
				this.score.text = LevelTimer.GetTimeString(userScore.score.time);
			} else {
				this.score.text = Course.GetScoreString(userScore.score.score);
			}

			username.text = userScore.user.username;
		}

	}
}
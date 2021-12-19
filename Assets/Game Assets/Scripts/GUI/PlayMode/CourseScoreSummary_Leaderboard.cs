using Game_Assets.Scripts.Backend.Server;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class CourseScoreSummary_Leaderboard : MonoBehaviour {

		public CourseScoreSummary_LeaderboardItem[] items;

		public void SetLeaderboard(DBUserScore[] scores, CourseScoreSummary_Leaderboard_Type type) {
			for (int i = 0; i < items.Length; i++) {
				if (scores == null) {
					items[i].SetScore(null, type);
					continue;
				}

				if (scores[i] == null) {
					items[i].gameObject.SetActive(false);
					continue;
				}
				
				items[i].gameObject.SetActive(true);
				items[i].SetScore(scores[i], type);
			}
		}

	}
	
	public enum CourseScoreSummary_Leaderboard_Type {Time, Score}
}
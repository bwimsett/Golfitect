using BWLocaliser;
using UnityEngine;

namespace Backend.Level {
	public class LevelUtitlity {

		public static string GetScoreStringID(int score, int par) {
			if (score == 1) {
				return "score_ace";
			}

			int golfScore = Mathf.Min(12,Mathf.Max(-4,score - par));

			if (golfScore < 0) {
				return "score_minus_" + Mathf.Abs(golfScore);
			}
			
			return "score_plus_" + Mathf.Abs(golfScore);
		}
		
	}
}
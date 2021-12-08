using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class ScoreCard_Score: MonoBehaviour {
		[SerializeField] private MPImage containerBox;
		[SerializeField] private TextMeshProUGUI score;
		[SerializeField] private string overParColorID, parColorID, belowParColorID, holeInOneColorID;

		public void Clear() {
			containerBox.StrokeWidth = 3f;
			score.text = "";
			containerBox.color = LevelManager.playModeHUD.colorBank.GetColor(parColorID);
		}
		
		public void SetScore(int score, int par) {
			containerBox.StrokeWidth = 0f;
			
			Color color = LevelManager.playModeHUD.colorBank.GetColor(parColorID);
			
			if (score > par) {
				color = LevelManager.playModeHUD.colorBank.GetColor(overParColorID);
			}

			if (score == par) {
				color = LevelManager.playModeHUD.colorBank.GetColor(parColorID);
			}

			if (score < par) {
				color = LevelManager.playModeHUD.colorBank.GetColor(belowParColorID);
			}

			if (score <= 1) {
				color = LevelManager.playModeHUD.colorBank.GetColor(holeInOneColorID);
			}

			this.score.text = "" + score;
			containerBox.color = color;
		}

	}
}
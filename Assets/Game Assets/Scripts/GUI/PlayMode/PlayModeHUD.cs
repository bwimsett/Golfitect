using System.Collections.Generic;
using Backend.Managers;
using Game_Assets.Scripts.GUI.GenericComponent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PlayModeHUD : GameHUD {

		[Header("Stroke Counter")] 
		public TextMeshProUGUI strokesText;
		public TextLocalizer parText;

		[Header("Score Card")] 
		public ScoreCard scoreCard;

		[Header("Timer")] 
		public TextMeshProUGUI timer;

		[Header("Level Summary")] public PlayMode_LevelSummary levelSummary;
		
		protected override void OpenGameHUD() {
			GameManager.courseTracker.OnShotTaken += RefreshStrokes;
			GameManager.courseTracker.OnHoleFinished += ShowHoleSummary;
			levelSummary.Close(false);
			Refresh();
		}

		private void Refresh() {
			RefreshStrokes();
			RefreshPar();
			scoreCard.Refresh();
			LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
		}

		private void RefreshStrokes() {
			strokesText.text = "" + GameManager.courseTracker.GetScoreForCurrentHole();
		}

		private void RefreshPar() {
			string par = ""+GameManager.courseTracker.GetCurrentLevel().par;
			parText.SetFields(new Dictionary<string, object>(){{"par", par}});
		}

		private void ShowHoleSummary() {
			levelSummary.Open();
		}
	}
}
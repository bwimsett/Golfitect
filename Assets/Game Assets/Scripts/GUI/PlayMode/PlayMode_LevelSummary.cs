using Backend.Level;
using Backend.Managers;
using BWLocaliser;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PlayMode_LevelSummary : MonoBehaviour {

		public CanvasGroup canvasGroup;
		public TextLocalizer scoreTitle, courseAndHole;
		public ScoreCard scoreCard;
		public Image headerBackground;
		public StripeButton nextButton;

		[Header("Animation Settings")] 
		public float openDuration, closeDuration;
		public Ease openEase;
		public Vector3 startScale;
		
		public void Open() {
			Refresh();
			transform.localScale = startScale;
			canvasGroup.alpha = 0;
			transform.DOScale(Vector3.one, openDuration).SetEase(openEase);
			canvasGroup.DOFade(1, openDuration).OnComplete(() => {
				canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
			});
		}
		
		public void Refresh() {
			scoreTitle.SetString(new LocString(LevelUtitlity.GetScoreStringID(GameManager.courseTracker.GetScoreForCurrentHole(), GameManager.currentLevel.par)));
			scoreCard.Refresh();
		}

		public void Close(bool animate = true) {
			canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
			
			if (animate) {
				canvasGroup.DOFade(0, closeDuration);
			} else {
				canvasGroup.alpha = 0;
			}
		}
		
	}
}
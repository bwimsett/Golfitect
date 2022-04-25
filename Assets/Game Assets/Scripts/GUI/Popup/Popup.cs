using Backend.Managers;
using DG.Tweening;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class Popup : MonoBehaviour {
		
		public CanvasGroup canvasGroup;
		
		[Header("Animation Settings")] 
		public float openDuration, closeDuration;
		public Ease openEase;
		public Vector3 startScale;
		
		public void Open() {
			Refresh();
			GameSceneManager.popupManager.DisableCanvases();
			transform.localScale = startScale;
			canvasGroup.alpha = 0;
			transform.DOScale(Vector3.one, openDuration).SetEase(openEase);
			canvasGroup.DOFade(1, openDuration).OnComplete(() => {
				canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
			});
		}

		public virtual void Refresh() {

		}

		public void Close(bool animate = true) {
			canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
			
			GameSceneManager.popupManager.EnableCanvases();

			if (animate) {
				canvasGroup.DOFade(0, closeDuration);
			} else {
				canvasGroup.alpha = 0;
			}
		}
		
	}
}
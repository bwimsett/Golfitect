using System.Diagnostics.Contracts;
using DG.Tweening;
using UnityEngine;

namespace Game_Assets.Scripts.GUI {
	public class GameHUD : MonoBehaviour {

		[SerializeField] private CanvasGroup _canvasGroup;
		private float fadeDuration = 0.25f;
		public GUIColorBank colorBank;
		
		public void Open() {
			if (LevelManager.currentGameHUD) {
				LevelManager.currentGameHUD.Close();
			}

			LevelManager.currentGameHUD = this;
			OpenGameHUD();
			_canvasGroup.DOFade(1, fadeDuration).OnComplete(() => {
				_canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
			});
		}

		protected virtual void OpenGameHUD() {
			
		}

		public void Close(bool animateFade = true) {
			if (animateFade) {
				_canvasGroup.DOFade(0, fadeDuration);
			} else {
				_canvasGroup.alpha = 0;
			}
			
			_canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
			CloseGameHUD();
		}

		protected virtual void CloseGameHUD() {
			
		}

	}
}
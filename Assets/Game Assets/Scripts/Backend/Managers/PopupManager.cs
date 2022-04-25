using Game_Assets.Scripts.GUI.PlayMode;
using UnityEngine;

namespace Backend.Managers {
	public class PopupManager : MonoBehaviour {

		[SerializeField] private GameObject popupPrefab;
		[SerializeField] private Canvas popupContainer;

		[SerializeField] private CanvasGroup[] disableCanvases;
		private bool[] canvasesEnabledState;
		private bool canvasesDisabled;
		
		public PopupAlert CreatePopup() {
			PopupAlert popup = Instantiate(popupPrefab, popupContainer.transform).GetComponent<PopupAlert>();
			popup.Open();
			return popup;
		}

		public void DisableCanvases() {
			if (canvasesDisabled) {
				return;
			}

			canvasesEnabledState = new bool[disableCanvases.Length];
			
			for (int i = 0; i < canvasesEnabledState.Length; i++) {
				canvasesEnabledState[i] = disableCanvases[i].interactable;
				disableCanvases[i].interactable = disableCanvases[i].blocksRaycasts = false;
			}

			canvasesDisabled = true;
		}

		public void EnableCanvases() {
			if (!canvasesDisabled) {
				return;
			}

			for (int i = 0; i < canvasesEnabledState.Length; i++) {
				disableCanvases[i].interactable = disableCanvases[i].blocksRaycasts = canvasesEnabledState[i];
			}

			canvasesDisabled = false;
		}

	}
}
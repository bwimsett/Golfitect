using Game_Assets.Scripts.GUI.PlayMode;
using UnityEngine;

namespace Backend.Managers {
	public class PopupManager : MonoBehaviour {

		[SerializeField] private GameObject popupPrefab;
		[SerializeField] private Canvas popupContainer;
		
		public PopupAlert CreatePopup() {
			PopupAlert popup = Instantiate(popupPrefab, popupContainer.transform).GetComponent<PopupAlert>();
			popup.Open();
			return popup;
		}
		
	}
}
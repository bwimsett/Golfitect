using BWLocaliser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PopupAlert : Popup {

		[SerializeField] private Button option1, option2, option3;
		[SerializeField] private TextLocalizer mainText, option1Text, option2Text, option3Text;

		public void SetValues(LocString popupText, PopupAlertCallback option1, bool option2Cancel = true) {
			SetValues(popupText);
			
			option1Text.SetString(option1.locString);
			this.option1.onClick.AddListener(WrapActionWithClose(option1.callback));

			if (option2Cancel) {
				option2Text.SetString(new LocString("generic_cancel"));
				this.option2.onClick.AddListener(Cancel);
			}
			
			option2.gameObject.SetActive(option2Cancel);
			option3.gameObject.SetActive(false);
		}

		public void SetValues(LocString popupText) {
			this.option1.onClick.RemoveAllListeners();
			this.option2.onClick.RemoveAllListeners();
			
			option1Text.SetString(new LocString("generic_cancel"));
			this.option1.onClick.AddListener(Cancel);
			
			mainText.SetString(popupText);
			
			this.option2.gameObject.SetActive(false);
		}

		public void Cancel() {
			Close();
		}

		private UnityAction WrapActionWithClose(UnityAction action) {
			return () => {
				action.Invoke();
				Close();
			};
		}
		
	}
}
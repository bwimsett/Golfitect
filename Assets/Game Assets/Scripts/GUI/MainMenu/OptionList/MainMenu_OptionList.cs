using System.Collections.Generic;
using Game_Assets.Scripts.GUI.GenericComponent;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.OptionList {
	public class MainMenu_OptionList : MainMenu_Subwindow {

		public MainMenu_OptionList_Option optionTemplate;
		public Transform buttonContainer;
		private List<MainMenu_OptionList_Option> optionButtons;

		void Awake() {
			optionTemplate.gameObject.SetActive(false);
		}
		
		public void SetOptions(TextButtonCallback[] options) {
			if (options == null) {
				optionButtons = new List<MainMenu_OptionList_Option>();
			}

			Clear();

			foreach (TextButtonCallback option in options) {
				MainMenu_OptionList_Option optionButton = Instantiate(optionTemplate, buttonContainer).GetComponent<MainMenu_OptionList_Option>();
				optionButton.gameObject.SetActive(true);
				optionButton.SetOption(option);
				optionButtons.Add(optionButton);
			}
		}
		
		private void Clear() {
			if (optionButtons == null) {
				optionButtons = new List<MainMenu_OptionList_Option>();
			}
			
			foreach (MainMenu_OptionList_Option option in optionButtons) {
				Destroy(option.gameObject);
			}

			optionButtons = new List<MainMenu_OptionList_Option>();
		}

	}
}
using System.Collections.Generic;
using Game_Assets.Scripts.GUI.GenericComponent;
using Game_Assets.Scripts.GUI.OptionList;
using GUI.MainMenu;
using UnityEngine;

namespace GUI.OptionList {
	public class MainMenu_OptionList : MainMenu_Subwindow {

		public MainMenu_OptionList_Option optionTemplate;
		public Transform buttonContainer;
		private List<MainMenu_OptionList_Option> optionButtons;

		private TextButtonCallback[] optionCallbacks;
		

		void Awake() {
			optionTemplate.gameObject.SetActive(false);
		}
		
		public void SetOptions(TextButtonCallback[] options, bool refresh = false) {
			optionCallbacks = options;

			if (refresh) {
				Refresh();
			}
		}

		protected override void Refresh() {
			base.Refresh();
			if (optionCallbacks == null) {
				optionButtons = new List<MainMenu_OptionList_Option>();
			}

			Clear();

			foreach (TextButtonCallback option in optionCallbacks) {
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
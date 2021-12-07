using BWLocaliser;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.OptionList {
	public class MainMenu_OptionList_Option : ShiftXOnMouseover {

		public TextLocalizer text;
		public Button button;
		
		public void SetOption(TextButtonCallback callback) {
			text.SetString(callback.textID);
			button.onClick.AddListener(callback.action);
		}
		
	}
}
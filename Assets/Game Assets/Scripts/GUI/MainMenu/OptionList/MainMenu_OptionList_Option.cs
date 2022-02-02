using BWLocaliser;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.OptionList {
	public class MainMenu_OptionList_Option : ShiftXOnMouseover {

		public TextLocalizer text;
		private TextButtonCallback onClickCallback;
		
		public void SetOption(TextButtonCallback callback) {
			text.SetString(callback.textID);
			this.onClickCallback = callback;
		}

		public void OnClick() {
			onClickCallback.action.Invoke();
		}

	}
}
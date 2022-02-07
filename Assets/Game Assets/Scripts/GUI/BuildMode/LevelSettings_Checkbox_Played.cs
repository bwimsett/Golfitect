using System;
using Backend.Managers;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class LevelSettings_Checkbox_Played : LevelSettings_Checkbox {

		public BuildMode_LevelSettings levelSettingsDisplay;
		
		void Start() {
			GameManager.currentLevel.OnCompletableChanged.AddListener(OnCompletableChanged);
		}

		private void OnDestroy() {
			GameManager.currentLevel.OnCompletableChanged.RemoveListener(OnCompletableChanged);
		}

		private void OnCompletableChanged(bool value) {
			SetValue(value);
			levelSettingsDisplay.RefreshSaveButton();	
		}
		
	}
}
using System;
using Backend.Managers;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class LevelSettings_Checkbox_Played : LevelSettings_Checkbox {

		void Start() {
			GameManager.currentLevel.OnCompletableChanged.AddListener(SetValue);
		}

		private void OnDestroy() {
			GameManager.currentLevel.OnCompletableChanged.RemoveListener(SetValue);
		}
		
	}
}
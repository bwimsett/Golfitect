using Backend.Managers;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class CustomisationOption_Height : BuildMode_CustomisationOption {

		[SerializeField] private Spinner spinner;
		
		void Awake() {
			spinner.SetMinMax(1, GameManager.currentLevel.levelDimensions.y);
			spinner.OnValueChanged += OnValueChanged;
		}

		private void OnValueChanged(int value) {
			LevelManager.levelInputManager.levelBuilderTool.SetTileHeight(value);
		}
		
	}
}
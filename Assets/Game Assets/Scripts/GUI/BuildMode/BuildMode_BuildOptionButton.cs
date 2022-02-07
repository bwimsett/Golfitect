using Backend.Enums;
using Backend.Level;
using Game_Assets.Scripts.GUI.GenericComponent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class BuildMode_BuildOptionButton : GUIScaleOnMouseover {

		public LevelObject levelObject { get; private set; }
		public Image icon;
		private BuildModeHUD hud;

		private UnityAction onSelect, onDeselect;

		public void SetLevelObject(LevelObject levelObject, BuildModeHUD hud) {
			this.hud = hud;
			this.levelObject = levelObject;
			Refresh();
		}

		public void SetCallbacks(UnityAction onSelect, UnityAction onDeselect, Sprite icon, BuildModeHUD hud) {
			this.hud = hud;
			this.icon.sprite = icon;
			this.onSelect = onSelect;
			this.onDeselect = onDeselect;
		}

		private void Refresh() {
			icon.sprite = levelObject.buildMenuIcon;
		}

		public void OnSelect() {
			// Ignore right clicks
			if (Input.GetMouseButtonDown(1)) {
				return;
			}

			hud.SelectBuildOptionFromDock(this);
			
			if (onSelect != null) {
				onSelect.Invoke();
				return;
			}
			
			LevelManager.levelInputManager.levelBuilderTool.SetActive(true);
			LevelManager.levelInputManager.levelBuilderTool.SetLevelObject(levelObject);
			ShowCustomisationOptions();
		}

		public void OnDeselect() {
			hud.DeselectBuildOptionFromDock(this);

			if (onDeselect != null) {
				onDeselect.Invoke();
				return;
			}

			LevelManager.levelInputManager.levelBuilderTool.SetActive(false);
		}

		private void ShowCustomisationOptions() {
			if (levelObject.levelObjectClass != LevelObjectClass.Tile) {
				return;
			}
			
			hud.ShowCustomisationOption(typeof(CustomisationOption_Height));
		}
	}
}
using System;
using Backend.Enums;
using Backend.Level;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class BuildMode_BuildOptionButton : GUIScaleOnMouseover {

		public LevelObject levelObject { get; private set; }
		public Image icon;
		private BuildModeHUD hud;

		public void SetLevelObject(LevelObject levelObject, BuildModeHUD hud) {
			this.hud = hud;
			this.levelObject = levelObject;
			Refresh();
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
			LevelManager.levelInputManager.levelBuilderTool.SetActive(true);
			LevelManager.levelInputManager.levelBuilderTool.SetLevelObject(levelObject);
			ShowCustomisationOptions();
		}

		public void OnDeselect() {
			hud.DeselectBuildOptionFromDock(this);
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
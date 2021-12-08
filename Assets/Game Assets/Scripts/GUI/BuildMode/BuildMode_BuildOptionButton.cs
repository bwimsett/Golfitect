using System;
using Backend.Level;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using UnityEngine;
using UnityEngine.UI;

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
			hud.SelectBuildOptionFromDock(this);
			LevelManager.levelInputManager.levelBuilderTool.SetActive(true);
			LevelManager.levelInputManager.levelBuilderTool.SetLevelObject(levelObject);
			
		}

		public void OnDeselect() {
			hud.DeselectBuildOptionFromDock(this);
			LevelManager.levelInputManager.levelBuilderTool.SetActive(false);
		}
	}
}
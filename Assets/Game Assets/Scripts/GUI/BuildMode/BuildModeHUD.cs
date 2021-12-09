using System.Collections;
using System.Collections.Generic;
using Backend.Enums;
using Backend.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class BuildModeHUD : GameHUD {
		
		[SerializeField] private Transform dockPrefab;
		[SerializeField] private RectTransform dockContainer;
		private Transform tileDock, sceneryDock;
		[SerializeField] private BuildMode_BuildOptionButton buildModeBuildOptionButtonPrefab;
		private BuildMode_BuildOptionButton _currentSelectedBuildModeBuildOption;
		
		
		protected override void OpenGameHUD() {
			GenerateBuildOptions();
		}
		
		private void GenerateBuildOptions() {
			buildModeBuildOptionButtonPrefab.gameObject.SetActive(false);
			dockPrefab.gameObject.SetActive(false);
			
			foreach (LevelObject levelObject in LevelManager.levelObjectUtility.objectBank.levelObjects) {
				if (levelObject.showInBuildMenuDock) {
					AddLevelObjectToDock(levelObject);
				}
			}

			StartCoroutine(RebuildAtEndOfFrame());
		}

		IEnumerator RebuildAtEndOfFrame() {
			yield return new WaitForEndOfFrame();
			LayoutRebuilder.ForceRebuildLayoutImmediate(dockContainer);
		}

		private void AddLevelObjectToDock (LevelObject levelObject) {
			Transform dock = tileDock;

			if (levelObject.levelObjectClass == LevelObjectClass.Tile && !tileDock) {
				dock = tileDock = Instantiate(dockPrefab.gameObject, dockPrefab.parent).transform;
				dock.gameObject.SetActive(true);
			}
			
			if (levelObject.levelObjectClass == LevelObjectClass.Scenery && !sceneryDock) {
				dock = sceneryDock = Instantiate(dockPrefab.gameObject, dockPrefab.parent).transform;
				dock.gameObject.SetActive(true);
			} else if (levelObject.levelObjectClass == LevelObjectClass.Scenery) {
				dock = sceneryDock;
			}
			
			BuildMode_BuildOptionButton button = Instantiate(buildModeBuildOptionButtonPrefab.gameObject, dock).GetComponent<BuildMode_BuildOptionButton>();
			button.gameObject.SetActive(true);
			
			button.SetLevelObject(levelObject, this);
		}

		public void SelectBuildOptionFromDock (BuildMode_BuildOptionButton modeBuildOption) {
			if (_currentSelectedBuildModeBuildOption) {
				_currentSelectedBuildModeBuildOption.Deselect();
			}

			_currentSelectedBuildModeBuildOption = modeBuildOption;

		}

		public void DeselectBuildOptionFromDock(BuildMode_BuildOptionButton modeBuildOption) {
			if (_currentSelectedBuildModeBuildOption == modeBuildOption) {
				_currentSelectedBuildModeBuildOption.Deselect();
				_currentSelectedBuildModeBuildOption = null;
			}
		}

		public void DeselectBuildOptionFromDock(LevelObject prefab) {
			if (_currentSelectedBuildModeBuildOption.levelObject.objectTypeID.Equals(prefab.objectTypeID)) {
				DeselectBuildOptionFromDock(_currentSelectedBuildModeBuildOption);
			}
		}

	}
}
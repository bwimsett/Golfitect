using System;
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
		public BuildMode_LevelSettings levelSettings;

		[SerializeField] private RectTransform customisationOptionContainer;
		[SerializeField] private BuildMode_CustomisationOption[] customisationOptions;
		
		protected override void OpenGameHUD() {
			GenerateBuildOptions();
			levelSettings.Refresh();
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

		public void SelectBuildOptionFromDock(BuildMode_BuildOptionButton modeBuildOption) {
			if (_currentSelectedBuildModeBuildOption) {
				DeselectBuildOptionFromDock(_currentSelectedBuildModeBuildOption);
			}

			_currentSelectedBuildModeBuildOption = modeBuildOption;
			
			UpdateCustomisationOptionContainerPosition();
		}

		public void DeselectBuildOptionFromDock(BuildMode_BuildOptionButton modeBuildOption) {
			if (_currentSelectedBuildModeBuildOption == modeBuildOption) {
				_currentSelectedBuildModeBuildOption.Deselect();
				_currentSelectedBuildModeBuildOption = null;
				ClearCustomisationOptions();
			}
		}

		public void DeselectBuildOptionFromDock(LevelObject prefab) {
			if (!_currentSelectedBuildModeBuildOption) {
				return;
			}
		
			if (_currentSelectedBuildModeBuildOption.levelObject.objectTypeID.Equals(prefab.objectTypeID)) {
				DeselectBuildOptionFromDock(_currentSelectedBuildModeBuildOption);
			}
		}

		public void ShowCustomisationOption(Type customisationOptionType) {
			foreach (BuildMode_CustomisationOption customisationOption in customisationOptions) {
				if (customisationOption.GetType() == customisationOptionType) {
					customisationOption.gameObject.SetActive(true);
					return;
				}
			}
		}

		public void ClearCustomisationOptions() {
			foreach (BuildMode_CustomisationOption option in customisationOptions) {
				option.gameObject.SetActive(false);
			}
		}

		private void UpdateCustomisationOptionContainerPosition() {
			if (!_currentSelectedBuildModeBuildOption) {
				return;
			}

			customisationOptionContainer.transform.position = new Vector3(_currentSelectedBuildModeBuildOption.transform.position.x, customisationOptionContainer.transform.position.y);
		}

	}
}
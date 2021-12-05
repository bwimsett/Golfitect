using System.Collections;
using System.Collections.Generic;
using Backend.Enums;
using Backend.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class LevelBuilderHUDManager : MonoBehaviour {
		
		[SerializeField] private Transform dockPrefab;
		[SerializeField] private RectTransform dockContainer;
		private Transform tileDock, sceneryDock;
		[SerializeField] private LevelBuilder_BuildOptionButton buildOptionButtonPrefab;
		private LevelBuilder_BuildOptionButton currentSelectedBuildOption;
		
		
		public void Initialise() {
			GenerateBuildOptions();
		}
		
		private void GenerateBuildOptions() {
			buildOptionButtonPrefab.gameObject.SetActive(false);
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
			}
			
			LevelBuilder_BuildOptionButton button = Instantiate(buildOptionButtonPrefab.gameObject, dock).GetComponent<LevelBuilder_BuildOptionButton>();
			button.gameObject.SetActive(true);
			
			button.SetLevelObject(levelObject, this);
		}

		public void SelectBuildOptionFromDock (LevelBuilder_BuildOptionButton option) {
			if (currentSelectedBuildOption) {
				currentSelectedBuildOption.Deselect();
			}

			currentSelectedBuildOption = option;

		}

		public void DeselectBuildOptionFromDock(LevelBuilder_BuildOptionButton option) {
			if (currentSelectedBuildOption == option) {
				currentSelectedBuildOption.Deselect();
				currentSelectedBuildOption = null;
			}
		}

		public void DeselectBuildOptionFromDock(LevelObject prefab) {
			if (currentSelectedBuildOption.levelObject.objectTypeID.Equals(prefab.objectTypeID)) {
				DeselectBuildOptionFromDock(currentSelectedBuildOption);
			}
		}

	}
}
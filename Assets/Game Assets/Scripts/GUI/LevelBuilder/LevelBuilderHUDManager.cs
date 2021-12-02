using Backend.Enums;
using Backend.Level;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class LevelBuilderHUDManager : MonoBehaviour {
		
		[SerializeField] private Transform tileDock;
		private Transform sceneryDock;
		[SerializeField] private LevelBuilder_BuildOptionButton buildOptionButtonPrefab;
		private LevelBuilder_BuildOptionButton currentSelectedBuildOption;
		
		
		public void Initialise() {
			GenerateBuildOptions();
		}

		private void GenerateBuildOptions() {
			buildOptionButtonPrefab.gameObject.SetActive(false);
			
			foreach (LevelObject levelObject in LevelManager.levelObjectUtility.objectBank.levelObjects) {
				if (levelObject.showInBuildMenuDock) {
					AddLevelObjectToDock(levelObject);
				}
			}
		}

		private void AddLevelObjectToDock (LevelObject levelObject) {
			Transform dock = tileDock;
			
			if (levelObject.levelObjectClass == LevelObjectClass.Scenery && !sceneryDock) {
				dock = sceneryDock = Instantiate(tileDock.gameObject, tileDock.parent).transform;
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

	}
}
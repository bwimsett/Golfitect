using Backend.Level;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelBuilder.BuildOptions {
	public class BuildMode_BuildOptions_Option : MonoBehaviour {

		private LevelObject levelObject;
		
		public void SetLevelObject(LevelObject levelObject) {
			this.levelObject = levelObject;
		}

		public void OnClick() {
			LevelManager.levelInputManager.levelBuilderTool.SetActive(true);
			LevelManager.levelInputManager.levelBuilderTool.SetLevelObject(levelObject);
		}
		
		
	}
}
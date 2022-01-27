using Backend.Managers;
using Game_Assets.Scripts.GUI.GenericComponent;

namespace Game_Assets.Scripts.GUI.LevelBuilder {
	public class BuildMode_PlayButton : GUIScaleOnMouseover {

		public void OnSelected() {
			GameManager.currentLevel.Play();
		}

		public void OnDeselected() {
			GameManager.currentLevel.BuildMode();
		}
		
	}
}
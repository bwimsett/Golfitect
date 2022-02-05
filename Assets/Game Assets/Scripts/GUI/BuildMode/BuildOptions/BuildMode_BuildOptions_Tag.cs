using Backend.Level;
using BWLocaliser;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.LevelBuilder.BuildOptions {
	public class BuildMode_BuildOptions_Tag : MonoBehaviour {

		public Toggle toggle;
		public TextLocalizer textLocalizer;
		public LevelObjectTag tag;
		private BuildMode_BuildOptions buildOptionsDisplay;
		
		public void SetTag(LevelObjectTag tag, BuildMode_BuildOptions buildOptionsDisplay) {
			this.buildOptionsDisplay = buildOptionsDisplay;
			this.tag = tag;
			textLocalizer.SetString(new LocString(tag.id));
		}

		public void OnToggle() {
			buildOptionsDisplay.RefreshSelectedTags();
		}

	}
}
using Backend.Level;
using TMPro;

namespace Game_Assets.Scripts.GUI.ReorderableList {
	public class GUIReorderableList_Hole : GUIReorderableList_Item {
		
		public TextMeshProUGUI text;
		
		public override void SetItem(object obj) {
			base.SetItem(obj);

			LevelInfo levelInfo = (LevelInfo) obj; ;
			text.text = levelInfo.title;
		}
	}
}
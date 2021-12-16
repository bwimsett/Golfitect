using Backend.Level;
using Game_Assets.Scripts.Backend.Server;
using TMPro;

namespace Game_Assets.Scripts.GUI.ReorderableList {
	public class GUIReorderableList_Hole : GUIReorderableList_Item {
		
		public TextMeshProUGUI text;
		
		public override void SetItem(object obj) {
			base.SetItem(obj);

			DBHoleInfo dbHoleInfo = (DBHoleInfo) obj;
			text.text = dbHoleInfo.name;
		}
	}
}
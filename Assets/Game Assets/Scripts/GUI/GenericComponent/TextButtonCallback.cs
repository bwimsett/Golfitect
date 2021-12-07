using BWLocaliser;
using UnityEngine.Events;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class TextButtonCallback {

		public LocString textID;
		public UnityAction action;

		public TextButtonCallback(string textID, UnityAction action) {
			this.textID = new LocString(textID);
			this.action = action;
		}

	}
}
using BWLocaliser;
using UnityEngine.Events;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PopupAlertCallback {

		public UnityAction callback;
		public  LocString locString;
		
		public PopupAlertCallback(UnityAction callback, LocString locString) {
			this.callback = callback;
			this.locString = locString;
		}
		
	}
}
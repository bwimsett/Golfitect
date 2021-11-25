using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelOptionGrid {
	public class LevelOptionGrid_Option : MonoBehaviour {

		private Vector2 normalPosition;
		public RectTransform rectTransform;
		private object obj;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(object obj) {
			this.obj = obj;
		}

	}
}
using Backend.Level;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelOptionGrid {
	public class LevelOptionGrid_Option : MonoBehaviour {

		private Vector2 normalPosition;
		public RectTransform rectTransform;
		public TextMeshProUGUI nameText;
		public CanvasGroup canvasGroup;
		private object obj;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(object obj) {
			this.obj = obj;
			Refresh();
		}

		private void Refresh() {
			if (obj == null) {
				canvasGroup.alpha = 0;
				return;
			}

			canvasGroup.alpha = 1;
			
			if (obj is LevelInfo levelInfo) {
				RefreshLevel(levelInfo);
			}	
		}

		private void RefreshLevel(LevelInfo levelInfo) {
			nameText.text = levelInfo.title;
		}

		private void RefreshCourse() {
			
		}

	}
}
using System;
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
		private MainMenu_Subwindow subwindow;

		[Header("Options for displaying holes")] [SerializeField]
		private AutoResizeSwitchButton addHoleButton;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(object obj, MainMenu_Subwindow subwindow) {
			this.obj = obj;
			this.subwindow = subwindow;
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
			addHoleButton.gameObject.SetActive(true);
			addHoleButton.OnClickAction = b => {
				if (subwindow is CourseCreator.CourseCreator courseCreator) {
					if (b) {
						courseCreator.holesList.RemoveHoleFromList(levelInfo);
					} else {
						courseCreator.holesList.AddHoleToList(levelInfo);
					}
				}
			};
		}

		private void RefreshCourse() {
			
		}

	}
}
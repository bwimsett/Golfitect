using System;
using Backend.Course;
using Backend.Level;
using Backend.Submittable;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.LevelOptionGrid {
	public class LevelOptionGrid_Option : MonoBehaviour {

		private Vector2 normalPosition;
		public RectTransform rectTransform;
		public TextMeshProUGUI nameText;
		public CanvasGroup canvasGroup;
		private LevelInfo levelInfo;
		private MainMenu_Subwindow subwindow;

		[Header("Options for displaying holesInfo")] [SerializeField]
		private AutoResizeSwitchButton addHoleButton;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(LevelInfo levelInfo, MainMenu_Subwindow subwindow) {
			this.levelInfo = levelInfo;
			this.subwindow = subwindow;
			Refresh();
		}

		private void Refresh() {
			if (this.levelInfo == null) {
				canvasGroup.alpha = 0;
				return;
			}

			canvasGroup.alpha = 1;

			if (this.levelInfo is CourseInfo courseInfo) {
				RefreshCourse(courseInfo);
				return;
			}
			
			if (this.levelInfo is LevelInfo levelInfo) {
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

		private void RefreshCourse(CourseInfo courseInfo) {
			addHoleButton.gameObject.SetActive(false);
		}

		public void OnClick() {
			LoadingScreenManager.Load(levelInfo);
		}

	}
}
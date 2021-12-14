using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Game_Assets.Scripts.GUI;
using GUI.MainMenu;
using GUI.MainMenu.CourseCreator;
using TMPro;
using UnityEngine;

namespace GUI.LevelOptionGrid {
	public class LevelOptionGrid_Option : MonoBehaviour {

		private Vector2 normalPosition;
		public RectTransform rectTransform;
		public TextMeshProUGUI nameText;
		public CanvasGroup canvasGroup;
		private SteamItemData _steamItemData;
		private MainMenu_Subwindow subwindow;

		[Header("Options for displaying holesInfo")] [SerializeField]
		private AutoResizeSwitchButton addHoleButton;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(SteamItemData steamItemData, MainMenu_Subwindow subwindow) {
			this._steamItemData = steamItemData;
			this.subwindow = subwindow;
			Refresh();
		}

		private void Refresh() {
			if (this._steamItemData == null) {
				canvasGroup.alpha = 0;
				return;
			}

			canvasGroup.alpha = 1;

			if (this._steamItemData is SteamCourseData courseInfo) {
				RefreshCourse(courseInfo);
				return;
			}
			
			if (this._steamItemData is SteamItemData levelInfo) {
				RefreshLevel(levelInfo);
			}
			
		}

		private void RefreshLevel(SteamItemData steamItemData) {
			nameText.text = steamItemData.title;
			addHoleButton.gameObject.SetActive(true);
			addHoleButton.OnClickAction = b => {
				if (subwindow is CourseCreator courseCreator) {
					if (b) {
						courseCreator.holesList.RemoveHoleFromList(steamItemData);
					} else {
						courseCreator.holesList.AddHoleToList(steamItemData);
					}
				}
			};
		}

		private void RefreshCourse(SteamCourseData steamCourseData) {
			addHoleButton.gameObject.SetActive(false);
		}

		public void OnClick() {
			LoadingScreenManager.Load(_steamItemData, GameMode.Play);
		}

	}
}
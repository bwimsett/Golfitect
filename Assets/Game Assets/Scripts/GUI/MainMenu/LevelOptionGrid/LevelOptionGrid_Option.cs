using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Game_Assets.Scripts.Backend.Server;
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
		private DBObject _dbObject;
		private MainMenu_Subwindow subwindow;

		[Header("Options for displaying holesInfo")] [SerializeField]
		private AutoResizeSwitchButton addHoleButton;
		
		public void SetNormalPosition(Vector2 position) {
			normalPosition = position;
		}

		public void SetObject(DBObject steamItemData, MainMenu_Subwindow subwindow) {
			this._dbObject = steamItemData;
			this.subwindow = subwindow;
			Refresh();
		}

		private void Refresh() {
			if (_dbObject == null) {
				canvasGroup.alpha = 0;
				return;
			}

			canvasGroup.alpha = 1;

			if (_dbObject is DBCourseInfo courseInfo) {
				RefreshCourse(courseInfo);
				return;
			}
			
			if (_dbObject is DBHoleInfo levelInfo) {
				RefreshLevel(levelInfo);
			}
			
		}

		private void RefreshLevel(DBHoleInfo dbHoleInfo) {
			nameText.text = dbHoleInfo.name;
			addHoleButton.gameObject.SetActive(true);
			addHoleButton.OnClickAction = b => {
				if (subwindow is CourseCreator courseCreator) {
					if (b) {
						courseCreator.holesList.RemoveHoleFromList(dbHoleInfo);
					} else {
						courseCreator.holesList.AddHoleToList(dbHoleInfo);
					}
				}
			};
		}

		private void RefreshCourse(DBCourseInfo dbCourseInfo) {
			addHoleButton.gameObject.SetActive(false);
			nameText.text = dbCourseInfo.name;
		}

		public void OnClick() {
			if (_dbObject == null) {
				return;
			}
			
			if (_dbObject is DBCourseInfo course) {
				MainMenu.MainMenu.courseOverview.Open(course);
				return;
			}
		}

	}
}
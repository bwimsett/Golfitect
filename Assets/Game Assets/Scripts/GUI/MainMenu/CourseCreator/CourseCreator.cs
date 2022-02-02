using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using Game_Assets.Scripts.GUI.CourseCreator;
using GUI.MainMenu;
using Steamworks;
using TMPro;
using UnityEngine;

namespace GUI.MainMenu.CourseCreator {
	public class CourseCreator : MainMenu_Subwindow {

		private SteamItemData[] levelInfo;
		[SerializeField] private TMP_InputField nameInputField, descriptionInputField;
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;
		public CourseCreator_HolesList holesList;

		void Start() {
			RequestLevelsFromServer();
		}

		private void RequestLevelsFromServer() {
			GameSceneManager.serverManager.GetUserLevelIDs(OnLevelIDsLoaded);
		}

		private void OnLevelIDsLoaded(string[] ids) {
			levelOptionGrid.SetIDs(ids, false, this);
		}

		public Course GetCourseFromInput() {
			string title = nameInputField.text;
			string description = descriptionInputField.text;
			DBHoleInfo[] holes = holesList.GetHoles();
			
			Course course = new Course(title, description, holes);

			return course;
		}

		public void Load() {
			
			
			Open();
		}
	}
}

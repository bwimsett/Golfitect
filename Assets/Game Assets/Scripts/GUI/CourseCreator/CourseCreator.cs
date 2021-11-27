using Backend.Course;
using Backend.Level;
using Steamworks;
using TMPro;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.CourseCreator {
	public class CourseCreator : MainMenu_Subwindow {

		private LevelInfo[] levelInfo;
		[SerializeField] private TMP_InputField nameInputField, descriptionInputField;
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;
		public CourseCreator_HolesList holesList;

		void Start() {
			RequestLevelInfoFromSteam();
		}

		private void RequestLevelInfoFromSteam() {
			LevelUploader levelUploader = new LevelUploader();
			levelUploader.GetUserLevelInfos(SteamUser.GetSteamID().GetAccountID(), 1, OnLevelInfoLoaded);
		}

		private void OnLevelInfoLoaded(LevelInfo[] info) {
			levelOptionGrid.SetOptions(info, this);
		}

		public Course GetCourseFromInput() {
			string title = nameInputField.text;
			string description = descriptionInputField.text;
			LevelInfo[] holes = holesList.GetHoles();
			
			Course course = new Course(title, description, holes);

			return course;
		}

	}
}

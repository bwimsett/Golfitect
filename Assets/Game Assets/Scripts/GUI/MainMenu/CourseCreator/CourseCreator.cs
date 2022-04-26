using System;
using System.Collections.ObjectModel;
using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using BWLocaliser;
using Game_Assets.Scripts.Backend.Language.ProfanityFilter;
using Game_Assets.Scripts.Backend.Server;
using Game_Assets.Scripts.GUI.CourseCreator;
using Game_Assets.Scripts.GUI.PlayMode;
using GUI.MainMenu;
using Sirenix.Utilities;
using Steamworks;
using TMPro;
using UnityEngine;

namespace GUI.MainMenu.CourseCreator {
	public class CourseCreator : MainMenu_Subwindow {

		private SteamItemData[] levelInfo;
		[SerializeField] private TMP_InputField nameInputField, descriptionInputField;
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;
		public CourseCreator_HolesList holesList;
		
		public DBCourseInfo currentCourse;

		void Awake() {
			nameInputField.characterLimit = Course.NAME_CHAR_LIMIT;
			descriptionInputField.characterLimit = Course.DESCRIPTION_CHAR_LIMIT;
		}
		
		private void RequestLevelsFromServer() {
			GameSceneManager.serverManager.GetUserLevelIDs(OnLevelIDsLoaded);
		}

		private void OnLevelIDsLoaded(string[] ids) {
			levelOptionGrid.SetIDs(ids, false, this);
		}
		private enum ValidityStatus {
			valid,
			emptyTitle,
			emptyHoles,
			profaneTitle,
			profaneDescription
		};
		
		private bool CheckCourseInputValidity() {
			string title = nameInputField.text;
			string description = descriptionInputField.text;
			DBHoleInfo[] holes = holesList.GetHoles();

			ValidityStatus validityStatus = ValidityStatus.valid;

			// Check whether holes have been selected
			if (holes.Length == 0) { validityStatus = ValidityStatus.emptyHoles; }
			
			ProfanityFilter filter = new ProfanityFilter();
			
			// Check the description
			ReadOnlyCollection<string> profanities = filter.DetectAllProfanities(description);
			if (profanities.Count > 0) { validityStatus = ValidityStatus.profaneDescription; }
			
			// Check the title
			profanities = filter.DetectAllProfanities(title);
			if (profanities.Count > 0) { validityStatus = ValidityStatus.profaneTitle; }
			
			if (title.IsNullOrWhitespace()) { validityStatus = ValidityStatus.emptyTitle; }

			switch (validityStatus) {
				case ValidityStatus.valid: return true;
				case ValidityStatus.emptyTitle: CreateInputInvalidPopup("coursecreator_popup_title_empty");
					break;
				case ValidityStatus.profaneTitle: CreateInputInvalidPopup("coursecreator_popup_title_profanity_detected");
					break;
				case ValidityStatus.profaneDescription: CreateInputInvalidPopup("coursecreator_popup_description_profanity_detected");
					break;
				case ValidityStatus.emptyHoles: CreateInputInvalidPopup("coursecreator_popup_holes_empty");
					break;
			}

			return false;
		}

		private void CreateInputInvalidPopup(string text) {
			PopupAlert popup = GameSceneManager.popupManager.CreatePopup();
			popup.SetValues(new LocString(text));
		}
		
		public Course GetCourseFromInput() {
			string title = nameInputField.text;
			string description = descriptionInputField.text;
			DBHoleInfo[] holes = holesList.GetHoles();

			bool validInput = CheckCourseInputValidity();

			if (!validInput) {
				return null;
			}

			Course course = new Course(title, description, holes);
			
			// If updating an existing course, set the ID so the new data is sent as an update
			if (currentCourse != null) {
				course._id = currentCourse._id;
			}
			
			return course;
		}

		public void Load(DBCourseInfo courseInfo) {
			currentCourse = courseInfo;
			
			nameInputField.text = courseInfo.name;
			descriptionInputField.text = courseInfo.description;
			
			// Populate the holes list
			holesList.Clear();
			GameSceneManager.serverManager.GetHoles(courseInfo.holeIDs, result => {
				for (int i = 0; i < courseInfo.holeIDs.Length; i++) {
					for (int hole = 0; hole < result.Length; hole++) {
						if (result[hole]._id.Equals(courseInfo.holeIDs[i])) {
							holesList.AddHoleToList(result[hole]);
							break;
						}
					}
				}
			});
			
			RequestLevelsFromServer();
			
			Open();
		}

		public void New() {
			currentCourse = null;
			nameInputField.text = "";
			descriptionInputField.text = "";
			holesList.Clear();
			RequestLevelsFromServer();
			Open();
		}
	}
}

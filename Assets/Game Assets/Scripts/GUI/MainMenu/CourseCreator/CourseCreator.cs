using System.Collections.ObjectModel;
using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Language.ProfanityFilter;
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

		public Course GetCourseFromInput() {
			string title = nameInputField.text;
			string description = descriptionInputField.text;
			DBHoleInfo[] holes = holesList.GetHoles();
			
			// Check the name
			ProfanityFilter filter = new ProfanityFilter();
			ReadOnlyCollection<string> profanities = filter.DetectAllProfanities(title);
			Debug.Log(profanities.Count+" profanities detected in title");
			
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

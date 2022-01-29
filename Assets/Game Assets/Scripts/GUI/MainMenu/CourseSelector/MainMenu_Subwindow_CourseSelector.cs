using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using Steamworks;
using UnityEngine;

namespace GUI.MainMenu.CourseSelector {
	public class MainMenu_Subwindow_CourseSelector : MainMenu_Subwindow {
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;
		private bool userOnly; // only displays courses created by the logged in user

		private string[] courseList;

		private void PopulateCourseOptions() {
			if (userOnly) {
				GameSceneManager.serverManager.GetUserCourseInfo(OnCoursesLoaded);
				return;
			}
			
			GameSceneManager.serverManager.GetNewestCourses(OnCourseListRetrieved);
		}
		
		private void OnCoursesLoaded(DBCourseInfo[] courses) {
			levelOptionGrid.SetOptions(courses, this);
		}

		private void OnCourseListRetrieved(string[] courseList) {
			this.courseList = courseList;
			levelOptionGrid.SetCourseIDs(courseList);
		}

		public void Refresh(bool userOnly) {
			this.userOnly = userOnly;
			PopulateCourseOptions();
		}

	}
}
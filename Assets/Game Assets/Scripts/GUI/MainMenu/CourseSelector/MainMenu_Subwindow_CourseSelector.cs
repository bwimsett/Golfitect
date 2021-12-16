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

		private void PopulateCourseOptions() {
			GameSceneManager.serverManager.GetUserCourseInfo(OnCoursesLoaded);
		}
		
		private void OnCoursesLoaded(DBCourseInfo[] courses) {
			levelOptionGrid.SetOptions(courses, this);
		}

		public void Refresh() {
			PopulateCourseOptions();
		}

	}
}
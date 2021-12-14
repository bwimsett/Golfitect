using Backend.Enums;
using Backend.Level;
using Steamworks;
using UnityEngine;

namespace GUI.MainMenu.CourseSelector {
	public class MainMenu_Subwindow_CourseSelector : MainMenu_Subwindow {
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;

		private void PopulateCourseOptions() {
			LevelLoader levelLoader = new LevelLoader();
			levelLoader.GetUserLevelInfos(SteamUser.GetSteamID().GetAccountID(), 1, OnCoursesLoaded, LevelType.Course);
		}
		
		private void OnCoursesLoaded(SteamItemData[] levelInfos) {
			levelOptionGrid.SetOptions(levelInfos, this);
		}

		public void Refresh() {
			PopulateCourseOptions();
		}

	}
}
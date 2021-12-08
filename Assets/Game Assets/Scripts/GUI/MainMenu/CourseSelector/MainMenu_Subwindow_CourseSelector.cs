using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Game_Assets.Scripts.GUI;
using Game_Assets.Scripts.GUI.LevelOptionGrid;
using Steamworks;
using UnityEngine;

namespace DefaultNamespace {
	public class MainMenu_Subwindow_CourseSelector : MainMenu_Subwindow {
		[SerializeField] private LevelOptionGrid levelOptionGrid;

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
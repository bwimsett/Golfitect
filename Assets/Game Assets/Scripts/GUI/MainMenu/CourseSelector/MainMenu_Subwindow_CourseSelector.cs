using System.Collections.Generic;
using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using Steamworks;
using TMPro;
using UnityEngine;

namespace GUI.MainMenu.CourseSelector {
	public class MainMenu_Subwindow_CourseSelector : MainMenu_Subwindow {
		[SerializeField] private LevelOptionGrid.LevelOptionGrid levelOptionGrid;
		private bool userOnly; // only displays courses created by the logged in user

		private string[] sortingOptions = { "Most Liked", "Most Played", "Newest" };
		private string[] sortingDurationOptions = { "Weekly", "Monthly", "Yearly", "All Time" };
		[SerializeField] private TMP_Dropdown sortingDropdown, durationDropdown;

		private string[] courseList;

		void Awake() {
			List<string> sortingOptionsList = new List<string>();
			List<string> sortingDurationOptionsList = new List<string>();
			sortingOptionsList.AddRange(sortingOptions);
			sortingDurationOptionsList.AddRange(sortingDurationOptions);
			
			sortingDropdown.ClearOptions();
			durationDropdown.ClearOptions();
			
			sortingDropdown.AddOptions(sortingOptionsList);
			durationDropdown.AddOptions(sortingDurationOptionsList);
			
			sortingDropdown.SetValueWithoutNotify(0);
			durationDropdown.SetValueWithoutNotify(0);
		}
		
		private void PopulateCourseOptions() {
			if (userOnly) {
				GameSceneManager.serverManager.GetUserCourseIDs(OnCourseListRetrieved);
				return;
			}

			switch (sortingDropdown.value) {
				case 0: GameSceneManager.serverManager.GetMostLikedCourses(GetDropdownDuration(), OnCourseListRetrieved); break;
				case 1: GameSceneManager.serverManager.GetMostPlayedCourses(GetDropdownDuration(), OnCourseListRetrieved); break;
				case 2: GameSceneManager.serverManager.GetNewestCourses(OnCourseListRetrieved); break;
			}
			
		}

		private void OnCourseListRetrieved(string[] courseList) {
			this.courseList = courseList;
			levelOptionGrid.SetIDs(courseList, true, this);
		}

		public void Refresh(bool userOnly) {
			this.userOnly = userOnly;
			OnDropdownModified();
		}

		public void Refresh() {
			Refresh(userOnly);
		}

		private int GetDropdownDuration() {
			switch (durationDropdown.value) {
				case 0: return 7;
				case 1: return 28;
				case 2: return 365;
				case 3: return -1;
			}

			return 7;
		}

		public void OnDropdownModified() {
			durationDropdown.gameObject.SetActive(sortingDropdown.value == 0 || sortingDropdown.value == 1);
			PopulateCourseOptions();
		}
	}
}
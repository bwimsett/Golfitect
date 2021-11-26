using Backend.Level;
using Steamworks;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.CourseCreator {
	public class CourseCreator : MainMenu_Subwindow {

		private LevelInfo[] levelInfo;
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

	}
}

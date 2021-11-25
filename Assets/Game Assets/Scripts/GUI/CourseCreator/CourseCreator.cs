using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Steamworks;
using UnityEngine;

public class CourseCreator : MonoBehaviour {

	private LevelInfo[] levelInfo;
	[SerializeField] private LevelOptionGrid levelOptionGrid;

	void Start() {
		RequestLevelInfoFromSteam();
	}

	private void RequestLevelInfoFromSteam() {
		LevelUploader levelUploader = new LevelUploader();
		levelUploader.GetUserLevelInfos(SteamUser.GetSteamID().GetAccountID(), 1, SetLevelInfo);
	}

	private void SetLevelInfo(LevelInfo[] info) {
		levelOptionGrid.SetOptions(info);
	}

}

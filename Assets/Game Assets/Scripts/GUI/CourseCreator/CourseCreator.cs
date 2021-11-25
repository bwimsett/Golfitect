using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Steamworks;
using UnityEngine;

public class CourseCreator : MonoBehaviour {

	void Start() {
		LevelUploader levelUploader = new LevelUploader();
		levelUploader.GetUserLevels(SteamUser.GetSteamID().GetAccountID(), 1, Test);
	}

	private void Test(PublishedFileId_t[] fileId) {
		
	}

}

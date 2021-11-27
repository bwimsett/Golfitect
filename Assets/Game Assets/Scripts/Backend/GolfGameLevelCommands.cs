using Backend.Level;
using Backend.Submittable;
using Steamworks;
using UnityEngine;

namespace Backend {
	public class GolfGameConsoleCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savelevel", new BWConsole_Action((string levelName) => {
				SteamUploader steamUploader = new SteamUploader();
				SteamSubmittable submittable = new SteamSubmittable(levelName, "", LevelManager.currentLevel);
				steamUploader.UploadToSteam(submittable);
			}));
		}
		
		

	}
}
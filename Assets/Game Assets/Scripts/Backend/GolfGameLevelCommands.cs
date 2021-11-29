using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using UnityEngine;

namespace Backend {
	public class GolfGameLevelCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savelevel", new BWConsole_Action(levelName => {
				SteamLoader steamLoader = new SteamLoader();
				SteamSubmittable submittable = new SteamSubmittable(levelName, "", GameManager.currentLevel);
				steamLoader.UploadToSteam(submittable);
			}));
		}
		
		

	}
}
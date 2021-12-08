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
			
			console.config.AddAction("play", new BWConsole_Action(() => {
				GameManager.currentLevel.Play();
			}));
			
			console.config.AddAction("setpar", new BWConsole_Action(par => { GameManager.currentLevel.par = par; }));
		}
		
		

	}
}
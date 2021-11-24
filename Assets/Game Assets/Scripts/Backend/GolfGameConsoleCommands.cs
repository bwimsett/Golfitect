using Backend.Level;
using UnityEngine;

namespace Backend {
	public class GolfGameConsoleCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savelevel", new BWConsole_Action(() => {
				LevelUploader levelUploader = new LevelUploader();
				levelUploader.UploadLevelToSteam(LevelManager.currentLevel);
			}));
		}
		
		

	}
}
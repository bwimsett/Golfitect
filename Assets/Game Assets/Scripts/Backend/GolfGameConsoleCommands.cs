using Backend.Level;
using UnityEngine;

namespace Backend {
	public class GolfGameConsoleCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savelevel", new BWConsole_Action((string levelName) => {
				LevelUploader levelUploader = new LevelUploader();
				LevelManager.currentLevel.levelName = levelName;
				levelUploader.UploadLevelToSteam(LevelManager.currentLevel);
			}));
		}
		
		

	}
}
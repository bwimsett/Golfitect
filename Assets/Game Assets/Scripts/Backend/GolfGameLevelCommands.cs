using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using UnityEngine;

namespace Backend {
	public class GolfGameLevelCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savelevel", new BWConsole_Action(() => {
				ServerSubmittable submittable = new ServerSubmittable(GameManager.currentLevel);
				LevelManager.serverManager.SubmitLevel(submittable, result=>{console.Print(result);});
			}));
			
			console.config.AddAction("play", new BWConsole_Action(() => {
				GameManager.currentLevel.Play();
			}));
			
			console.config.AddAction("setpar", new BWConsole_Action(par => { GameManager.currentLevel.par = par; }));
			
			console.config.AddAction("closelevsum", new BWConsole_Action(()=> {
				LevelManager.playModeHUD.levelSummary.Close();
			}));
			console.config.AddAction("openlevsum", new BWConsole_Action(LevelManager.playModeHUD.levelSummary.Open));
		}
		
		

	}
}
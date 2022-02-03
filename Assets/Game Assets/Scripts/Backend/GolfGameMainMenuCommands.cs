using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.GUI.MainMenu;
using GUI.MainMenu;
using UnityEngine;

namespace Backend {
	public class GolfGameMainMenuCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savecourse", new BWConsole_Action(() => {
				GameSceneManager.serverManager.SubmitCourse(MainMenu.courseCreator.GetCourseFromInput(), res => {
					console.Print(res);
				}, false);
			}));
			
			console.config.AddAction("editcourse", new BWConsole_Action(() => {
				GameSceneManager.serverManager.SubmitCourse(MainMenu.courseCreator.GetCourseFromInput(), res => {
					console.Print(res);
				}, true);
			}));
		}
		
	}
}
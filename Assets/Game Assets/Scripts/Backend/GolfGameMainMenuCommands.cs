using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using GUI.MainMenu;
using UnityEngine;

namespace Backend {
	public class GolfGameMainMenuCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savecourse", new BWConsole_Action(() => {
				/*ServerLoader serverLoader = new ServerLoader();
				Course.Course course = MainMenu.courseCreator.GetCourseFromInput();
				serverLoader.UploadToSteam(new ServerSubmittable(course.title, course.description, course));
				LoadingScreenManager.Load(course.steamHoleData[0], GameMode.Play);*/
			}));
		}
		
	}
}
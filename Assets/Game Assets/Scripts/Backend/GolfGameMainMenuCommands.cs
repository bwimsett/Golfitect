using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using UnityEngine;

namespace Backend {
	public class GolfGameMainMenuCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savecourse", new BWConsole_Action(() => {
				SteamUploader steamUploader = new SteamUploader();
				Course.Course course = MainMenuManager.courseCreator.GetCourseFromInput();
				steamUploader.UploadToSteam(new SteamSubmittable(course.title, course.description, course));
			}));
		}
		
	}
}
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using UnityEngine;

namespace Backend {
	public class GolfGameMainMenuCommands : MonoBehaviour {

		public BWConsole console;
		
		public void Initialise() {
			console.config.AddAction("savecourse", new BWConsole_Action(() => {
				SteamLoader steamLoader = new SteamLoader();
				Course.Course course = MainMenu.courseCreator.GetCourseFromInput();
				steamLoader.UploadToSteam(new SteamSubmittable(course.title, course.description, course));
				LoadingScreenManager.Load(course.holesInfo[0], GameMode.Play);
			}));
		}
		
	}
}
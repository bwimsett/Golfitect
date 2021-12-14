using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {

	private static SteamItemData loadTarget;

	/// <summary>
	/// Sets the load target, and opens the loading screen
	/// </summary>
	public static void Load(SteamItemData steamItemData, GameMode gameMode) {
		GameManager.gameMode = gameMode;
		loadTarget = steamItemData;
		SceneManager.LoadScene("Loading");
	}
	
	void Start() {
		InitiateLoad();
	}
	
	private void InitiateLoad() {
		if (loadTarget is SteamCourseData courseInfo) {
			LoadCourse(courseInfo);
			return;
		} else if (loadTarget != null) {
			LoadHole();
			return;
		}
		
		IEnumerator load = LoadScene("Game");
		StartCoroutine(load);
	}

	private void LoadCourse(SteamCourseData steamCourseData) {
		/*// First download the course
		ServerLoader serverLoader = new ServerLoader();
		serverLoader.GetFileFromID(steamCourseData.id, levelString => {
			// Back out of loading screen if no valid file returned
			if (levelString.Equals(string.Empty)) {
				Debug.LogError("Couldn't download course: "+steamCourseData.id);
				SceneManager.LoadScene("Main Menu");
				return;
			}

			// Deserialize the course
			Course course = (Course)JsonConvert.DeserializeObject(levelString, typeof(Course));

			if (course == null) {
				Debug.LogError("Deserialized course is null: "+steamCourseData.id);
				SceneManager.LoadScene("Main Menu");
				return;
			}

			// Download the levels and set the first hole as the current hole
			course.DownloadLevels(() => {
				if (course.holes.Length == 0) {
					Debug.LogError("Downloaded course has no holes: "+steamCourseData.id);
					SceneManager.LoadScene("Main Menu");
					return;
				}
				
				// Load the first hole
				GameManager.StartCourse(course);
				IEnumerator load = LoadScene("Game");
				StartCoroutine(load);
			});
		});*/
	}

	private void LoadHole() {
		/*// First download the level data
		// Then pass the data to the game scene
		// Then load the scene async
		ServerLoader serverLoader = new ServerLoader();
		serverLoader.GetFileFromID(loadTarget.id, levelString => {
			// Back out of loading screen if no valid file returned
			if (levelString.Equals(string.Empty)) {
				SceneManager.LoadScene("Main Menu");
			}

			Level level = (Level)JsonConvert.DeserializeObject(levelString, typeof(Level));
			GameManager.SetCurrentLevel(level);
			IEnumerator load = LoadScene("Game");
			StartCoroutine(load);
		});*/
	}

	private IEnumerator LoadScene(string sceneName)
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
		// a sceneBuildIndex of 1 as shown in Build Settings.

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

}

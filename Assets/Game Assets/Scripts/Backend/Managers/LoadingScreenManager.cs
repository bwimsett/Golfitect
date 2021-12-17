using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Backend.Course;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game_Assets.Scripts.Backend.Server;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {

	private static DBObject loadTarget;

	/// <summary>
	/// Sets the load target, and opens the loading screen
	/// </summary>
	public static void Load(DBObject loadTarget, GameMode gameMode) {
		GameManager.gameMode = gameMode;
		LoadingScreenManager.loadTarget = loadTarget;
		SceneManager.LoadScene("Loading");
	}
	
	void Start() {
		InitiateLoad();
	}
	
	private void InitiateLoad() {
		if (loadTarget is DBCourseInfo courseInfo) {
			LoadCourse(courseInfo);
			return;
		} else if (loadTarget != null) {
			LoadHole();
			return;
		}
		
		IEnumerator load = LoadScene("Game");
		StartCoroutine(load);
	}

	private void LoadCourse(DBCourseInfo courseInfo) {
		GameSceneManager.serverManager.GetCourse(courseInfo._id, course => {
			course.DownloadLevels(() => {
				GameManager.StartCourse(course, () => {
					StartCoroutine(LoadScene("Game"));
				});
			});
		});
	}

	private void LoadHole() {
		/* First download the level data
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

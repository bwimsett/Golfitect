using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Backend.Level;
using Backend.Managers;
using Backend.Submittable;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour {

	private static LevelInfo loadTarget;

	/// <summary>
	/// Sets the load target, and opens the loading screen
	/// </summary>
	public static void LoadLevel(LevelInfo levelInfo) {
		loadTarget = levelInfo;
		SceneManager.LoadScene("Loading");
	}
	
	void Start() {
		InitiateLoad();
	}

	private void InitiateLoad() {
		// First download the level data
		// Then pass the data to the game scene
		// Then load the scene async
		SteamLoader steamLoader = new SteamLoader();
		steamLoader.GetFileFromID(loadTarget.id, levelString => {
			// Back out of loading screen if no valid file returned
			if (levelString.Equals(string.Empty)) {
				SceneManager.LoadScene("Main Menu");
			}

			Level level = (Level)JsonConvert.DeserializeObject(levelString, typeof(Level));
			GameManager.SetCurrentLevel(level);
			IEnumerator load = LoadScene("Game");
			StartCoroutine(load);
		});
		
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

using System;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game;
using Game_Assets.Scripts.GUI;
using Game_Assets.Scripts.GUI.LevelBuilder;
using Game_Assets.Scripts.GUI.PlayMode;
using Steamworks;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	
	[SerializeField] private CameraController _cameraController;
	public static CameraController cameraController;
	[SerializeField] private LevelGrid _levelGrid;
	public static LevelGrid levelGrid;
	[SerializeField] private LevelInputManager _levelInputManager;
	public static LevelInputManager levelInputManager;
	[SerializeField] private LevelObjectUtility _levelObjectUtility;
	public static LevelObjectUtility levelObjectUtility;
	[SerializeField] private BuildModeHUD _buildModeHUD;
	public static BuildModeHUD buildModeHUD;
	[SerializeField] private PlayModeHUD _playModeHUD;
	public static PlayModeHUD playModeHUD; 
	[SerializeField] private MeshCollider _levelCollider;
	public static MeshCollider levelCollider;
	[SerializeField] private LevelTimer _levelTimer;
	public static LevelTimer levelTimer;
	[SerializeField] private ServerManager _serverManager;
	public static ServerManager serverManager;

	public static GameHUD currentGameHUD;
	private static bool inGameScene;
	
	void Awake() {
		inGameScene = true;
		PopulateGlobalVariables();
		Initialise();
	}

	void OnDestroy() {
		inGameScene = false;
	}

	private void PopulateGlobalVariables() {
		cameraController = _cameraController;
		levelGrid = _levelGrid;
		levelInputManager = _levelInputManager;
		levelObjectUtility = _levelObjectUtility;
		levelCollider = _levelCollider;
		buildModeHUD = _buildModeHUD;
		playModeHUD = _playModeHUD;
		levelTimer = _levelTimer;
		serverManager = _serverManager;
	}

	private void Initialise() {
		// Create a new level if the current level is set to null
		if (GameManager.currentLevel == null) {
			GameManager.SetCurrentLevel(new Level(new Vector3Int(50, 10, 50)));
		}

		cameraController.Initialise();
		levelGrid.Initialise();

		GameManager.currentLevel.Load();
	}

	public static void InitialiseGameMode() {
		if (!inGameScene) {
			return;
		}
		
		buildModeHUD.gameObject.SetActive(true);
		playModeHUD.gameObject.SetActive(true);
		
		buildModeHUD.Close(false);
		playModeHUD.Close(false);

		switch (GameManager.gameMode) {
			case GameMode.Build: currentGameHUD = buildModeHUD; break;
			case GameMode.Play:
				levelTimer.StartTimer();
				currentGameHUD = playModeHUD; break;
		}
		GameManager.currentLevel.Play();
		currentGameHUD.gameObject.SetActive(true);
		currentGameHUD.Open();
	}

}

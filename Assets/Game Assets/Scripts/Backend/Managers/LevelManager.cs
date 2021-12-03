using System;
using Backend.Level;
using Backend.Managers;
using Game;
using Game_Assets.Scripts.GUI.LevelBuilder;
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
	[SerializeField] private LevelBuilderHUDManager levelBuilderHUD;
	[SerializeField] private MeshCollider _levelCollider;
	public static MeshCollider levelCollider;

	private static bool inGameScene;
	private static LevelMode mode;
	public enum LevelMode {
		Build, Play
	}
	
	void Awake() {
		inGameScene = true;
		PopulateGlobalVariables();
		Initialise();
		SetMode(mode);
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
	}

	private void Initialise() {
		// Create a new level if the current level is set to null
		if (GameManager.currentLevel == null) {
			GameManager.SetCurrentLevel(new Level(new Vector3Int(50, 10, 50)));
		}

		cameraController.Initialise();
		levelGrid.Initialise();
		levelBuilderHUD.Initialise();

		GameManager.currentLevel.Load();
	}

	public static void SetMode(LevelMode mode) {
		LevelManager.mode = mode;

		if (!inGameScene) {
			return;
		}

		switch (mode) {
			case LevelMode.Build: break;
			case LevelMode.Play: EnterPlayMode(); break;
		}
	}

	private static void EnterPlayMode() {
		
	}

}

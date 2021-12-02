using Backend.Level;
using Backend.Managers;
using Game;
using Game_Assets.Scripts.GUI.LevelBuilder;
using Steamworks;
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

	void Awake() {
		PopulateGlobalVariables();
		Initialise();
	}
	
	private void PopulateGlobalVariables() {
		cameraController = _cameraController;
		levelGrid = _levelGrid;
		levelInputManager = _levelInputManager;
		levelObjectUtility = _levelObjectUtility;
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

}

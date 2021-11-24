using Backend.Level;
using Backend.Managers;
using Game;
using Steamworks;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static Level currentLevel { get; private set; }

	[SerializeField] private CameraController _cameraController;
	public static CameraController cameraController;
	[SerializeField] private LevelGrid _levelGrid;
	public static LevelGrid levelGrid;
	[SerializeField] private LevelInputManager _levelInputManager;
	public static LevelInputManager levelInputManager;

	void Awake() {
		PopulateGlobalVariables();
		Initialise();
	}


	private void PopulateGlobalVariables() {
		cameraController = _cameraController;
		levelGrid = _levelGrid;
		levelInputManager = _levelInputManager;
	}

	private void Initialise() {
		currentLevel = new Level(new Vector3Int(50, 10, 50));
		
		cameraController.Initialise();
		levelGrid.Initialise();
		levelInputManager.Initialise();
	}

}

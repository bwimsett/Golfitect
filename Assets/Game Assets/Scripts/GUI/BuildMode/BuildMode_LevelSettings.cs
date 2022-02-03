using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Backend.Managers;
using TMPro;
using UnityEngine;

public class BuildMode_LevelSettings : MonoBehaviour {

	public TMP_InputField levelNameField;
	public Spinner parSpinner;
	
	public void Refresh() {
		levelNameField.text = GameManager.currentLevel.name;
		parSpinner.SetMinMax(1, Level.maxPar);
	}

	public void SaveLevel() {
		SetLevelVariables();
		LevelManager.serverManager.SubmitLevel(GameManager.currentLevel, result=>{Debug.Log(result);}, false);
	}

	private void SetLevelVariables() {
		Level level = GameManager.currentLevel;
		level.par = parSpinner.value;
		level.name = levelNameField.text;
	}
	
}

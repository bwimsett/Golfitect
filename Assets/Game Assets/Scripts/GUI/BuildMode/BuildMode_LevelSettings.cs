using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Backend.Managers;
using TMPro;
using UnityEngine;

public class BuildMode_LevelSettings : MonoBehaviour {

	public TMP_InputField levelNameField;
	public Spinner parSpinner;
	public StripeButton saveButton;

	public void Refresh() {
		levelNameField.text = GameManager.currentLevel.name;
		parSpinner.SetMinMax(1, Level.maxPar);
		saveButton.SetInteractable(GameManager.currentLevel.completable);
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

	public void RefreshSaveButton() {
		saveButton.SetInteractable(GameManager.currentLevel.completable);
	}
	
}

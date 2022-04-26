using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Backend.Level;
using Backend.Managers;
using BWLocaliser;
using Game_Assets.Scripts.Backend.Language.ProfanityFilter;
using Game_Assets.Scripts.GUI.PlayMode;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class BuildMode_LevelSettings : MonoBehaviour {

	public TMP_InputField levelNameField;
	public Spinner parSpinner;
	public StripeButton saveButton;

	void Awake() {
		levelNameField.characterLimit = Level.NAME_CHAR_LIMIT;
	}
	
	public void Refresh() {
		levelNameField.text = GameManager.currentLevel.name;
		parSpinner.SetMinMax(1, Level.maxPar);
		saveButton.SetInteractable(GameManager.currentLevel.completable);
	}

	public void SaveLevel() {
		SetLevelVariables();
		bool validity = CheckHoleValidity(GameManager.currentLevel);
		if (!validity) {
			return;
		}
		LevelManager.serverManager.SubmitLevel(GameManager.currentLevel, result=>{Debug.Log(result);}, false);
	}

	private void SetLevelVariables() {
		Level level = GameManager.currentLevel;
		level.par = parSpinner.value;
		level.name = levelNameField.text;
	}

	private enum HoleValidity{Valid, EmptyName, ProfaneName}
	
	private bool CheckHoleValidity(Level level) {
		HoleValidity validity = HoleValidity.Valid;
		
		if (level.name.IsNullOrWhitespace()) {
			validity = HoleValidity.EmptyName;
		}
		
		ProfanityFilter filter = new ProfanityFilter();
		ReadOnlyCollection<string> profanities = filter.DetectAllProfanities(level.name);

		if (profanities.Count > 0) {
			validity = HoleValidity.ProfaneName;
		}

		switch (validity) {
			case HoleValidity.Valid: return true;
			case HoleValidity.EmptyName: CreateValidityPopup("levelbuilder_popup_title_empty");
				break;
			case HoleValidity.ProfaneName: CreateValidityPopup("levelbuilder_popup_title_profanity_detected");
				break;
		}

		return false;
	}

	private void CreateValidityPopup(string text) {
		PopupAlert popup = GameSceneManager.popupManager.CreatePopup();
		popup.SetValues(new LocString(text));
	}

	public void RefreshSaveButton() {
		saveButton.SetInteractable(GameManager.currentLevel.completable);
	}
	
}

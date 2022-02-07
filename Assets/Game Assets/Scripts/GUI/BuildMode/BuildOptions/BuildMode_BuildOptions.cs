using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Backend;
using Backend.Level;
using Backend.Managers;
using BWLocaliser;
using DG.Tweening;
using Game_Assets.Scripts.GUI.LevelBuilder.BuildOptions;
using UnityEngine;

public class BuildMode_BuildOptions : MonoBehaviour {
	[SerializeField] private GameObject buildOptionPrefab;
	private List<BuildMode_BuildOptions_Option> options;
	[SerializeField] private Transform optionContainer;

	private List<BuildMode_BuildOptions_Tag> tagOptions;
	[SerializeField] private GameObject tagOptionPrefab;
	[SerializeField] private Transform tagOptionsContainer;
	[SerializeField] private TextLocalizer filterCountText;
	
	private List<LevelObjectTag> selectedTags;
	private List<LevelObject> visibleObjects;
	private List<LevelObjectTag> tags;

	private RectTransform rectTransform;
	[SerializeField] private float openDuration, closeDuration, xPadding;
	[SerializeField] private Ease openEase, closeEase;
	private Tween moveTween;
	private bool open;

	void Start() {
		InitialiseTagsList();
		rectTransform = GetComponent<RectTransform>();
	}

	private void InitialiseTagsList() {
		tags = new List<LevelObjectTag>();

		if (tagOptions == null) {
			tagOptions = new List<BuildMode_BuildOptions_Tag>();
		}
		
		tagOptionPrefab.gameObject.SetActive(false);
		
		foreach (LevelObject obj in LevelManager.levelObjectUtility.objectBank.levelObjects) {
			foreach (LevelObjectTag tag in obj.tags) {
				if (!tags.Contains(tag)) {
					tags.Add(tag);
					BuildMode_BuildOptions_Tag tagOption = Instantiate(tagOptionPrefab, tagOptionsContainer).GetComponent<BuildMode_BuildOptions_Tag>();
					tagOption.SetTag(tag, this);
					tagOptions.Add(tagOption);
					tagOption.gameObject.SetActive(true);
				}
			}
		}
		
		RefreshSelectedTags();
	}

	private void ApplyTags() {
		visibleObjects = new List<LevelObject>();

		// If no tags selected, add all
		if (selectedTags.Count == 0) {
			visibleObjects.AddRange(LevelManager.levelObjectUtility.objectBank.levelObjects);
			return;
		}

		// Add objects to the visible list if they have all of the requested tags
		foreach (LevelObject obj in LevelManager.levelObjectUtility.objectBank.levelObjects) {
			bool objectHasTag = false;
			foreach (LevelObjectTag tag in selectedTags) {
				objectHasTag = false;
				foreach (LevelObjectTag objTag in obj.tags) {
					if (tag == objTag) {
						objectHasTag = true;
						break;
					}
				}

				if (!objectHasTag) {
					break;
				}
			}

			if (objectHasTag) {
				visibleObjects.Add(obj);
			}
		}
	}

	public void RefreshSelectedTags() {
		selectedTags = new List<LevelObjectTag>();
		
		foreach (BuildMode_BuildOptions_Tag tag in tagOptions) {
			if (tag.toggle.isOn) {
				selectedTags.Add(tag.tag);	
			}
		}
		
		filterCountText.SetString(new LocString("levelbuilder_buildoptions_filtercount", new Dictionary<string, object>(){{"filters", selectedTags.Count}}));

		ApplyTags();
		Refresh();
	}

	public void Refresh() {
		if (options == null) {
			options = new List<BuildMode_BuildOptions_Option>();
		}
		
		buildOptionPrefab.gameObject.SetActive(false);

		// Instantiate new options if needed
		int optionsRequired = visibleObjects.Count - options.Count;
		if (optionsRequired > 0) {
			for (int i = 0; i < optionsRequired; i++) {
				BuildMode_BuildOptions_Option option = Instantiate(buildOptionPrefab.gameObject, optionContainer).GetComponent<BuildMode_BuildOptions_Option>();
				options.Add(option);
			}
		}
		
		// Now assign LevelObjects to the options
		for (int i = 0; i < options.Count; i++) {
			// Disable any surplus
			if (i >= visibleObjects.Count) {
				options[i].gameObject.SetActive(false);
				continue;
			}
			
			options[i].gameObject.SetActive(true);
			options[i].SetLevelObject(visibleObjects[i]);
		}
	}

	public void ToggleFilterDrawer() {
		tagOptionsContainer.gameObject.SetActive(!tagOptionsContainer.gameObject.activeSelf);	
	}

	public void ToggleOpen() {
		SetOpen(!open);
	}

	public void SetOpen(bool open) {
		if (this.open == open) {
			return;
		}
		
		this.open = open;

		float tweenProgress = 0;
		float duration = 0;
		
		if (moveTween != null) {
			tweenProgress = moveTween.ElapsedPercentage();
			moveTween.Kill();
		}
		
		Vector2 pos = rectTransform.transform.position;
		
		if (open) {
			duration = openDuration * (1-tweenProgress);
			rectTransform.SetPivot(new Vector2(1, 0.5f));
			moveTween = rectTransform.DOAnchorPos(new Vector2(-xPadding, 0), duration).SetEase(openEase);
		}
		else {
			duration = closeDuration * (1-tweenProgress);
			rectTransform.SetPivot(new Vector2(0, 0.5f));
			moveTween = rectTransform.DOAnchorPos(new Vector2(xPadding, 0), duration).SetEase(closeEase);
		}
	}
}

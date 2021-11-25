using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Game_Assets.Scripts.GUI.LevelOptionGrid;
using UnityEngine;

public class LevelOptionGrid : MonoBehaviour {

	[SerializeField] private GameObject optionPrefab;
	[SerializeField] private Vector2Int layout;
	[SerializeField] private float padding;
	[SerializeField] private RectTransform _rectTransform;
	
	private int page;
	private object[,] pages; // x = page, y = option
	private LevelOptionGrid_Option[] options;

	void Awake() {
		SetOptions(new object[5]);
	}
	
	public void SetOptions(object[] optionObjects) {
		// Populate pages array
		int numPerPage = layout.x * layout.y;
		int pageCount = Mathf.CeilToInt(((float) optionObjects.Length / numPerPage));
		pages = new object[pageCount, numPerPage];
		int levelIndex = 0;

		for (int page = 0; page < pageCount; page++) {
			for (int slot = 0; slot < numPerPage; slot++) {
				if (levelIndex >= optionObjects.Length) {
					break;
				}
				
				pages[page, slot] = optionObjects[levelIndex];
				levelIndex++;
			}
		}
		
		// Set page to 0
		page = 0;
		
		Refresh();
	}

	public void Refresh() {
		// Generate optionObjects if the array doesn't exist yet
		if (options == null) {
			// First calculate dimensions per option
			Vector2 gridArea = _rectTransform.rect.size;
			Vector2 usableArea = gridArea - padding * new Vector2(layout.x - 1, layout.y - 1);
			Vector2 optionSize = usableArea / layout;
			
			options = new LevelOptionGrid_Option[layout.x * layout.y];
			int optionIndex = 0;
			
			for (int y = 0; y < layout.y; y++) {
				for (int x = 0; x < layout.x; x++) {
					options[optionIndex] = Instantiate(optionPrefab, _rectTransform).GetComponent<LevelOptionGrid_Option>();
					// Set position and size
					options[optionIndex].rectTransform.sizeDelta = optionSize;
					options[optionIndex].rectTransform.anchoredPosition = GetOptionPosition(new Vector2Int(x, y), optionSize);
					optionIndex++;
				}
			}
		}
		
		// Set values for the optionObjects
		for (int i = 0; i < options.Length; i++) {
			options[i].SetObject(pages[page, i]);
		}
	}

	private Vector2 GetOptionPosition(Vector2Int index, Vector2 optionDimensions) {
		float xPos = index.x * optionDimensions.x + index.x * padding;
		float yPos = index.y * optionDimensions.y + index.y * padding;
		return new Vector2(xPos, -yPos);
	}

}

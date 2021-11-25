

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GridBackgroundGenerator : MonoBehaviour {

	public Sprite sprite;
	public Color color;
	public bool scroll, alternatePosition, alternateRotation;
	public float scrollTime;
	public Vector2 gridSpacing;
	public float rotation;
	public RectTransform container;

	private Vector2 startScrollPos, endScrollPos;
	
	void Awake() {
		Initialise();
	}

	public void Initialise() {
		startScrollPos = new Vector2(-gridSpacing.x*3, 0);
		endScrollPos = new Vector2(startScrollPos.x+2*gridSpacing.x, startScrollPos.y-2*gridSpacing.y);
		container.anchoredPosition = startScrollPos;
		
		GenerateGrid();
		Scroll();
		
	}

	private void GenerateGrid() {
		// First get screen dimensions
		Vector2 dimensions = new Vector2(Screen.width, Screen.height);
		Vector2Int gridDimensions = new Vector2Int(Mathf.CeilToInt(dimensions.x / gridSpacing.x)+3, Mathf.CeilToInt(dimensions.y / gridSpacing.y)+3);

		for (int y = 0; y < gridDimensions.y; y++) {
			for (int x = 0; x < gridDimensions.x; x++) {
				Image image = new GameObject().AddComponent<Image>();
				image.gameObject.transform.SetParent(container);
				
				// Set rotation
				image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotation));
				if (alternateRotation && (x+y)%2 == 0) {
					image.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,-rotation));
				}
				
				// Set position
				image.transform.localPosition = new Vector3(x * gridSpacing.x, y * gridSpacing.y);
				if (alternatePosition && y % 2 == 0) {
					image.transform.localPosition = new Vector3(x+(1/2f), y)*gridSpacing;
				}
				
				image.sprite = sprite;
				image.color = color;
			}
		}
	}

	private void Scroll() {
		if (!scroll) {
			return;
		}
		
		container.DOAnchorPos(endScrollPos, scrollTime).SetEase(Ease.Linear).OnComplete(() => {
			container.anchoredPosition = startScrollPos;
			Scroll();
		});
	}

}

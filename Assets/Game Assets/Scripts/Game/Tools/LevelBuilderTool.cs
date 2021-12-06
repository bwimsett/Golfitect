using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelBuilderTool : Tool {

	private LevelObject currentLevelObject;
	private List<GameObject> objectCache;

	private LevelObject placing;
	private Vector3 startPosition, endPosition, origin, limit;
	private bool mouseDown;

	protected override void OnMouseHover() {
		startPosition = endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnMouseDown() {
		startPosition = endPosition = mousePosition;
		mouseDown = true;
		RefreshScaleAndPosition();
	}

	protected override void MouseDown() {
		endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnMouseUp() {
		Place();
		startPosition = endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnRightMouseDown() {
		SetActive(false);
	}

	protected override void Deactivate() {
		LevelManager.levelBuilderHUD.DeselectBuildOptionFromDock(currentLevelObject);
		currentLevelObject = null;
		Destroy(placing.gameObject);
	}

	private void Place() {
		//foreach (Tuple<Vector3Int, GameObject> placingObject in placing) {
			//LevelObject levelObject = placingObject.Item2.GetComponent<LevelObject>();
			//GameManager.currentLevel.Place(placingObject.Item1, levelObject);			
			//objectCache.Remove(placingObject.Item2);
		//}
		
		GameManager.currentLevel.Place(origin, placing);
		placing = null;
	}

	private void RefreshScaleAndPosition() {
		if (!placing) {
			placing = Instantiate(currentLevelObject.gameObject, LevelManager.levelObjectUtility.levelContainer).GetComponent<LevelObject>();
		}
		
		Vector3 gridStartPos = LevelManager.levelGrid.WorldPositionToGridPosition(startPosition);
		Vector3 gridEndPos = LevelManager.levelGrid.WorldPositionToGridPosition(endPosition);

		// Only scale tile object types
		if (placing.levelObjectClass == LevelObjectClass.Tile) {
			origin = GetOrigin(gridStartPos, gridEndPos);
			limit = GetLimit(gridStartPos, gridEndPos);

			Vector3 scale = limit - origin;
			scale = new Vector3(Mathf.Max(1, scale.x), Mathf.Max(1, scale.y), Mathf.Max(scale.z));

			Transform placingTransform = placing.transform;
		
			placingTransform.localScale = scale;
			placingTransform.position = origin;
		
			placing.SetScaleAndPosition(scale, origin);
		} else {
			Vector3 mousePos = Vector3.zero;
			bool isLevelSurface = LevelManager.levelInputManager.GetMouseLevelSurfacePosition(out RaycastHit hit);

			// If couldn't find a point on the level surface at the given coordinate
			if (!isLevelSurface) {
				bool foundGrid = LevelManager.levelInputManager.GetMouseLevelGridPosition(out mousePos);
			} else {
				mousePos = hit.point;
			}
			
			placing.LevelBuilderHover(hit);

			origin = mousePos;
			placing.transform.position = mousePos;
		}
	}

	private Vector3 GetOrigin(Vector3 startPosition, Vector3 endPosition) {
		float minX = Mathf.Min(startPosition.x, endPosition.x);
		float minY = Mathf.Min(startPosition.y, endPosition.y);
		float minZ = Mathf.Min(startPosition.z, endPosition.z);
		
		return new Vector3(minX, minY, minZ);
	}

	private Vector3 GetLimit(Vector3 startPosition, Vector3 endPosition) {
		float maxX = Mathf.Max(startPosition.x, endPosition.x);
		float maxY = Mathf.Max(startPosition.y, endPosition.y);
		float maxZ = Mathf.Max(startPosition.z, endPosition.z);
		
		return new Vector3(maxX, maxY, maxZ);
	}
	
	public void SetLevelObject(LevelObject levelObject) {
		this.currentLevelObject = levelObject;
		ClearObjectCache();
		objectCache = new List<GameObject>();
	}

	private void ClearObjectCache() {
		if (objectCache == null) {
			return;
		}
		
		foreach (GameObject g in objectCache) {
			Destroy(g);
		}
	}
	
}

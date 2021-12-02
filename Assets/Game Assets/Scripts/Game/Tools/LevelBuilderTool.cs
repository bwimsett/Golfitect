using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Level;
using Backend.Managers;
using Game.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelBuilderTool : Tool {

	private LevelObject currentLevelObject;
	private List<GameObject> objectCache;

	private LevelObject placing;
	//private List<Tuple<Vector3Int, GameObject>> placing;
	private Vector3Int startCoordinate, endCoordinate, origin, limit;
	private bool mouseDown;

	protected override void OnMouseHover() {
		startCoordinate = endCoordinate = mousePosition;
		RefreshScale();
	}

	protected override void OnMouseDown() {
		startCoordinate = endCoordinate = mousePosition;
		mouseDown = true;
		RefreshScale();
	}

	protected override void MouseDown() {
		endCoordinate = mousePosition;
		RefreshScale();
	}

	protected override void OnMouseUp() {
		Place();
		startCoordinate = endCoordinate = mousePosition;
		RefreshScale();
	}

	private void Place() {
		//foreach (Tuple<Vector3Int, GameObject> placingObject in placing) {
			//LevelObject levelObject = placingObject.Item2.GetComponent<LevelObject>();
			//GameManager.currentLevel.PlaceAtCoordinate(placingObject.Item1, levelObject);			
			//objectCache.Remove(placingObject.Item2);
		//}
		
		GameManager.currentLevel.PlaceAtCoordinate(origin, placing);
		placing = null;
	}

	/*private void RefreshPlacing() {
		if (!currentLevelObject) {
			return;
		}
		
		Vector3Int coordinateDiff = endCoordinate - startCoordinate;
		int objectCount = (Mathf.Abs(coordinateDiff.x) + 1) * (Mathf.Abs(coordinateDiff.z) + 1);

		int countDifference = objectCount - objectCache.Count;
		
		// If object count < object cache, instantiate more
		if (countDifference > 0) {
			for (int i = 0; i < countDifference; i++) {
				GameObject newObject = Instantiate(currentLevelObject.gameObject, LevelManager.levelObjectUtility.levelContainer);
				objectCache.Add(newObject);
			}
		}

		int objectIndex = 0;

		int minX = Mathf.Min(startCoordinate.x, endCoordinate.x);
		int minZ = Mathf.Min(startCoordinate.z, endCoordinate.z);
		int maxX = Mathf.Max(startCoordinate.x, endCoordinate.x);
		int maxZ = Mathf.Max(startCoordinate.z, endCoordinate.z);

		placing = new List<Tuple<Vector3Int, GameObject>>();
		
		// Set positions
		for (int x = minX; x <= maxX; x++) {
			for (int z = minZ; z <= maxZ; z++) {
				Vector3Int buildPos = new Vector3Int(x, startCoordinate.y, z);
				
				// Put object at position and show it
				GameObject placingObject = objectCache[objectIndex];
				placingObject.transform.position = LevelManager.levelGrid.GridCoordinateToWorldPosition(buildPos);
				placingObject.SetActive(true);
				objectIndex++;
				
				placing.Add(new Tuple<Vector3Int, GameObject>(buildPos, placingObject));
			}
		}

		for (int i = objectIndex; i < objectCache.Count; i++) {
			// Hide any objects in cache that aren't needed
			if (i >= objectCount) {
				objectCache[i].SetActive(false);
				continue;
			}
		}
		
		//Debug.Log($"Start coordinate: {startCoordinate}, end coordinate: {endCoordinate}, object count: {objectCount}");
	}*/

	private void RefreshScale() {
		if (!placing) {
			placing = Instantiate(currentLevelObject.gameObject, LevelManager.levelObjectUtility.levelContainer).GetComponent<LevelObject>();
		}
		
		origin = GetOrigin();
		limit = GetLimit();

		Vector3Int scale = limit - origin;
		scale = new Vector3Int(Mathf.Max(1, scale.x), Mathf.Max(1, scale.y), Mathf.Max(scale.z));

		Transform placingTransform = placing.transform;
		
		placingTransform.localScale = scale;
		placingTransform.position = LevelManager.levelGrid.GridCoordinateToWorldPosition(origin);
		
		placing.SetScaleAndPosition(scale, origin);
	}

	private Vector3Int GetOrigin() {
		int minX = Mathf.Min(startCoordinate.x, endCoordinate.x);
		int minY = Mathf.Min(startCoordinate.y, endCoordinate.y);
		int minZ = Mathf.Min(startCoordinate.z, endCoordinate.z);
		
		return new Vector3Int(minX, minY, minZ);
	}

	private Vector3Int GetLimit() {
		int maxX = Mathf.Max(startCoordinate.x, endCoordinate.x);
		int maxY = Mathf.Max(startCoordinate.y, endCoordinate.y);
		int maxZ = Mathf.Max(startCoordinate.z, endCoordinate.z);
		
		return new Vector3Int(maxX, maxY, maxZ);
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

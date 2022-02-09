using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Enums;
using Backend.Level;
using Backend.Managers;
using Game.LevelBuilder;
using Game.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelBuilderTool : Tool {

	private LevelObject currentLevelObject;

	private LevelObjectScaleParent placing;
	private Vector3 startPosition, endPosition, origin, limit;
	private bool mouseDown;
	private float rotationAmount;
	private int tileHeight = 1;
	
	[SerializeField] private KeyCode rotateKey;
	private float rotation;

	protected override void ToolUpdate() {
		base.ToolUpdate();

		if (Input.GetKeyDown(rotateKey)) {
			Rotate();
		}
	}

	protected override void OnMouseHover() {
		startPosition = endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnMouseDown() {
		if (pointerOverGUI) {
			mouseDown = false;
			return;
		}
		
		startPosition = endPosition = mousePosition;
		mouseDown = true;
		LevelManager.cameraController.DisableDrag(this);
		RefreshScaleAndPosition();
	}

	protected override void MouseDown() {
		if (!mouseDown) {
			return;
		}
		
		endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnMouseUp() {
		if (!mouseDown) {
			return;
		}

		mouseDown = false;
		LevelManager.cameraController.EnableDrag(this);
		Place();
		startPosition = endPosition = mousePosition;
		RefreshScaleAndPosition();
	}

	protected override void OnRightMouseDown() {
		SetActive(false);
	}

	protected override void Deactivate() {
		mouseDown = false;
		LevelManager.cameraController.EnableDrag(this);
		LevelManager.buildModeHUD.DeselectBuildOptionFromDock(currentLevelObject);
		currentLevelObject = null;
		if (placing.levelObject) {
			placing.levelObject.OnLevelBuilderCancel();
		}
		Destroy(placing.gameObject);
	}

	private void Rotate() {
		if(!placing.levelObject.rotatable) {
			return;
		}
		
		rotation += 90;
		if (rotation >= 360) {
			rotation = 0;
		}
	}

	public void SetTileHeight(int tileHeight) {
		this.tileHeight = tileHeight;
		RefreshScaleAndPosition();
	}

	private void Place() {
		//foreach (Tuple<Vector3Int, GameObject> placingObject in placing) {
			//LevelObject levelObject = placingObject.Item2.GetComponent<LevelObject>();
			//GameManager.currentLevel.Place(placingObject.Item1, levelObject);			
			//objectCache.Remove(placingObject.Item2);
		//}
		
		// Move LevelObject out of the ScaleParent
		LevelObject levelObject = placing.levelObject;
		levelObject.transform.parent = LevelManager.levelObjectUtility.levelContainer;
		Destroy(placing.gameObject);
		
		GameManager.currentLevel.Place(origin, levelObject);
		placing = null;
	}

	private void RefreshScaleAndPosition() {
		if (!placing) {
			LevelObject lvlObject = Instantiate(currentLevelObject.gameObject).GetComponent<LevelObject>();
			// Create a scale parent to contain the object for rotations
			placing = new GameObject("Placing Parent", new[] { typeof(LevelObjectScaleParent) }).GetComponent<LevelObjectScaleParent>();
			placing.transform.SetParent(LevelManager.levelObjectUtility.levelContainer);
			lvlObject.transform.SetParent(placing.transform);
			placing.levelObject = lvlObject;
		}
		
		Vector3 gridStartPos = LevelManager.levelGrid.WorldPositionToGridPosition(startPosition);
		Vector3 gridEndPos = LevelManager.levelGrid.WorldPositionToGridPosition(endPosition);
		
		// Only scale tile object types
		if (placing.levelObject.levelObjectClass == LevelObjectClass.Tile) {
			origin = GetOrigin(gridStartPos, gridEndPos);
			limit = GetLimit(gridStartPos, gridEndPos)+new Vector3(1,0,1); // Add one to x and z to account for scale
			
			Debug.Log("Start: "+gridStartPos.ToString()+"EndPosition: "+gridEndPos.ToString()+"Limit: "+limit.ToString()+", origin: "+origin.ToString());

			Vector3 scale = limit - origin;
			scale = new Vector3(Mathf.Max(1, scale.x),  LevelManager.levelGrid.GridHeightToWorldHeight(tileHeight), Mathf.Max(1, scale.z));

			if (placing.levelObject.lockXScale) { scale.x = 1; }
			if (placing.levelObject.lockYScale) { scale.y = 1; }
			if (placing.levelObject.lockZScale) { scale.z = 1; }

			placing.SetScalePositionAndRotation(scale, origin, new Vector3(0, rotation, 0));
		} else {
			Vector3 pos = Vector3.zero;
			bool isLevelSurface = LevelManager.levelInputManager.GetMouseLevelSurfacePosition(out RaycastHit hit);

			// If couldn't find a point on the level surface at the given coordinate
			if (!isLevelSurface) {
				bool foundGrid = LevelManager.levelInputManager.GetMouseLevelGridPosition(out pos);
			} else {
				pos = hit.point;
			}
			
			placing.levelObject.LevelBuilderHover(hit);

			if (placing.levelObject.snapToGrid) {
				pos = LevelManager.levelGrid.WorldPositionToGridPosition(pos);
			}

			placing.SetScalePositionAndRotation(Vector3.one, pos, new Vector3(0, rotation, 0));
			origin = pos;
			placing.transform.position = pos;
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
		if (placing) {
			Destroy(placing.gameObject);
		}
		placing = null;

		if (!levelObject.rotatable) {
			rotation = 0;
		}
		
		RefreshScaleAndPosition();
	}

}

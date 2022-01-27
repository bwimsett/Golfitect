using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Managers;
using Shapes;
using UnityEngine;

[ExecuteAlways]
public class LevelGrid : MonoBehaviour {

	private Grid grid;
	public Vector3 cellDimensions => grid.cellSize;
	public BoxCollider collider { get; private set; }
	[SerializeField] private float gridThickness;
	[SerializeField] private Color gridColor;
	public bool drawGrid;

	private Vector3 gridOffset;
	private int layer = 0;
	private float layer_y;
	[SerializeField] private KeyCode layerUpKey, layerDownKey;
	
	void Update() {
		if (Input.GetKeyDown(layerUpKey)) {
			IncrementGridLayer(1);
		}

		if (Input.GetKeyDown(layerDownKey)) {
			IncrementGridLayer(-1);
		}
	}
	
	public void Initialise() {
		grid = GetComponent<Grid>();
		collider = GetComponent<BoxCollider>();

		Vector3Int levelDimensions = GameManager.currentLevel.levelDimensions;
		collider.size = new Vector3(levelDimensions.x, 0.1f, levelDimensions.z);
		collider.center = new Vector3(-collider.size.x / 2f, 0, collider.size.z / 2f);
		gridOffset = new Vector3(-levelDimensions.x, 0);
	}

	public void DrawGrid() {
		if (!drawGrid) {
			return;
		}
		
		Draw.LineGeometry = LineGeometry.Volumetric3D;
		Draw.ThicknessSpace = ThicknessSpace.Meters;
		Draw.Thickness = gridThickness;
		//Draw.Matrix = transform.localToWorldMatrix;

		Vector3Int dimensions = GameManager.currentLevel.levelDimensions;

		int width = dimensions.x;
		int depth = dimensions.z;

		for (int pos = 0; pos <= dimensions.z; pos++) {
			
			Vector3Int startCoord = new Vector3Int(pos, layer, 0);
			Vector3Int endCoord = new Vector3Int(pos, layer, depth);
			Vector3 worldStart = GridCoordinateToWorldPosition(startCoord)+gridOffset;
			Vector3 worldEnd = GridCoordinateToWorldPosition(endCoord)+gridOffset;
			worldStart.y = worldEnd.y = layer_y;

			Draw.Line(worldStart, worldEnd, gridColor);

			startCoord = new Vector3Int(0, layer, pos);
			endCoord = new Vector3Int(width, layer, pos);
			worldStart = GridCoordinateToWorldPosition(startCoord)+gridOffset;
			worldEnd = GridCoordinateToWorldPosition(endCoord)+gridOffset;
			worldStart.y = worldEnd.y = layer_y;
			Draw.Line(worldStart, worldEnd, gridColor);
		}
		
	}

	public Vector3Int WorldPositionToGridCoordinate(Vector3 position) {
		return grid.WorldToCell(position);
	}

	public Vector3 WorldPositionToGridPosition(Vector3 position) {
		return GridCoordinateToWorldPosition(WorldPositionToGridCoordinate(position));
	}

	public Vector3 GridCoordinateToWorldPosition(Vector3Int gridCoordinate) {
		return grid.CellToWorld(gridCoordinate);
	}

	public Vector3 WorldScaleToGridScale(Vector3 scale) {
		return new Vector3(scale.x * grid.cellSize.x, scale.y*grid.cellSize.y, scale.z*grid.cellSize.z);
	}

	public float GridHeightToWorldHeight(int gridHeight) {
		return gridHeight * grid.cellSize.y;
	}

	public void IncrementGridLayer(int amount) {
		layer = Mathf.Min(GameManager.currentLevel.levelDimensions.y / 2, Mathf.Max(-GameManager.currentLevel.levelDimensions.y / 2, layer + amount));
		layer_y = layer * grid.cellSize.y;
		transform.position = new Vector3(transform.position.x, layer_y, transform.position.z);
	}

}

using System.Collections;
using System.Collections.Generic;
using Backend.Managers;
using UnityEngine;

public class LevelGrid : MonoBehaviour {

	private Grid grid;
	private BoxCollider collider;
	
	public void Initialise() {
		grid = GetComponent<Grid>();
		collider = GetComponent<BoxCollider>();

		Vector3Int levelDimensions = GameManager.currentLevel.levelDimensions;
		collider.size = new Vector3(levelDimensions.x, 0.1f, levelDimensions.z);
	}

	public Vector3Int WorldPositionToGridCoordinate(Vector3 position) {
		return grid.WorldToCell(position);
	}

	public Vector3 WorldPositionToGridPosition(Vector3 position) {
		return GridCoordinateToWorldPosition(WorldPositionToGridCoordinate(position));
	}

	public Vector3 GridCoordinateToWorldPosition(Vector3Int gridCoordinate) {
		Vector3 pos = grid.GetCellCenterWorld(gridCoordinate);
		pos = pos - grid.cellSize / 2f;
		return pos;
	}

	public Vector3 WorldScaleToGridScale(Vector3 scale) {
		return new Vector3(scale.x * grid.cellSize.x, scale.y*grid.cellSize.y, scale.z*grid.cellSize.z);
	}
	
}

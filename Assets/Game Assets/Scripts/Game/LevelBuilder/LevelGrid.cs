using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {

	private Grid grid;
	private BoxCollider collider;
	
	public void Initialise() {
		grid = GetComponent<Grid>();
		collider = GetComponent<BoxCollider>();

		Vector3Int levelDimensions = LevelManager.currentLevel.levelDimensions;
		collider.size = new Vector3(levelDimensions.x, 0.1f, levelDimensions.z);
	}

	public Vector3Int WorldPositionToGridCoordinate(Vector3 position) {
		return grid.WorldToCell(position);
	}

	public Vector3 GridCoordinateToWorldPosition(Vector3Int gridCoordinate) {
		Vector3 pos = grid.GetCellCenterWorld(gridCoordinate);
		pos = pos - grid.cellSize / 2f;
		return pos;
	}
	
}

using System.Collections.Generic;
using Backend.Managers;
using UnityEngine;

namespace Backend.Level {
	public class FlatGround : LevelObject {

		[SerializeField] private MeshRenderer meshRenderer;
		private List<FlatGround> splitObjects;
		[HideInInspector] public bool isTemporary;

		/// <summary>
		/// Temporarily remove a tile at the given point
		/// </summary>
		public void TempSplit(Vector3 splitPoint) {
			if (isTemporary) {
				return;
			}
			
			Vector3Int originGridPos = LevelManager.levelGrid.WorldPositionToGridCoordinate(origin);
			Vector3Int limitGridPos = LevelManager.levelGrid.WorldPositionToGridCoordinate(origin + scale);
			Vector3Int splitGridPos = LevelManager.levelGrid.WorldPositionToGridCoordinate(splitPoint);

			// Ignore invalid coordinates
			if (splitGridPos.x >= limitGridPos.x || splitGridPos.y >= limitGridPos.y || splitGridPos.z >= limitGridPos.z) {
				return;
			}
			
			// Calculate origins of the 4 splits
			Vector3Int[] origins = new Vector3Int[4];
			origins[0] = new Vector3Int(originGridPos.x,originGridPos.y, splitGridPos.z + 1);
			origins[1] = new Vector3Int(splitGridPos.x + 1, originGridPos.y, splitGridPos.z);
			origins[2] = new Vector3Int(splitGridPos.x, originGridPos.y, originGridPos.z);
			origins[3] = originGridPos;
			
			// Calculate limits of the 4 splits
			Vector3Int[] limits = new Vector3Int[4];
			limits[0] = new Vector3Int(splitGridPos.x+1, limitGridPos.y, limitGridPos.z);
			limits[1] = limitGridPos;
			limits[2] = new Vector3Int(limitGridPos.x, limitGridPos.y, splitGridPos.z);
			limits[3] = new Vector3Int(splitGridPos.x, limitGridPos.y, splitGridPos.z+1);

			splitObjects = new List<FlatGround>();

			// Create new objects for each of the splits that have volume
			for (int i = 0; i < origins.Length; i++) {
				Vector3Int dimensions = limits[i] - origins[i];
				int volume = Mathf.Abs(dimensions.x * dimensions.y * dimensions.z);
				if (volume == 0) {
					continue;
				}

				FlatGround splitObject = Instantiate(gameObject, LevelManager.levelObjectUtility.levelContainer).GetComponent<FlatGround>();
				splitObjects.Add(splitObject);
				splitObject.isTemporary = true;
				splitObject.SetScaleAndPosition(LevelManager.levelGrid.WorldScaleToGridScale(dimensions), LevelManager.levelGrid.GridCoordinateToWorldPosition(origins[i]), Vector3.zero, true);
				splitObject.buildModeCollider.enabled = false;
			}
			meshRenderer.enabled = false;
		}

		public void ResetTempSplit() {
			if (splitObjects == null) {
				return;
			}
			
			foreach (LevelObject levelObject in splitObjects) {
				Destroy(levelObject.gameObject);
			}

			splitObjects = new List<FlatGround>();
			meshRenderer.enabled = true;
		}

		public void ConfirmSplit() {
			if (splitObjects == null || splitObjects.Count == 0) {
				return;
			}

			foreach (FlatGround levelObject in splitObjects) {
				GameManager.currentLevel.Place(levelObject.origin, levelObject);
				levelObject.buildModeCollider.enabled = true;
				levelObject.isTemporary = false;
			}
			
			// Destroy this object
			GameManager.currentLevel.DestroyLevelObject(this);
		}
	}
}
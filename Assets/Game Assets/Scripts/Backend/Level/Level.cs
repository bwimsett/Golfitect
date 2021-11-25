using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backend.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Level {
	[Serializable]
	public class Level {

		public string levelName = "Test";
		
		public Vector3Int levelDimensions { get; private set; }
		[JsonProperty] private List<string> objectTypesUsed; // Lists the IDs of all the tile types used. Indexed by LevelTile for the tileType field to keep save files small.
		[JsonProperty] private Dictionary<int, LevelObjectSave> objectSaves;
		private Dictionary<int, LevelObject> objects;

		private int[,,] tileGrid; // Grid of all TILE objects in the world. Does not include regular objects.
		
		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
			tileGrid = new int[levelDimensions.x, levelDimensions.y, levelDimensions.z];
			objects = new Dictionary<int, LevelObject>();
		}

		public void PlaceAtCoordinate(Vector3Int origin, LevelObject levelObject) {
			Vector3Int[] placingCoordinates = levelObject.GetPlacingCoordinates(origin);
			levelObject.origin = origin;
			levelObject.Construct();
			
			switch (levelObject.LevelObjectClass) {
				case LevelObjectClass.Tile: SetTiles(placingCoordinates, levelObject.objectID); break;
			}
			
			// Add to list of objects in the level
			objects.Add(levelObject.objectID, levelObject);
			
			levelObject.gameObject.SetActive(true);
		}

		private void SetTiles(Vector3Int[] tiles, int objectID) {
			foreach (Vector3Int tile in tiles) {
				tileGrid[tile.x, tile.y, tile.z] = objectID;
			}
		}

		public int GetObjectIndexFromID(string objectTypeID) {
			if (objectTypesUsed == null) {
				objectTypesUsed = new List<string>();
			}
			
			for (int i = 0; i < objectTypesUsed.Count; i++) {
				if (objectTypesUsed[i].Equals(objectTypeID)) {
					return i;
				}
			}

			// Add the objectTypeID to the list of tile types used
			objectTypesUsed.Add(objectTypeID);
			return objectTypesUsed.Count - 1;
		}
		
		public string GetObjectTypeIDFromIndex(int index) {
			if (index < 0 || index >= objectTypesUsed.Count) {
				throw new Exception($"Invalid object index: {index}");
			}
			
			return objectTypesUsed[index];
		}

		private void UpdateObjectSaves() {
			objectSaves = new Dictionary<int, LevelObjectSave>();
			
			foreach (KeyValuePair<int, LevelObject> levelObject in objects) {
				objectSaves.Add(levelObject.Key, levelObject.Value.Save());
			}
		}
		
		public void Save() {
			UpdateObjectSaves();
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context) {
			tileGrid = new int[levelDimensions.x, levelDimensions.y, levelDimensions.z];
		}


	}
}
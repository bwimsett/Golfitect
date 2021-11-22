using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Level {
	[Serializable]
	public class Level {
		public Vector3Int levelDimensions { get; private set; }
		[JsonProperty] private List<string> objectTypesUsed; // Lists the IDs of all the tile types used. Indexed by LevelTile for the tileType field to keep save files small.
		private LevelTile[,,] tiles;

		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
			tiles = new LevelTile[levelDimensions.x, levelDimensions.y, levelDimensions.z];
		}

		public void SetTile(Vector3Int coordinate, LevelObject tileObject) {
			LevelTile tile;
			
			if (tiles[coordinate.x, coordinate.y, coordinate.z] == null) {
				tile = tiles[coordinate.x, coordinate.y, coordinate.z] = new LevelTile(coordinate);
			} else {
				tile = tiles[coordinate.x, coordinate.y, coordinate.z];
			}

			tile.SetTile(tileObject);
		}

		public void SetTiles(Vector3Int[] coordinates, LevelObject tileObject) {
			
		}

		public int GetObjectIndexFromID(string objectTypeID) {
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

		[OnDeserialized]
		private void OnDeserialized() {
			tiles = new LevelTile[levelDimensions.x, levelDimensions.y, levelDimensions.z];
		}


	}
}
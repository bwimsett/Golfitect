using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Backend.Level {
	[Serializable]
	public class Level {
		public Vector3Int levelDimensions { get; private set; }
		
		[JsonProperty] private List<string> objectTypesUsed; // Lists the IDs of all the tile types used. Indexed by LevelTile for the tileType field to keep save files small.
		[JsonProperty] private Dictionary<int, LevelObjectSave> objectSaves;
		
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

			tileObject.objectTypeIndex = GetObjectIndexFromID(tileObject.objectTypeID);
			tile.SetTile(tileObject);
		}

		public void SetTiles(Vector3Int[] coordinates, LevelObject tileObject) {
			foreach (Vector3Int coordinate in coordinates) {
				SetTile(coordinate, tileObject);
			}
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

		private void UpdateObjectSaves() {
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			int depth = tiles.GetLength(2);

			objectSaves = new Dictionary<int, LevelObjectSave>();
			
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					for (int z = 0; z < depth; z++) {
						if (tiles[x,y,z] == null) {
							continue;
						}
						
						LevelObject tileObject = tiles[x, y, z].GetTileObject();

						if (!tileObject) {
							continue;
						}
						
						objectSaves.Add(tileObject.objectID, tileObject.Save());
					}
				}
			}
		}
		
		public void Save() {
			UpdateObjectSaves();
		}

		[OnDeserialized]
		private void OnDeserialized() {
			tiles = new LevelTile[levelDimensions.x, levelDimensions.y, levelDimensions.z];
		}


	}
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backend.Enums;
using Backend.Managers;
using Backend.Serialization;
using Backend.Submittable;
using Newtonsoft.Json;
using UnityEditor.SceneTemplate;
using UnityEngine;

namespace Backend.Level {
	[Serializable]
	public class Level : ISteamSerializable {

		public string title { get; set; }
		public string description { get; set; }
		public string fileExtension { get => "golflvl"; }
		public string saveFolderName { get => "levels"; }
		
		public Vector3Int levelDimensions { get; private set; }
		[JsonProperty] private List<string> objectTypesUsed; // Lists the IDs of all the tile types used. Indexed by LevelTile for the tileType field to keep save files small.
		[JsonProperty] private Dictionary<int, LevelObjectSave> objectSaves;
		private Dictionary<int, LevelObject> objects;

		private int[,,] tileGrid; // Grid of all TILE objects in the world. Does not include regular objects.
		
		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
			tileGrid = new int[levelDimensions.x, levelDimensions.y, levelDimensions.z];
			objectTypesUsed = new List<string>();
			objectSaves = new Dictionary<int, LevelObjectSave>();
			objects = new Dictionary<int, LevelObject>();
		}

		public void PlaceAtCoordinate(Vector3Int origin, LevelObject levelObject) {
			if (!levelObject.isGridTile) {
				return;
			}
			
			Vector3Int[] placingCoordinates = levelObject.GetPlacingCoordinates(origin);
			levelObject.origin = origin;
			levelObject.Construct();
			
			switch (levelObject.levelObjectClass) {
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
		
		public void Load() {
			if (objectSaves == null) {
				return;
			}
			
			// First, get a prefab for each of the object types used
			LevelObject[] prefabs = new LevelObject[objectTypesUsed.Count];
			for (int i = 0; i < prefabs.Length; i++) {
				prefabs[i] = LevelManager.levelObjectUtility.objectBank.GetLevelObjectWithID(objectTypesUsed[i]);
			}

			// Then run through the objects in the level and place them at the given coordinates
			foreach (KeyValuePair<int, LevelObjectSave> save in objectSaves) {
				LevelObjectSave saveObj = save.Value;
				
				// Select the prefab
				LevelObject prefab = prefabs[saveObj.objectTypeIndex];

				// Instantiate
				LevelObject levelObject = LevelManager.levelObjectUtility.InstantiatePrefab(prefab, saveObj.origin);

				// Provide object save to instantiated object and load
				levelObject.Load(saveObj);
				
				// Add the object to the object list
				objects.Add(levelObject.objectID, levelObject);

				// Add the object to the grid if necessary
				if (levelObject.isGridTile) {
					PlaceAtCoordinate(LevelManager.levelGrid.WorldPositionToGridCoordinate(levelObject.origin), levelObject);
				}
			}
		}


		[OnDeserialized]
		private void OnDeserialized(StreamingContext context) {
			tileGrid = new int[levelDimensions.x, levelDimensions.y, levelDimensions.z];
		}


	}
}
using System;
using System.Collections.Generic;
using Backend.Enums;
using Backend.Serialization;
using Game;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Backend.Level {
	[Serializable]
	public class Level : ServerSerializable {

		public static int maxPar = 12;
		
		public int par = 3;
		public string name ="New Level", description = "Description";

		[HideInInspector, JsonIgnore] public Ball ball;

		public Vector3Int levelDimensions { get; private set; }
		[JsonProperty] private List<string> objectTypesUsed; // Lists the IDs of all the tile types used. Indexed by LevelTile for the tileType field to keep save files small.
		[JsonProperty] private Dictionary<int, LevelObjectSave> objectSaves;
		private Dictionary<int, LevelObject> objects;

		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
			objectTypesUsed = new List<string>();
			objectSaves = new Dictionary<int, LevelObjectSave>();
			objects = new Dictionary<int, LevelObject>();
		}

		public void Play() {
			foreach (KeyValuePair<int, LevelObject> obj in objects) {
				obj.Value.EnterPlayMode();
			}
			
			LevelManager.levelGrid.drawGrid = false;
			RefreshLevelCollider();
			LevelManager.levelInputManager.ballControllerTool.SetActive(true);
		}
		
		public void Place(Vector3 origin, LevelObject levelObject) {
			levelObject.origin = origin;
			levelObject.Construct();

			// Add to list of objects in the level
			objects.Add(levelObject.objectID, levelObject);

			levelObject.gameObject.SetActive(true);
		}

		public void DestroyLevelObject(LevelObject levelObject) {
			if (!objects.ContainsValue(levelObject)) {
				return;
			}

			objects.Remove(levelObject.objectID);
			Object.Destroy(levelObject.gameObject);
		}
		
		public void RefreshLevelCollider() {
			Mesh colliderMesh = new Mesh();
			List<CombineInstance> combineInstances = new List<CombineInstance>();
			
			foreach (KeyValuePair<int, LevelObject> obj in objects) {
				if (!obj.Value.ballCollisions) {
					continue;
				}
				
				combineInstances.AddRange(obj.Value.CombineMeshes());
			}

			colliderMesh.CombineMeshes(combineInstances.ToArray(), true, true);

			LevelManager.levelCollider.sharedMesh = colliderMesh;
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
		
		public override void Save() {
			UpdateObjectSaves();
		}

		public void Load() {
			if (objectSaves == null) {
				return;
			}
			
			LevelManager.levelObjectUtility.ClearLevel();
			
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
				LevelObject levelObject = LevelManager.levelObjectUtility.InstantiatePrefab(prefab, saveObj.origin.Vector3());

				// Provide object save to instantiated object and load
				levelObject.Load(saveObj);
				
				// Add the object to the object list
				objects.Add(levelObject.objectID, levelObject);

				// Add the object to the grid if necessary
				if (levelObject.levelObjectClass != LevelObjectClass.Tile) {
					Place(LevelManager.levelGrid.WorldPositionToGridCoordinate(levelObject.origin), levelObject);
				}
			}
			
			LevelManager.InitialiseGameMode();
		}
	}
}
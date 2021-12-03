using System;
using System.Collections.Generic;
using System.Linq;
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

		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
			objectTypesUsed = new List<string>();
			objectSaves = new Dictionary<int, LevelObjectSave>();
			objects = new Dictionary<int, LevelObject>();
		}

		public void Place(Vector3 origin, LevelObject levelObject) {
			levelObject.origin = origin;
			levelObject.Construct();

			// Add to list of objects in the level
			objects.Add(levelObject.objectID, levelObject);
			RefreshLevelCollider();
			
			levelObject.gameObject.SetActive(true);
		}
		
		public void RefreshLevelCollider() {
			Mesh colliderMesh = new Mesh();
			List<CombineInstance> combineInstances = new List<CombineInstance>();

			int index = 0;
			foreach (KeyValuePair<int, LevelObject> obj in objects) {
				Mesh mesh = obj.Value.meshFilter.mesh;

				for (int i = 0; i < mesh.subMeshCount; i++) {
					CombineInstance combineInstance = new CombineInstance();
					combineInstance.mesh = obj.Value.meshFilter.mesh;
					combineInstance.transform = obj.Value.transform.localToWorldMatrix;
					combineInstance.subMeshIndex = i;
					combineInstances.Add(combineInstance);
				}
				
				index++;
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
				if (levelObject.levelObjectClass != LevelObjectClass.Tile) {
					Place(LevelManager.levelGrid.WorldPositionToGridCoordinate(levelObject.origin), levelObject);
				}
			}
		}
	}
}
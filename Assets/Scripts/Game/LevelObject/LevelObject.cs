using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;

namespace Backend.Level {
	public class LevelObject : MonoBehaviour {

		public string objectTypeID;
		[HideInInspector] public int objectTypeIndex;
		public int objectID;
		public Texture2D[] layout;
		[HideInInspector] public List<Vector3Int> coordinates;

		public void Construct() {
			objectID = gameObject.GetInstanceID();
			ConstructLevelObject();
		}

		public virtual void ConstructLevelObject() {
			
		}
		
		public virtual LevelObjectSave Save() {
			return new LevelObjectSave(this);
		}

		public void Load(LevelObjectSave save) {
			this.objectTypeID = save.objectTypeID;
			this.objectID = save.objectID;
			this.coordinates = save.coordinates;
			LoadLevelObject(save);
		}

		protected void LoadLevelObject(LevelObjectSave save) {
			
		}

		public LevelObjectLayout[,,] GetLayout() {
			if (layout == null || layout.Length == 0) {
				throw new Exception($"Layout sprites not set for: {gameObject.name}");
			}
			
			int width = layout[0].width;
			int depth = layout[0].height;
			int height = layout.Length;
			
			LevelObjectLayout[,,] levelLayout = new LevelObjectLayout[width, height, depth];

			for (int z = 0; z < depth; z++) {
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						bool occupied = layout[y].GetPixel(x, z).r > 0;
						if (occupied) {
							levelLayout[x, y, z] = LevelObjectLayout.Occupied;
						} else {
							levelLayout[x, y, z] = LevelObjectLayout.Unoccupied;
						}
					}
				}
			}

			return levelLayout;
		}

		public Vector3Int[] GetPlacingCoordinates(Vector3Int origin) {
			LevelObjectLayout[,,] levelLayout = GetLayout();

			int width = levelLayout.GetLength(0);
			int height = levelLayout.GetLength(1);
			int depth = levelLayout.GetLength(2);

			List<Vector3Int> coordinates = new List<Vector3Int>();
			
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < depth; y++) {
					for (int z = 0; z < height; z++) {
						if (levelLayout[x, y, z] == LevelObjectLayout.Occupied) {
							coordinates.Add(new Vector3Int(x,y,z));
						}	
					}
				}
			}

			return coordinates.ToArray();
		}

	}
	
	[Serializable]
	public class LevelObjectSave {
		[JsonProperty] private int objectTypeIndex; // Index of objecttypeid stored in the level class
		[JsonIgnore] public string objectTypeID { get => LevelManager.currentLevel.GetObjectTypeIDFromIndex(objectTypeIndex); }
		public int objectID;
		public List<Vector3Int> coordinates;

		public LevelObjectSave(LevelObject levelObject) {
			objectTypeIndex = levelObject.objectTypeIndex;
			objectID = levelObject.objectID;
			coordinates = levelObject.coordinates;
		}
	}
}
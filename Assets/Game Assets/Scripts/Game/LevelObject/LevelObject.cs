using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backend.Enums;
using Backend.Managers;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Steamworks;
using UnityEngine;

namespace Backend.Level {
	public class LevelObject : MonoBehaviour {

		public string objectTypeID;
		public LevelObjectClass LevelObjectClass;
		public bool isGridTile;
		[ShowIf("isGridTile")]
		public Texture2D[] layout;
		[HideInInspector] public int objectID;
		[HideInInspector] public Vector3Int origin, scale;

		[SerializeField] private bool tileTextures;
		[ShowIf("tileTextures"), SerializeField] private int[] horizontalTextures, verticalTextures;

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
			save.Load(this);
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

		public void SetScaleAndPosition(Vector3Int scale, Vector3Int position) {
			this.scale = scale;
			this.origin = position;
			
			RefreshTextures();
		}

		protected virtual void RefreshTextures() {
			if (!tileTextures) {
				return;
			}

			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
			Vector2 horizontalScale = new Vector2(scale.x, scale.z)*2;
			Vector2 verticalScale = new Vector2(scale.x, scale.y)*2;
			Vector2 horizontalOffset = new Vector2(0, 0)/2f;
			Vector2 verticalOffset = new Vector2(0, -origin.y%2)/2f;

			Vector3 scaleAbs = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
			Vector3 originAbs = new Vector3(Mathf.Abs(origin.x), Mathf.Abs(origin.y), Mathf.Abs(origin.z));

			// ENSURE ALL TILES ALWAYS START AT THE SAME POINT ON THE TEXTURE
			if (scaleAbs.x % 2 > 0) {
				horizontalOffset = new Vector2(-0.25f, 0);
			} 

			if (scaleAbs.z % 2 > 0) {
				horizontalOffset = new Vector2(horizontalOffset.x, 0.5f);
			}

			int divisor = Mathf.CeilToInt(scaleAbs.x / 2);
			horizontalOffset = new Vector2(horizontalOffset.x, horizontalOffset.y+divisor*0.5f);
			
			// SHIFT THE TEXTURE DEPENDING ON POSITION
			int xPlusZ = Mathf.RoundToInt(originAbs.x + originAbs.z);
			if (xPlusZ % 2 > 0) {
				horizontalOffset = horizontalOffset + new Vector2(0, 0.5f);
			}

			foreach (int i in horizontalTextures) {
				//Debug.Log(meshRenderer.materials[i].GetTexturePropertyNames()[0]);
				meshRenderer.materials[i].mainTextureScale = horizontalScale;
				meshRenderer.materials[i].mainTextureOffset = horizontalOffset;
			}

			foreach (int i in verticalTextures) {
				meshRenderer.materials[i].mainTextureScale = verticalScale;
				meshRenderer.materials[i].mainTextureOffset = verticalOffset;
			}
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
							coordinates.Add(new Vector3Int(x+origin.x,y+origin.y,z+origin.z));
						}	
					}
				}
			}

			return coordinates.ToArray();
		}

	}
	
	[Serializable]
	public class LevelObjectSave {
		[JsonProperty] public int objectTypeIndex { get; private set; } // Index of objecttypeid stored in the level class
		public int objectID;
		public Vector3Int origin, scale;

		public LevelObjectSave() {
			
		}

		public LevelObjectSave(LevelObject levelObject) {
			objectTypeIndex = GameManager.currentLevel.GetObjectIndexFromID(levelObject.objectTypeID);
			objectID = levelObject.objectID;
			origin = levelObject.origin;
			scale = levelObject.scale;
		}

		public void Load(LevelObject levelObject) {
			levelObject.objectID = objectID;
			levelObject.origin = origin;
			levelObject.scale = scale;
			LoadLevelObject();
		}

		protected virtual void LoadLevelObject() {
			
		}

	}
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Backend.Enums;
using Backend.Managers;
using Backend.Serialization;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Steamworks;
using UnityEngine;

namespace Backend.Level {
	public class LevelObject : MonoBehaviour {

		public string objectTypeID;
		public LevelObjectClass levelObjectClass;
		public Sprite buildMenuIcon;
		public bool showInBuildMenuDock, ballCollisions = true;
		[HideInInspector] public int objectID;
		[HideInInspector] public Vector3 origin, scale;

		[SerializeField] private bool tileTextures;
		[ShowIf("tileTextures"), SerializeField] private int[] horizontalTextures, verticalTextures;

		public MeshFilter meshFilter;

		public void Construct() {
			objectID = gameObject.GetInstanceID();
			ConstructLevelObject();
		}

		public virtual void ConstructLevelObject() {
			
		}

		public virtual void EnterPlayMode() {
			
		}
		
		public virtual LevelObjectSave Save() {
			return new LevelObjectSave(this);
		}

		public void Load(LevelObjectSave save) {
			save.Load(this);
		}

		protected void LoadLevelObject(LevelObjectSave save) {
			
		}

		public void SetScaleAndPosition(Vector3 scale, Vector3 position) {
			this.scale = scale;
			this.origin = position;

			transform.localScale = scale;
			transform.localPosition = position;
			
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

	}
	
	[Serializable]
	public class LevelObjectSave {
		[JsonProperty] public int objectTypeIndex { get; private set; } // Index of objecttypeid stored in the level class
		public int objectID;
		public Vector3Save origin, scale;

		public LevelObjectSave() {
			
		}

		public LevelObjectSave(LevelObject levelObject) {
			objectTypeIndex = GameManager.currentLevel.GetObjectIndexFromID(levelObject.objectTypeID);
			objectID = levelObject.objectID;
			origin = new Vector3Save(levelObject.origin);
			scale = new Vector3Save(levelObject.scale);
		}

		public void Load(LevelObject levelObject) {
			levelObject.objectID = objectID;
			levelObject.SetScaleAndPosition(scale.Vector3(), origin.Vector3());
			LoadLevelObject();
		}

		protected virtual void LoadLevelObject() {
			
		}

	}
}
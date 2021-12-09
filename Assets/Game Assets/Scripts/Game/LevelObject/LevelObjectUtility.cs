using System.ComponentModel;
using Backend.Managers;
using UnityEngine;

namespace Backend.Level {
	public class LevelObjectUtility : MonoBehaviour {

		public LevelObjectBank objectBank;
		public Transform levelContainer;
		
		public LevelObject InstantiatePrefab(LevelObject prefab, Vector3 position) {
			LevelObject newObject = Instantiate(prefab.gameObject, levelContainer).GetComponent<LevelObject>();
			newObject.transform.localPosition = position;
			return newObject;
		}

		public LevelObject InstantiatePrefab(LevelObject prefab, Vector3Int position) {
			Vector3 newPos = LevelManager.levelGrid.GridCoordinateToWorldPosition(position);
			return InstantiatePrefab(prefab, newPos);
		}

		public void ClearLevel() {
			int childCount = levelContainer.childCount;

			for (int i = 0; i < childCount; i++) {
				Destroy(levelContainer.GetChild(i).gameObject);
			}
			
		}
		
	}
}
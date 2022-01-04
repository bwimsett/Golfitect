using Backend.Enums;
using Backend.Level;
using UnityEngine;

namespace Game.LevelBuilder {
	public class LevelObjectScaleParent : MonoBehaviour {
		public LevelObject levelObject;

		public void SetScalePositionAndRotation(Vector3 scale, Vector3 position, Vector3 rotation) {
			transform.localPosition = position;
			transform.localScale = scale;
			
			levelObject.RefreshBuildModeCollider();

			int rotationAmount = Mathf.RoundToInt(rotation.y/90);
			
			// Rotate the level object
			if (levelObject.levelObjectClass == LevelObjectClass.Tile) {
				switch (rotationAmount) {
					case 0: levelObject.transform.localPosition = new Vector3(0, 0, 0);
						break;
					case 1: levelObject.transform.localPosition = new Vector3(0, 0, 1);
						break;
					case 2: levelObject.transform.localPosition = new Vector3(1, 0, 1);
						break;
					case 3: levelObject.transform.localPosition = new Vector3(1, 0, 0);
						break;
				}
			}
			
			levelObject.SetScaleAndPosition(scale, position, rotation, false);
			
			levelObject.transform.rotation = Quaternion.Euler(rotation);
		}
	}
}
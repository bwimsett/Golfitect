using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Backend.Level {
	[CreateAssetMenu(menuName = "Level Object Bank", fileName = "Level Object Bank")]
	public class LevelObjectBank : ScriptableObject {
		
		public LevelObject[] levelObjects;
		
		public LevelObject GetLevelObjectWithID(string ID) {
			foreach (LevelObject levelObject in levelObjects) {
				if (levelObject.objectTypeID == ID) {
					return levelObject;
				}
			}

			throw new Exception($"No object with the ID: {ID} found.");
		}
		
	}
}
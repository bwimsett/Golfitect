using UnityEngine;

namespace Backend.Level {
	public class Level {
		public Vector3Int levelDimensions { get; private set; }
		
		public Level(Vector3Int levelDimensions) {
			this.levelDimensions = levelDimensions;
		}
		
	}
}
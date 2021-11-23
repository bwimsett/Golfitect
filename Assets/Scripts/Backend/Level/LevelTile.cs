using UnityEngine;

namespace Backend.Level {
	public class LevelTile {
		
		private LevelObject tileObject;
		private Vector3Int coordinate;

		public LevelTile(Vector3Int coordinate) {
			this.coordinate = coordinate;
		}

		public LevelObject GetTileObject() {
			return tileObject;
		}
		
		public void SetTile(LevelObject tileObject) {
			this.tileObject = tileObject;
			tileObject.coordinates.Add(coordinate);
		}

	}
}
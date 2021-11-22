using Backend.Level;
using Game.Tools;
using UnityEngine;

namespace Backend.Managers {
	public class LevelInputManager : MonoBehaviour {
		[SerializeField] private BoxCollider levelGridCollider;
		[SerializeField] private int levelGridColliderLayerID;

		[SerializeField] public LevelBuilderTool levelBuilderTool;

		public LevelObject testLevelObject;
		
		[HideInInspector] public Tool currentActiveTool;

		public void Initialise() {
			levelBuilderTool.SetLevelObject(testLevelObject);
			levelBuilderTool.SetActive(true);
		}

		public Vector3Int GetMouseGridCoordinate() {
			Ray ray = LevelManager.cameraController.camera.ScreenPointToRay(Input.mousePosition);
			int targetLayer = 1 << levelGridColliderLayerID;
			Physics.Raycast(ray, out RaycastHit hit, 500, targetLayer);
        
			if (hit.collider) {
				return LevelManager.levelGrid.WorldPositionToGridCoordinate(hit.point);
			}
        
			return new Vector3Int(-1, -1, -1);
		}

	}
}
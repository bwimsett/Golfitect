using Backend.Level;
using Game.Tools;
using UnityEngine;

namespace Backend.Managers {
	public class LevelInputManager : MonoBehaviour {
		[SerializeField] private BoxCollider levelGridCollider;
		[SerializeField] private int levelGridColliderLayerID, levelSurfaceColliderLayerID;

		[SerializeField] public LevelBuilderTool levelBuilderTool;

		[HideInInspector] public Tool currentActiveTool;

		public bool GetMouseWorldPosition(out Vector3 vector3) {
			int targetLayer = 1 << levelGridColliderLayerID;
			bool hit = CastRayFromMouse(targetLayer, out Vector3 hitPos);
			
			if (hit) {
				vector3 = hitPos;
				return true;
			}

			vector3 = Vector3Int.zero;
			return false;
		}
		
		public bool GetMouseGridCoordinate(out Vector3Int vector3) {
			bool success = GetMouseWorldPosition(out Vector3 worldPos);

			if (success) {
				vector3 = LevelManager.levelGrid.WorldPositionToGridCoordinate(worldPos);
				return true;
			}

			vector3 = Vector3Int.zero;
			return false;
		}

		public bool GetMouseLevelSurfacePosition(out Vector3 vector3) {
			int targetLayer = 1 << levelSurfaceColliderLayerID;
			bool hit = CastRayFromMouse(targetLayer, out Vector3 hitPos);
			vector3 = hitPos;
			return hit;
		}

		public bool GetLevelSurfaceAtGridCoordinate(Vector3Int gridCoordinate, out Vector3 vector3) {
			int targetLayer = 1 << levelSurfaceColliderLayerID;
			float maxY = LevelManager.levelGrid.GridCoordinateToWorldPosition(new Vector3Int(gridCoordinate.x, gridCoordinate.y + 1, gridCoordinate.z)).y;
			Vector3 minPos = LevelManager.levelGrid.GridCoordinateToWorldPosition(gridCoordinate);
			Vector3 maxPos = new Vector3(minPos.x, maxY, minPos.z); // Cast downwards from the top of the grid position
			Ray ray = new Ray(maxPos, minPos);
			return CastRay(ray, targetLayer, out vector3);
		}

		private bool CastRay(Ray ray, int layerMask, out Vector3 vector3) {
			Physics.Raycast(ray, out RaycastHit hit, 500, layerMask);

			if (hit.collider) {
				vector3 = hit.point;
				return true;
			}

			vector3 = Vector3.zero;
			return false;
		}
		
		private bool CastRayFromMouse(int layerMask, out Vector3 vector3) {
			Ray ray = LevelManager.cameraController.camera.ScreenPointToRay(Input.mousePosition);
			bool success = CastRay(ray, layerMask, out vector3);
			return success;
		}
	}
}
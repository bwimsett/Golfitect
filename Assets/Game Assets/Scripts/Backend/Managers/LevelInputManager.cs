using Backend.Level;
using Game.Tools;
using UnityEngine;

namespace Backend.Managers {
	public class LevelInputManager : MonoBehaviour {
		[SerializeField] private BoxCollider levelGridCollider;
		public int levelGridColliderLayerID, levelSurfaceColliderLayerID, ballColliderLayerID;

		[SerializeField] public LevelBuilderTool levelBuilderTool;
		[SerializeField] public BallControllerTool ballControllerTool;

		[HideInInspector] public Tool currentActiveTool;

		public Vector3 GetMouseWorldPosition() {
			return LevelManager.cameraController.camera.ScreenToWorldPoint(Input.mousePosition);
		}

		public bool GetMouseLevelGridPosition(out Vector3 vector3) {
			int targetLayer = 1 << levelGridColliderLayerID;
			bool success = CastRayFromMouse(targetLayer, out RaycastHit hit);
			
			if (success) {
				vector3 = hit.point;
				return true;
			}

			vector3 = Vector3Int.zero;
			return false;
		}
		
		public bool GetMouseGridCoordinate(out Vector3Int vector3) {
			bool success = GetMouseLevelGridPosition(out Vector3 worldPos);

			if (success) {
				vector3 = LevelManager.levelGrid.WorldPositionToGridCoordinate(worldPos);
				return true;
			}

			vector3 = Vector3Int.zero;
			return false;
		}

		public bool GetMouseLevelSurfacePosition(out RaycastHit hit) {
			int targetLayer = 1 << levelSurfaceColliderLayerID;
			bool success = CastRayFromMouse(targetLayer, out hit);
			return success;
		}

		public bool GetLevelSurfaceAtGridCoordinate(Vector3Int gridCoordinate, out Vector3 vector3) {
			int targetLayer = 1 << levelSurfaceColliderLayerID;
			float maxY = LevelManager.levelGrid.GridCoordinateToWorldPosition(new Vector3Int(gridCoordinate.x, gridCoordinate.y + 1, gridCoordinate.z)).y;
			Vector3 minPos = LevelManager.levelGrid.GridCoordinateToWorldPosition(gridCoordinate);
			Vector3 maxPos = new Vector3(minPos.x, maxY, minPos.z); // Cast downwards from the top of the grid position
			Ray ray = new Ray(maxPos, minPos);
			bool success = CastRay(ray, targetLayer, out RaycastHit hit);
			vector3 = hit.point;
			return success;
		}

		private bool CastRay(Ray ray, int layerMask, out RaycastHit hit) {
			Physics.Raycast(ray, out hit, 500, layerMask);

			if (hit.collider) {
				return true;
			}
			
			return false;
		}
		
		private bool CastRay(Ray ray, string tag, out RaycastHit hit) {
			Physics.Raycast(ray, out hit, 500);

			if (hit.collider) {
				return hit.collider.gameObject.tag.Equals(tag);
			}
			
			return false;
		}

		public bool CastRayFromMouse(int layerMask, out RaycastHit hit) {
			Ray ray = LevelManager.cameraController.camera.ScreenPointToRay(Input.mousePosition);
			bool success = CastRay(ray, layerMask, out hit);
			return success;
		}

		public bool CastRayFromMouse(string tag, out RaycastHit hit) {
			Ray ray = LevelManager.cameraController.camera.ScreenPointToRay(Input.mousePosition);
			bool success = CastRay(ray, tag, out hit);
			return success;
		}
		
		
	}
}
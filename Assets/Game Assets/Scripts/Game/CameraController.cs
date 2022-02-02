using System;
using System.Collections.Generic;
using Backend.Managers;
using DG.Tweening.Plugins;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {
	public class CameraController : MonoBehaviour {
		
		public Camera camera { get; private set; }

		private bool mouseDragDown, mouseRotateDown;
		private Vector2 mouseDragStart, mouseRotateStart;
		private Vector3 rotationStart, rotationOrigin;

		[SerializeField] private float xRotationLowerBound, xRotationUpperBound;
		[SerializeField] private float zoomLowerBound, zoomUpperBound;
		private float zoomStep;
		
		public float dragSensitivity, rotateSensitivity, zoomSensitivity;
		private bool enableDrag = true;
		private List<object> dragDisablers;

		void Update() {
			UpdateDrag();
			UpdateRotate();
			UpdateZoom();
		}

		private void UpdateDrag() {
			// Don't control camera if drag has been disabled
			if (!enableDrag) {
				mouseDragDown = false;
				return;
			}

			if (Input.GetMouseButtonUp(0)) {
				mouseDragDown = false;
			}
			
			if (Input.GetMouseButtonDown(0)) {
				mouseDragStart = Input.mousePosition;
			}
			
			Vector2 drag = Vector2.zero;
			
			if (Input.GetMouseButton(0)) {
				mouseDragDown = true;
				Vector2 mousePos = Input.mousePosition;
				drag = mouseDragStart - mousePos;
				drag *= dragSensitivity;
				mouseDragStart = mousePos;
			}
			
			//Debug.Log("Drag distance: "+drag.magnitude);
			transform.Translate(drag);
		}
		
		public void DisableDrag(object obj){
			dragDisablers ??= new List<object>();
			
			if (!dragDisablers.Contains(obj)) {
				dragDisablers.Add(obj);
			}

			enableDrag = false;
		}

		public void EnableDrag(object obj) {
			dragDisablers.Remove(obj);

			// Only reenable drag when nothing is left to prevent the drag
			if (dragDisablers.Count == 0) {
				enableDrag = true;
			}

			mouseDragStart = Input.mousePosition;
		}

		private void UpdateRotate() {
			if (!enableDrag || mouseDragDown) {
				mouseRotateDown = false;
				return;
			}

			if (!Input.GetMouseButton(1)) {
				mouseRotateDown = false;
				return;
			}

			if (!mouseRotateDown) {
				mouseRotateDown = true;
				mouseRotateStart = Input.mousePosition;
				rotationStart = transform.rotation.eulerAngles;
				int targetLayer = 1 << LevelManager.levelInputManager.levelSurfaceColliderLayerID;
				Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
				LevelManager.levelInputManager.CastRay(ray, targetLayer, out RaycastHit hit);
				if (hit.collider) {
					rotationOrigin = hit.point;
				}
			}

			Vector2 mousePos = Input.mousePosition;
			Vector2 drag = mouseRotateStart - mousePos;
			mouseRotateStart = mousePos;
			transform.RotateAround(rotationOrigin, Vector3.up, -drag.x * rotateSensitivity);

			Vector3 rotation = transform.rotation.eulerAngles;

			if ((rotation.x <= xRotationLowerBound + 0.5f && drag.y < 0) ||
			    (rotation.x >= xRotationUpperBound - 0.5f && drag.y > 0)) {
				return;
			}

			Vector3 direction = rotationOrigin - camera.transform.position;
			Vector3 verticalRotationAxis = Vector3.Cross(direction, Vector3.up).normalized;
			transform.RotateAround(rotationOrigin, verticalRotationAxis, -drag.y * rotateSensitivity);

			rotation = transform.rotation.eulerAngles;

			// Clamp
			float xRotation = Mathf.Max(xRotationLowerBound, Mathf.Min(xRotationUpperBound, rotation.x));
			transform.rotation = Quaternion.Euler(xRotation, rotation.y, rotation.z);
		}

		private void UpdateZoom() {
			float scrollDelta = -Input.mouseScrollDelta.y;

			/*if (scrollDelta <= 0.05f) {
				return;
			}*/
			
			float sizeDelta = scrollDelta * zoomSensitivity;

			float cameraSize = camera.orthographicSize;
			float newSize = cameraSize + sizeDelta;

			newSize = Mathf.Min(zoomUpperBound, Mathf.Max(zoomLowerBound, newSize));

			camera.orthographicSize = newSize;
		}
		
		public void Initialise() {
			this.camera = GetComponent<Camera>();
		}

	}
}
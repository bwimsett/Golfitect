using System.Collections.Generic;
using Backend.Managers;
using DG.Tweening.Plugins;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {
	public class CameraController : MonoBehaviour {
		
		public Camera camera { get; private set; }

		private bool mouseDown;
		private Vector2 mouseStart;
		public float dragSensitivity;
		private bool enableDrag = true;
		private List<object> dragDisablers;
		
		void Update() {
			UpdateDrag();
		}

		private void UpdateDrag() {
			// Don't control camera if drag has been disabled
			if (!enableDrag) {
				return;
			}
			
			if (Input.GetMouseButtonDown(0)) {
				mouseStart = Input.mousePosition;
			}
			
			Vector2 drag = Vector2.zero;
			
			if (Input.GetMouseButton(0)) {
				Vector2 mousePos = Input.mousePosition;
				drag = mouseStart - mousePos;
				drag *= dragSensitivity;
				mouseStart = mousePos;
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

			mouseStart = Input.mousePosition;
		}
		
		public void Initialise() {
			this.camera = GetComponent<Camera>();
		}

	}
}
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Tools {
	public class Tool : MonoBehaviour {

		private bool active;
		protected Vector3Int mousePosition;

		void Update() {
			if (!active) {
				return;
			}

			HandleInput();
		}

		private void HandleInput() {
			mousePosition = LevelManager.levelInputManager.GetMouseGridCoordinate();
			
			if (Input.GetMouseButtonDown(0)) {
				OnMouseDown();
				return;
			}

			if (Input.GetMouseButton(0)) {
				MouseDown();
				return;
			}

			if (Input.GetMouseButtonUp(0)) {
				OnMouseUp();
				return;
			}
			
			OnMouseHover();
		}

		public void SetActive(bool active) {
			bool stateChanged = active != this.active;

			if (!stateChanged) {
				return;
			}
			
			this.active = active;
			Tool currentActiveTool = LevelManager.levelInputManager.currentActiveTool;
			
			if (active) {
				if (currentActiveTool != this) {
					if (currentActiveTool) {
						currentActiveTool.SetActive(false);
					}
					LevelManager.levelInputManager.currentActiveTool = this;
				}
				
				Activate();
				return;
			}
			
			if (currentActiveTool == this) {
				LevelManager.levelInputManager.currentActiveTool = null;
			}
			
			Deactivate();
		}

		protected virtual void Activate() {

		}

		protected virtual void Deactivate() {

		}

		protected virtual void OnMouseHover() {
			
		}
		
		protected virtual void OnMouseDown() {
			
		}

		protected virtual void MouseDown() {
			
		}
		
		protected virtual void OnMouseUp() {
			
		}
	}
}
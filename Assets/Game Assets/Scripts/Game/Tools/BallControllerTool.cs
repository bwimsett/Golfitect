using System;
using UnityEngine;

namespace Game.Tools {
	public class BallControllerTool : Tool {

		public GameObject ballPrefab;
		private Ball ball;
		private bool swingInitiated;

		private Vector3 mouseStart, mouseEnd;
		
		protected override void ToolUpdate() {
			bool mouseOverBall = LevelManager.levelInputManager.CastRayFromMouse(ballPrefab.tag, out RaycastHit hit);
			if (mouseOverBall) {
				Debug.Log("Mouse over ball");
				ball = hit.collider.GetComponent<Ball>();
			} else if (!swingInitiated) {
				ball = null;
			}
		}

		private void OnDrawGizmos() {
			if (swingInitiated) {
				Gizmos.color = Color.red;
				ProjectMouseToBallPlane(out mouseEnd);
				Gizmos.DrawSphere(mouseEnd, 0.2f);
			}
		}

		protected override void OnMouseDown() {
			if (ball) {
				swingInitiated = true;
				mouseStart = ball.transform.position;
			}
		}

		protected override void OnMouseUp() {
			if (!swingInitiated) {
				return;
			}
			
			bool hitBallPlane = ProjectMouseToBallPlane(out Vector3 mousePos);

			if (hitBallPlane) {
				mouseEnd = mousePos;
			}
			
			Debug.Log("swing, length: "+(mouseEnd-mouseStart).magnitude);
			swingInitiated = false;
			ball.Swing(mouseStart-mouseEnd);
		}

		protected override void OnRightMouseDown() {
			swingInitiated = false;
		}

		private bool ProjectMouseToBallPlane(out Vector3 point) {
			LevelManager.levelInputManager.CastRayFromMouse(ball.mouseDragPlane.tag, out RaycastHit hit);

			if (hit.collider) {
				point = hit.point;
				return true;
			}

			point = Vector3.zero;
			return false;
		}
	}
}
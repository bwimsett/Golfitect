using System;
using UnityEngine;

namespace Game.Tools {
	public class BallControllerTool : Tool {

		public Ball ballPrefab;
		private Ball ball;
		private bool swingInitiated;

		private Vector3 mouseStart, mouseEnd;
		
		protected override void ToolUpdate() {
			bool mouseOverBall = LevelManager.levelInputManager.CastRayFromMouse(ballPrefab.ballHandle.tag, out RaycastHit hit);
			if (mouseOverBall) {
				Ball newBall = hit.collider.GetComponent<BallHandle>().ball;

				if (!newBall.IsMoving()) {
					if (ball != newBall) {
						ball = newBall;
					}
					ball.MouseOver();
				}
				else {
					ball = null;
				}
			} else if (!swingInitiated) {
				if (ball) {
					ball.MouseExit();
				}
				
				ball = null;
			}

			RefreshMousePos();

			if (ball) {
				if (swingInitiated) {
					ball.SetSwingDirection(mouseEnd);
				} else {
					ball.SetSwingDirection(ball.transform.position);
				}
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
				LevelManager.cameraController.DisableDrag(this);
				swingInitiated = true;
				mouseStart = ball.transform.position;
			}
		}

		private void RefreshMousePos() {
			bool hitBallPlane = ProjectMouseToBallPlane(out Vector3 mousePos);

			if (hitBallPlane) {
				mouseEnd = mousePos;
			}
		}

		protected override void OnMouseUp() {
			if (!swingInitiated) {
				return;
			}
			
			//Debug.Log("swing, length: "+(mouseEnd-mouseStart).magnitude);
			swingInitiated = false;
			ball.Swing(mouseEnd);
			LevelManager.cameraController.EnableDrag(this);
		}

		protected override void OnRightMouseDown() {
			swingInitiated = false;
			if (ball) {
				LevelManager.cameraController.EnableDrag(this);
			}
		}

		private bool ProjectMouseToBallPlane(out Vector3 point) {
			point = Vector3.zero;
			
			if (!ball) {
				return false;
			}
			
			LevelManager.levelInputManager.CastRayFromMouse(ball.mouseDragPlane.tag, out RaycastHit hit);

			if (hit.collider) {
				point = hit.point;
				return true;
			}
			
			return false;
		}
	}
}
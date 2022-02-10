using Backend.Managers;
using DG.Tweening;
using Shapes;
using UnityEngine;

namespace Game {
	public class Ball : MonoBehaviour {

		[SerializeField] private Rigidbody rigidbody;
		public GameObject mouseDragPlane;
		public BallHandle ballHandle;

		public float maxCircleRadius, maxCircleThickness, circleAppearDuration;
		private float circleRadius, circleThickness;
		private Tween circleRadiusTween, circleThicknessTween;
		public float maxMouseDistance, maxDirectionLineLength;

		private Vector3 mouseSwingPos, swingStart, lineEnd;
		private float swingStrength;
		public float maxForce;

		public float dashSize, dashSpacing, dashOffset;

		private Vector3 lastPosition;

		private bool mouseOver;

		void Update() {
			ResetIfOutOfBounds();
		}

		public bool IsMoving() {
			return !rigidbody.IsSleeping();
		}

		public void Swing(Vector3 mousePos) {
			//SetSwingDirection(mousePos);

			lastPosition = transform.position;

			Vector3 swingDirection = Vector3.Normalize(transform.position - mouseSwingPos);
			Vector3 swing = swingDirection * swingStrength * maxForce;

			// Ignore swings with extremely low magnitude
			if (swing.magnitude < 0.05f) {
				return;
			}
			
			mouseSwingPos = transform.position;
			
			rigidbody.AddForce(swing);
			if (GameManager.courseTracker != null) {
				GameManager.courseTracker.AddShot();
			}
		}

		private void ResetIfOutOfBounds() {
			if (transform.position.y > GameManager.currentLevel.levelBoundaryY) {
				return;
			}

			transform.position = lastPosition;
			rigidbody.velocity = Vector3.zero;
		}
		
		public void MouseOver() {
			float duration = circleAppearDuration;
			mouseOver = true;
			
			if (circleRadiusTween != null) {
				duration -= circleRadiusTween.Elapsed();
				circleRadiusTween.Kill();
				circleThicknessTween?.Kill();
			}
			
			circleRadiusTween = DOTween.To(() => circleRadius, value => circleRadius = value, maxCircleRadius, duration);
			circleThicknessTween = DOTween.To(() => circleThickness, value => circleThickness = value, maxCircleThickness, duration);
		}

		public void MouseExit() {
			float duration = circleAppearDuration;
			mouseOver = false;
			
			if (circleRadiusTween != null) {
				duration -= circleRadiusTween.Elapsed();
				circleRadiusTween.Kill();
				circleThicknessTween?.Kill();
			}
			
			circleRadiusTween = DOTween.To(() => circleRadius, value => circleRadius = value, 0, duration);
			circleThicknessTween = DOTween.To(() => circleThickness, value => circleThickness = value, 0, duration);
		}

		public void SetSwingDirection(Vector3 mousePos) {
			mouseSwingPos = mousePos;
			
			Vector3 pos = transform.position;
			
			Vector3 directionVector = pos - mouseSwingPos;
			lineEnd = pos + directionVector;
			lineEnd.y = pos.y;
			swingStart = Vector3.MoveTowards(pos, lineEnd, circleRadius+circleThickness/2f);
			swingStart.y = pos.y;
			
			float lineLength = (lineEnd - swingStart).magnitude;
			swingStrength  = Mathf.Min(1,lineLength / maxMouseDistance);
		}

		public void Destroy() {
			Destroy(gameObject);
		}
		
		public void DrawHandle() {
			Draw.Thickness = circleThickness;
			Draw.LineEndCaps = LineEndCap.Round;
			Draw.UseDashes = false;
			
			Vector3 lineEnd = Vector3.MoveTowards(swingStart, this.lineEnd, (swingStrength*maxDirectionLineLength));
			Draw.Ring(transform.position, Vector3.up, circleRadius, circleThickness);
			
			Draw.DashSize = dashSize;
			Draw.UseDashes = true;
			Draw.DashSpacing = dashSpacing;
			Draw.DashOffset = dashOffset;

			if (mouseOver) {
				Draw.Line(swingStart, lineEnd);
			}
			
		}

	}
}
using DG.Tweening;
using UnityEngine;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class ShiftXOnMouseover : MonoBehaviour{ 
		public RectTransform shiftContainer;
		public float mouseOverMoveDistance, mouseOverMoveDuration;
		private float moveStartX;
		private Tween moveTween;
		
		void Awake() {
			moveStartX = shiftContainer.anchoredPosition.x;
		}
		
		public void OnMouseOver() {
			float animationDuration = mouseOverMoveDuration;
			
			if (moveTween != null) {
				animationDuration -= moveTween.Elapsed();
				moveTween.Kill();
			}

			moveTween = shiftContainer.DOAnchorPosX(moveStartX+mouseOverMoveDistance, animationDuration);
		}

		public void OnMouseExit() {
			float animationDuration = mouseOverMoveDuration;
			
			if (moveTween != null) {
				animationDuration -= moveTween.Elapsed();
				moveTween.Kill();
			}

			moveTween = shiftContainer.DOAnchorPosX(moveStartX, animationDuration);
		}
	}
}
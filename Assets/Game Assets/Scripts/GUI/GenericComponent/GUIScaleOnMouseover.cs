using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.GenericComponent {
	public class GUIScaleOnMouseover : MonoBehaviour {
		[SerializeField] private RectTransform scaleTarget;
		[SerializeField] private Image colorTarget;
		
		[SerializeField] private float mouseOverTime, mouseExitTime, mouseClickTime;
		[SerializeField] private Ease mouseOverEase, mouseExitEase, mouseClickEase;
		[SerializeField] private Vector2 mouseOverSize, mouseClickSize;
		[SerializeField] private Color mouseOverColor, mouseExitColor, mouseClickColor;

		[SerializeField] private UnityEvent OnClick, OnSelected, OnDeselected;
		
		private Vector2 defaultSize;
		private Color defaultColor;
		
		private Tween scaleTween, colorTween;
		
		[SerializeField] private bool isToggle;
		[ShowIf("isToggle"), SerializeField] private bool lockScale, lockColor; 
		protected bool isSelected;

		void Awake() {
			defaultSize = scaleTarget.rect.size;
			defaultColor = colorTarget.color;
		}
		
		public void OnMouseEnter() {
			if (!(isToggle && lockScale && isSelected)) {
				scaleTween?.Kill();
				scaleTween = scaleTarget.DOSizeDelta(mouseOverSize, mouseOverTime).SetEase(mouseOverEase);
			}

			if (!(isToggle && lockColor && isSelected)) {
				colorTween?.Kill();
				colorTween = colorTarget.DOColor(mouseOverColor, mouseOverTime).SetEase(mouseOverEase);
			}
		}

		public void OnMouseExit() {
			if (!(isToggle && lockScale && isSelected)) {
				scaleTween?.Kill();
				scaleTween = scaleTarget.DOSizeDelta(defaultSize, mouseExitTime).SetEase(mouseExitEase);
			}

			if (!(isToggle && lockColor && isSelected)) {
				colorTween?.Kill();
				colorTween = colorTarget.DOColor(defaultColor, mouseExitTime).SetEase(mouseExitEase);
			}
		}

		public void OnMouseDown() {
			scaleTween?.Kill();
			colorTween?.Kill();
			scaleTween = scaleTarget.DOSizeDelta(mouseClickSize, mouseClickTime).SetEase(mouseClickEase);
			colorTween = colorTarget.DOColor(mouseClickColor, mouseClickTime).SetEase(mouseClickEase);
		}

		public void Deselect() {
			isSelected = false;
			OnMouseExit();
		}
		
		public void OnMouseClick() {
			isSelected = !isSelected;

			if (isSelected && isToggle) {
				OnSelected?.Invoke();
			} else if (!isSelected && isToggle) {
				OnDeselected?.Invoke();
			} else {
				OnClick?.Invoke();
			}
			
			OnMouseEnter();
		}
		
	}
}
using System;
using BWLocaliser;
using DG.Tweening;
using UnityEngine;

namespace Game_Assets.Scripts.GUI {
	public class AutoResizeSwitchButton : MonoBehaviour {
		
		[SerializeField] private float animationDuration;
		[SerializeField] private Ease animationEase;
		[SerializeField] private float textSidePadding;
		[SerializeField] private TextLocalizer offTextLocaliser, onTextLocaliser;

		private RectTransform rectTransform;
		private bool state;
		private Tween currentShapeTween, currentOnTextTween, currentOffTextTween;

		public Action<bool> OnClickAction;

		void Awake() {
			onTextLocaliser.RefreshString();
			offTextLocaliser.RefreshString();
		}
		
		void Start() {
			SetState(state, true);
		}
		
		public void OnClick() {
			SetState(!state, true);
			OnClickAction.Invoke(state);
		}

		public void SetState(bool state, bool animate) {
			if (rectTransform == null) {
				rectTransform = GetComponent<RectTransform>();
			}
			
			if (state == this.state) {
				return;
			}

			this.state = state;

			if (animate) {
				Animate();
				return;
			}

			if (state) {
				onTextLocaliser.text.alpha = 1;
				offTextLocaliser.text.alpha = 0;
				rectTransform.sizeDelta =
					new Vector2(onTextLocaliser.GetComponent<RectTransform>().rect.width + 2 * textSidePadding,
						rectTransform.rect.height);
			}
			else {
				onTextLocaliser.text.alpha = 0;
				offTextLocaliser.text.alpha = 1;
				rectTransform.sizeDelta =
					new Vector2(offTextLocaliser.GetComponent<RectTransform>().rect.width + 2 * textSidePadding,
						rectTransform.rect.height);
			}
		}

		private void AnimateSize(float width) {
			currentShapeTween?.Kill();
			currentShapeTween = rectTransform.DOSizeDelta(new Vector2(width, rectTransform.rect.height), animationDuration).SetEase(animationEase);
		}

		private void Animate() {
			currentOnTextTween?.Kill();
			currentOffTextTween?.Kill();

			float newWidth, onTextFade, offTextFade;
			string newTextId = "";

			if (state) {
				newWidth = onTextLocaliser.GetComponent<RectTransform>().rect.width;
				onTextFade = 1;
				offTextFade = 0;
			} else {
				newWidth = offTextLocaliser.GetComponent<RectTransform>().rect.width;
				onTextFade = 0;
				offTextFade = 1;
			}

			AnimateSize(newWidth+textSidePadding*2);

			currentOnTextTween = onTextLocaliser.text.DOFade(onTextFade, animationDuration);
			currentOffTextTween = offTextLocaliser.text.DOFade(offTextFade, animationDuration);
		}
	}
	
}
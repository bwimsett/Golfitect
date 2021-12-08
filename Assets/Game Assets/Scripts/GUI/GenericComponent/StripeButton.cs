

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StripeButton : MonoBehaviour {

	public Image background;
	[SerializeField] private RectTransform resizeTarget;
	[SerializeField] private Vector2 backgroundStartX, backgroundEndX;
	[SerializeField] private float backgroundAnimationSpeed, scaleSpeed, shrinkSpeed;
	[SerializeField] private Ease enlargeEase;
	[SerializeField] private Vector2 enlargeScale;
	private Vector2 normalSize;
	private bool animateBackground;
	private Tween currentBackgroundAnimation, currentBackgroundFade, currentEnlargeAnimation, currentShrinkAnimation;

	void Awake() {
		normalSize = resizeTarget.rect.size;
	}

	private void ShowBackground() {
		animateBackground = true;
		currentBackgroundFade = background.DOFade(1, scaleSpeed);
		AnimateBackground();
	}
	
	private void AnimateBackground() {
		currentBackgroundAnimation?.Kill();
		
		if (!animateBackground) {
			return;
		}

		currentBackgroundAnimation = background.transform.DOLocalMove(backgroundEndX, backgroundAnimationSpeed).OnComplete(
			() => {
				background.transform.localPosition = backgroundStartX;
				AnimateBackground();
			}).SetEase(Ease.Linear);
	}

	private void HideBackground() {
		currentBackgroundFade?.Kill();
		currentBackgroundFade = background.DOFade(0, shrinkSpeed).OnComplete(() => {
			animateBackground = false;
			currentBackgroundAnimation?.Kill();
			background.transform.localPosition = backgroundStartX;
		});
	}

	public void OnMouseEnter() {
		ShowBackground();
		currentShrinkAnimation?.Kill();
		Vector2 newSize = enlargeScale * normalSize;
		currentEnlargeAnimation = resizeTarget.DOSizeDelta(newSize, scaleSpeed).SetEase(enlargeEase);
	}

	public void OnMouseDown() {
		
	}
	
	public void OnMouseExit() {
		HideBackground();
		currentEnlargeAnimation?.Kill();
		currentShrinkAnimation = resizeTarget.DOSizeDelta(normalSize, shrinkSpeed);
	}

}

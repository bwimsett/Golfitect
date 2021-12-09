

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StripeButton : MonoBehaviour {

	public Image background, stripes;
	[SerializeField] private RectTransform resizeTarget;
	[SerializeField] private Vector2 backgroundStartX, backgroundEndX;
	[SerializeField] private float backgroundAnimationSpeed, scaleSpeed, shrinkSpeed;
	[SerializeField] private Ease enlargeEase;
	[SerializeField] private Vector2 enlargeScale;
	public TextLocalizer buttonText;
	private Vector2 normalSize;
	private bool animateBackground;
	private Tween currentBackgroundAnimation, currentBackgroundFade, currentEnlargeAnimation, currentShrinkAnimation;

	void Awake() {
		normalSize = resizeTarget.rect.size;
	}

	private void ShowStripes() {
		animateBackground = true;
		currentBackgroundFade = stripes.DOFade(1, scaleSpeed);
		AnimateStripes();
	}
	
	private void AnimateStripes() {
		currentBackgroundAnimation?.Kill();
		
		if (!animateBackground) {
			return;
		}

		currentBackgroundAnimation = stripes.transform.DOLocalMove(backgroundEndX, backgroundAnimationSpeed).OnComplete(
			() => {
				stripes.transform.localPosition = backgroundStartX;
				AnimateStripes();
			}).SetEase(Ease.Linear);
	}

	private void HideStripes() {
		currentBackgroundFade?.Kill();
		currentBackgroundFade = stripes.DOFade(0, shrinkSpeed).OnComplete(() => {
			animateBackground = false;
			currentBackgroundAnimation?.Kill();
			stripes.transform.localPosition = backgroundStartX;
		});
	}

	public void OnMouseEnter() {
		ShowStripes();
		currentShrinkAnimation?.Kill();
		Vector2 newSize = enlargeScale * normalSize;
		currentEnlargeAnimation = resizeTarget.DOSizeDelta(newSize, scaleSpeed).SetEase(enlargeEase);
	}

	public void OnMouseDown() {
		
	}
	
	public void OnMouseExit() {
		HideStripes();
		currentEnlargeAnimation?.Kill();
		currentShrinkAnimation = resizeTarget.DOSizeDelta(normalSize, shrinkSpeed);
	}

}

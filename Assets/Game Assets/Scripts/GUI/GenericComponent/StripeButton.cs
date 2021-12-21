

using Backend.Managers;
using DG.Tweening;
using GUI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

public class StripeButton : MonoBehaviour {

	public Image background, stripes;
	public string backgroundColorID, textColorID, textChangeColorID;
	[SerializeField] private RectTransform resizeTarget;
	[SerializeField] private Vector2 backgroundStartX, backgroundEndX;
	[SerializeField] private float backgroundAnimationSpeed, scaleSpeed, shrinkSpeed;
	[SerializeField] private Ease enlargeEase;
	[SerializeField] private Vector2 enlargeScale;
	public TextLocalizer buttonText;
	public Image buttonImage;
	private Vector2 normalSize;
	private bool animateBackground;
	private Tween currentBackgroundAnimation, currentBackgroundFade, currentEnlargeAnimation, currentShrinkAnimation, currentTextTween;

	void Start() {
		normalSize = resizeTarget.rect.size;
		if (!string.IsNullOrEmpty(backgroundColorID)) {
			Color targetColor = GameSceneManager.colorBank.GetColor(backgroundColorID);
			targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0);
			stripes.color = targetColor;
		}
		
		if (!string.IsNullOrEmpty(textColorID)) {
			if (buttonText) {
				buttonText.text.color = GameSceneManager.colorBank.GetColor(textColorID);
			}

			if (buttonImage) {
				buttonImage.color = GameSceneManager.colorBank.GetColor(textColorID);
			}
		}
		
	}

	private void ShowStripes() {
		currentTextTween?.Kill();
		animateBackground = true;
		currentBackgroundFade = stripes.DOFade(1, scaleSpeed);
		if (buttonText && !string.IsNullOrEmpty(textChangeColorID)) {
			currentTextTween = buttonText.text.DOColor(GameSceneManager.colorBank.GetColor(textChangeColorID), scaleSpeed);
		}
		if (buttonImage && !string.IsNullOrEmpty(textChangeColorID)) {
			currentTextTween = buttonImage.DOColor(GameSceneManager.colorBank.GetColor(textChangeColorID), scaleSpeed);
		}
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
		currentTextTween?.Kill();
		currentBackgroundFade?.Kill();
		currentBackgroundFade = stripes.DOFade(0, shrinkSpeed).OnComplete(() => {
			animateBackground = false;
			currentBackgroundAnimation?.Kill();
			stripes.transform.localPosition = backgroundStartX;
		});
		
		if (buttonText && !string.IsNullOrEmpty(textColorID)) {
			currentTextTween = buttonText.text.DOColor(GameSceneManager.colorBank.GetColor(textColorID), scaleSpeed);
		}
		if (buttonImage && !string.IsNullOrEmpty(textColorID)) {
			currentTextTween = buttonImage.DOColor(GameSceneManager.colorBank.GetColor(textColorID), scaleSpeed);
		}
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

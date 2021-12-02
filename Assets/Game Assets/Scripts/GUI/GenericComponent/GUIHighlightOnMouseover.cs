using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MPUIKIT;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIHighlightOnMouseover : MonoBehaviour {

	private Image image;
	private float defaultAlpha;
	private float fadeDuration = 0.2f;
	private Tween currentTween;

	private bool locked;
	
	void Awake() {
		image = GetComponent<Image>();
		defaultAlpha = image.color.a;
	}

	public void OnMouseEnter() {
		Fade(1);
	}

	public void OnMouseExit() {
		if (locked) {
			return;
		}

		Fade(defaultAlpha);
	}

	private void Fade(float alpha) {
		currentTween?.Kill();
		currentTween = image.DOFade(alpha, fadeDuration);
	}

	public void Lock() {
		locked = true;
	}

	public void Unlock() {
		locked = false;
		Fade(defaultAlpha);
	}


}

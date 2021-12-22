using System.Collections;
using System.Collections.Generic;
using Backend.Managers;
using DG.Tweening;
using Game_Assets.Scripts.Backend.Server;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerProfileDisplay : MonoBehaviour {

	[SerializeField] private int screenEdgePadding, originPadding;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeDuration;

	[Header("Fields")] public TextMeshProUGUI usernameText;
	public TextMeshProUGUI goldTrophiesText, silverTrophiesText, bronzeTrophiesText;
	public TextMeshProUGUI playCountText;
	public TextLocalizer likesReceivedText, coursesBuiltText;
	
	private DBUser user;
	private RectTransform rectTransform, origin;
	private Tween fadeTween;
	
	void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void SetUser(DBUser user) {
		this.user = user;
	}

	public void SetOrigin(Transform origin) {
		this.origin = origin.GetComponent<RectTransform>();
		RefreshPosition();
	}

	private void RefreshPosition() {
		// Position based on anchors set to: (0,0)
		Vector2 pos = rectTransform.anchoredPosition;
		Vector2 dimensions = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
		Vector2 screenArea = new Vector2(Screen.width, Screen.height);

		float maxY = screenArea.y - dimensions.y - screenEdgePadding;
		float minY = screenEdgePadding;
		float maxX = screenArea.x - dimensions.x - screenEdgePadding;
		float minX = screenEdgePadding;

		Vector2 originPos = origin.transform.position;
		rectTransform.position = originPos;
		
		Vector2 anchoredPos = rectTransform.anchoredPosition;
		
		// Shift to below the origin by originPadding and the height of the profile box
		rectTransform.anchoredPosition = new Vector2(anchoredPos.x - dimensions.x/2, anchoredPos.y - dimensions.y - originPadding);
			
		// Adjust based on min / max
		// If clipping below the minimum for the screen, shift to above the origin by origin padding
		if (anchoredPos.y < minY) {
			rectTransform.position = new Vector3(rectTransform.position.x, originPos.y);
			anchoredPos = rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(anchoredPos.x, anchoredPos.y + originPadding);
		}

		anchoredPos = rectTransform.anchoredPosition;
		
		// Now set the min, max
		rectTransform.anchoredPosition = new Vector2(Mathf.Max(minY, Mathf.Min(maxX, anchoredPos.x)), Mathf.Max(minX, Mathf.Min(maxY, anchoredPos.y)));
	}

	private void Refresh() {
		usernameText.text = user.username;
		
		goldTrophiesText.text = "" + user.goldtrophies;
		silverTrophiesText.text = "" + user.silvertrophies;
		bronzeTrophiesText.text = "" + user.bronzetrophies;

		playCountText.text = "" + user.playcount;
		
		likesReceivedText.SetFields(new Dictionary<string, object>(){{"likes", user.likes}});
		
		coursesBuiltText.SetFields(new Dictionary<string, object>(){{"courses", user.coursesbuilt}});
	}

	public void Open() {
		Refresh();
		fadeTween?.Kill();
		fadeTween = canvasGroup.DOFade(1, fadeDuration);
	}

	public void Close() {
		fadeTween?.Kill();
		fadeTween = canvasGroup.DOFade(0, fadeDuration);
	}
	
}

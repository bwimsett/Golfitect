using System.Collections.Generic;
using Backend.Enums;
using Backend.Managers;
using DG.Tweening;
using Game_Assets.Scripts.Backend.Server;
using Game_Assets.Scripts.GUI.PlayMode;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.MainMenu.CourseOverview {
	public class MainMenu_CourseOverview : MonoBehaviour {

		public CanvasGroup canvasGroup;
		public RectTransform rectTransform;

		public Vector2 openStartPos;
		public Ease openEase, closeEase;
		public float openDuration, closeDuration;
		private Tween posTween, alphaTween;

		public Image headerBackground;
		public TextMeshProUGUI courseNameText, likesText, playCountText;
		public TextLocalizer creatorNameText, holeCountText;

		public TextMeshProUGUI descriptionText;
		public MPImage screenshotImage;

		public CourseScoreSummary scoreSummary;

		private DBCourseInfo courseInfo;

		public void Awake() {
			headerBackground.color = GameSceneManager.colorBank.GetColor("generic_orange");
			canvasGroup.alpha = 0;
			canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
		}
		
		public void SetCourse(DBCourseInfo courseInfo) {
			this.courseInfo = courseInfo;

			playCountText.text = "" + courseInfo.playcount;
			likesText.text = "" + courseInfo.likes;
			
			courseNameText.text = courseInfo.name;
			creatorNameText.SetFields(new Dictionary<string, object>(){{"username", courseInfo.user.username}});

			holeCountText.SetFields(new Dictionary<string, object>(){{"holecount", courseInfo.holeIDs.Length}, {"par", courseInfo.par}});
			
			descriptionText.text = courseInfo.description;
			
			scoreSummary.SetCourse(courseInfo, true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		}

		public void Open(DBCourseInfo course) {
			SetCourse(course);
			posTween?.Kill();
			alphaTween?.Kill();
			rectTransform.anchoredPosition = openStartPos;
			rectTransform.DOAnchorPos(new Vector2(0, 0), openDuration).SetEase(openEase).OnComplete(() => {
				canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
			}).OnPlay(()=> {
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			});
			
			canvasGroup.DOFade(1, openDuration).SetEase(Ease.Linear);
		}

		public void Close() {
			posTween?.Kill();
			alphaTween?.Kill();
			rectTransform.DOAnchorPos(openStartPos, closeDuration).SetEase(closeEase).OnComplete(() => {
				canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
			});;
			canvasGroup.DOFade(0, openDuration).SetEase(Ease.Linear);
		}

		public void Play() {
			LoadingScreenManager.Load(courseInfo, GameMode.Play);
		}

	}
}
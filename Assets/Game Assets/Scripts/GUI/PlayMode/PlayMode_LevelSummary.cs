using System.Collections.Generic;
using Backend.Course;
using Backend.Level;
using Backend.Managers;
using BWLocaliser;
using DG.Tweening;
using Game_Assets.Scripts.GUI.GenericComponent;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PlayMode_LevelSummary : MonoBehaviour {

		public CanvasGroup canvasGroup;
		public TextLocalizer scoreTitle, courseAndHole, courseScore;
		public ScoreCard scoreCard;
		public Image headerBackground;
		public StripeButton nextButton;

		[Header("Animation Settings")] 
		public float openDuration, closeDuration;
		public Ease openEase;
		public Vector3 startScale;
		
		public void Open() {
			Refresh();
			transform.localScale = startScale;
			canvasGroup.alpha = 0;
			transform.DOScale(Vector3.one, openDuration).SetEase(openEase);
			canvasGroup.DOFade(1, openDuration).OnComplete(() => {
				canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
			});
		}

		public void Refresh() {
			CourseTracker courseTracker = GameManager.courseTracker;

			scoreTitle.SetString(new LocString(LevelUtility.GetScoreNameID(courseTracker.GetScoreForHole(courseTracker.currentHoleIndex-1),
				GameManager.currentLevel.par)));

			courseAndHole.SetFields(new Dictionary<string, object>() {
				{ "coursename", courseTracker.course.title },
				{ "holenumber", courseTracker.currentHoleIndex },
				{ "holecount", courseTracker.course.holes.Length }
			});

			string scoreString = LevelUtility.GetScoreAsString(
					courseTracker.GetCurrentScoreForCourse(courseTracker.currentHoleIndex - 1));
			
			courseScore.SetFields(new Dictionary<string, object>(){{"totalshots", courseTracker.GetTotalShotsForCourse()},{"score", scoreString}});

			nextButton.buttonText.SetFields(new Dictionary<string, object>(){{"holenumber", courseTracker.currentHoleIndex+1}});
			
			scoreCard.Refresh();

			Color backgroundColor = scoreCard.scoreItems[courseTracker.currentHoleIndex-1].color;

			headerBackground.color = nextButton.background.color = backgroundColor;
			nextButton.stripes.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
		}

		public void Close(bool animate = true) {
			canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
			
			if (animate) {
				canvasGroup.DOFade(0, closeDuration);
			} else {
				canvasGroup.alpha = 0;
			}
		}

		public void NextHole() {
			GameManager.currentLevel.Load();
			Close();
		}
		
	}
}
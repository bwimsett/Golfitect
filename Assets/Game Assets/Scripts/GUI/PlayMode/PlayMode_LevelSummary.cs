using System.Collections.Generic;
using Backend.Course;
using Backend.Level;
using Backend.Managers;
using Backend.Serialization;
using BWLocaliser;
using DG.Tweening;
using Game;
using Game_Assets.Scripts.GUI.GenericComponent;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.PlayMode {
	public class PlayMode_LevelSummary : Popup {
		
		public TextLocalizer scoreTitle, courseAndHole, courseScore;
		public ScoreCard scoreCard;
		public TextMeshProUGUI time, highScoreText, worldRecord;
		
		public Image headerBackground;
		public StripeButton nextButton;
		
		public override void Refresh() {
			CourseTracker courseTracker = GameManager.courseTracker;

			scoreTitle.SetString(new LocString(LevelUtility.GetScoreNameID(courseTracker.GetScoreForHole(courseTracker.currentHoleIndex-1),
				courseTracker.GetPreviousLevel().par)));

			courseAndHole.SetFields(new Dictionary<string, object>() {
				{ "holenumber", courseTracker.currentHoleIndex },
				{ "holecount", courseTracker.course.holes.Length }
			});

			string scoreString = LevelUtility.GetScoreAsString(
					courseTracker.GetCurrentScoreForCourse(courseTracker.currentHoleIndex - 1));
			
			courseScore.SetFields(new Dictionary<string, object>(){{"totalshots", courseTracker.GetTotalShotsForCourse()},{"score", scoreString}});

			nextButton.buttonText.SetFields(new Dictionary<string, object>(){{"holenumber", courseTracker.currentHoleIndex+1}});
			
			scoreCard.Refresh();
			
			// Set time scores
			string defaultTimeText = "-:-";
			time.text = highScoreText.text = worldRecord.text = defaultTimeText;

			HoleScore highScore = courseTracker.highScores[courseTracker.currentHoleIndex - 1];
			if (highScore != null) {
				highScoreText.text = "" + LevelTimer.GetTimeString(highScore.time);
			}
			
			time.text = LevelTimer.GetTimeString(courseTracker.GetTimeForHole(courseTracker.currentHoleIndex - 1));

			Color backgroundColor = scoreCard.scoreItems[courseTracker.currentHoleIndex-1].color;

			headerBackground.color = nextButton.background.color = backgroundColor;
			nextButton.stripes.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
		}

		public void NextHole() {
			GameManager.currentLevel.Load();
			Close();
		}
		
	}
}
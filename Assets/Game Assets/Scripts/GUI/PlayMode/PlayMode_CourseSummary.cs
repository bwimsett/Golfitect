using System.Collections.Generic;
using Backend.Course;
using Backend.Managers;
using Game_Assets.Scripts.GUI.GenericComponent;
using Game_Assets.Scripts.GUI.PlayMode;
using TMPro;

public class PlayMode_CourseSummary : Popup {

	public TextMeshProUGUI courseNameText;
	public TextLocalizer creatorText;
	public ScoreCard ScoreCard;
	public CourseScoreSummary scoreSummary;

	public override void Refresh() {
		CourseTracker courseTracker = GameManager.courseTracker;
		
		courseNameText.text = courseTracker.course.name;
		creatorText.SetFields(new Dictionary<string, object>(){{"username", "GET USERNAMES"}});

		ScoreCard.Refresh();
		
		scoreSummary.SetScores(courseTracker.DBCourseScore, courseTracker.DBCourseHighScore);
		scoreSummary.SetCourse(courseTracker.course.courseInfo);
		scoreSummary.Refresh();
		
		scoreSummary.SetLeaderboards(courseTracker.timeLeaderboard, courseTracker.scoreLeaderboard);
	}

	public void Like() {
		GameSceneManager.serverManager.SubmitLike(GameManager.courseTracker.course.courseInfo, true, null);
	}

	public void Dislike() {
		GameSceneManager.serverManager.SubmitLike(GameManager.courseTracker.course.courseInfo, false, null);
	}

}

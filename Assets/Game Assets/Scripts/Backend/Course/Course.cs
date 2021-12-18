using System;
using System.Collections.Generic;
using Backend.Level;
using Backend.Managers;
using Backend.Serialization;
using Game_Assets.Scripts.Backend.Server;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Course {
	[Serializable]
	public class Course : ServerSerializable {
		
		public string[] holeIDs;
		[JsonIgnore] public DBCourseInfo courseInfo;

		[JsonIgnore] public Level.Level[] holes;
		public string name, description;

		[JsonConstructor]
		public Course(string name, string description, string[] holeIDs) {
			this.name = name;
			this.description = description;
			this.holeIDs = holeIDs;
		}
		
		public Course(string name, string description, DBHoleInfo[] holeData) {
			this.name = name;
			this.description = description;
			holeIDs = new string[holeData.Length];
			for (int i = 0; i < holeIDs.Length; i++) {
				holeIDs[i] = holeData[i]._id;
			}
		}

		public void DownloadLevels(UnityAction onComplete) {
			holes = new Level.Level[holeIDs.Length];
			int levelDownloadsRemaining = holes.Length;
			
			for (int i = 0; i < holes.Length; i++) {
				int index = i;
				GameSceneManager.serverManager.GetHole(holeIDs[i], level => {
					holes[index] = level;
					levelDownloadsRemaining--;
					Debug.Log("Levels Downloaded: "+(holes.Length-levelDownloadsRemaining));
					if (levelDownloadsRemaining == 0) {
						onComplete.Invoke();
					}
				});
			}
		}

		private void OnLevelDownloaded(string levelData) {
			
			
		}

		public override void Save() {
			
		}

		public static string GetScoreString(int score) {
			if (score > 0) {
				return "+" + score;
			}

			return "" + score;
		}
	}
}
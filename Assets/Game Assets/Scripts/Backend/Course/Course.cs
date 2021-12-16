using System;
using System.Collections.Generic;
using Backend.Level;
using Backend.Serialization;
using Game_Assets.Scripts.Backend.Server;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Course {
	[Serializable]
	public class Course : ServerSerializable {

		public SteamCourseData steamCourseData;
		public string[] holeIDs;

		[JsonIgnore] public Level.Level[] holes;
		public override string name { get; }
		public override string description { get; }

		public override string fileExtension { get => "golfcourse"; }
		public override string saveFolderName { get => "courses"; }
		public string itemTypeTag { get => "Course"; }

		public Course(string title, string description, DBHoleInfo[] holeData) {
			this.name = title;
			this.description = description;
			holeIDs = new string[holeData.Length];
			for (int i = 0; i < holeIDs.Length; i++) {
				holeIDs[i] = holeData[i]._id;
			}
		}

		public void DownloadLevels(UnityAction onComplete) {
			/*holes = new Level.Level[holeData.Length];
			int levelDownloadsRemaining = holes.Length;
			
			for (int i = 0; i < holes.Length; i++) {
				ServerLoader serverLoader = new ServerLoader();
				int holeIndex = i;
				serverLoader.GetFileFromID(holeData[i].id, levelData => {
					levelDownloadsRemaining--;
					if (levelData.Equals(String.Empty)) {
						throw new Exception("Missing level data for course");
					};
					Level.Level level = (Level.Level)JsonConvert.DeserializeObject(levelData, typeof(Level.Level));
					holes[holeIndex] = level;

					if (levelDownloadsRemaining == 0) {
						Debug.Log("Course level downloads complete");
						onComplete?.Invoke();
					}
				});
			}*/
		}

		private void OnLevelDownloaded(string levelData) {
			
			
		}

		public override void Save() {
			
		}
	}
}
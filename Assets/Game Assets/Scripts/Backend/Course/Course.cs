using System;
using System.Collections.Generic;
using Backend.Level;
using Backend.Serialization;
using Backend.Submittable;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Course {
	[Serializable]
	public class Course : IServerSerializable {

		public SteamCourseData steamCourseData;
		public SteamItemData[] steamHoleData;

		[JsonIgnore] public Level.Level[] holes;
		public string title { get; set; }
		public string description;

		public string fileExtension { get => "golfcourse"; }
		public string saveFolderName { get => "courses"; }
		public string itemTypeTag { get => "Course"; }

		public Course(string title, string description, SteamItemData[] holesInfo) {
			this.title = title;
			this.description = description;
			this.steamHoleData = holesInfo;
		}

		public void DownloadLevels(UnityAction onComplete) {
			/*holes = new Level.Level[steamHoleData.Length];
			int levelDownloadsRemaining = holes.Length;
			
			for (int i = 0; i < holes.Length; i++) {
				ServerLoader serverLoader = new ServerLoader();
				int holeIndex = i;
				serverLoader.GetFileFromID(steamHoleData[i].id, levelData => {
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

		public void Save() {
			
		}
	}
}
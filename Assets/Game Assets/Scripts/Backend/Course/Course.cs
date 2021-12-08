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
	public class Course : ISteamSerializable {

		public SteamItemData[] holesInfo;

		[JsonIgnore] public Level.Level[] holes;
		public string title;
		public string description;

		public string fileExtension { get => "golfcourse"; }
		public string saveFolderName { get => "courses"; }
		public string itemTypeTag { get => "Course"; }

		public Course(string title, string description, SteamItemData[] holesInfo) {
			this.title = title;
			this.description = description;
			this.holesInfo = holesInfo;
		}

		public void DownloadLevels(UnityAction onComplete) {
			holes = new Level.Level[holesInfo.Length];
			int levelDownloadsRemaining = holes.Length;
			
			for (int i = 0; i < holes.Length; i++) {
				SteamLoader steamLoader = new SteamLoader();
				int holeIndex = i;
				steamLoader.GetFileFromID(holesInfo[i].id, levelData => {
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
			}
		}

		private void OnLevelDownloaded(string levelData) {
			
			
		}

		public void Save() {
			
		}

		
	}
}
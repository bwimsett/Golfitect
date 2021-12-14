using System.Collections.Generic;
using System.IO;
using Backend.Level;
using Backend.Submittable;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;

namespace Backend.Serialization {
	/// <summary>
	/// Local save data for high scores and profile information. Uploaded to steam cloud, but not tracked in stats.
	/// </summary>
	public class UserScores {

		[JsonProperty] private Dictionary<PublishedFileId_t, float> timeScores;
		[JsonProperty] private Dictionary<PublishedFileId_t, int> courseScores;

		private const string FOLDER_NAME = "Cloud";
		private const string FILE_NAME = "scores.json";
		
		public UserScores() {
			timeScores = new Dictionary<PublishedFileId_t, float>();
			courseScores = new Dictionary<PublishedFileId_t, int>();
		}
		
		public void SetTimeScore(SteamItemData workshopItem, float time) {
			bool scoreExists = timeScores.TryGetValue(workshopItem.id, out float currentHighScore);
			
			if (scoreExists && time < currentHighScore) {
				timeScores.Remove(workshopItem.id);
			} else if (scoreExists && time > currentHighScore) {
				return;
			}
			
			timeScores.Add(workshopItem.id, time);
			SaveLocal();
		}
		
		public void SetCourseScore(SteamItemData workshopItem, int score) {
			bool scoreExists = courseScores.TryGetValue(workshopItem.id, out int currentHighScore);

			if (scoreExists && score < currentHighScore) {
				courseScores.Remove(workshopItem.id);
			} else if (scoreExists && score > currentHighScore) {
				return;
			}
			
			courseScores.Add(workshopItem.id, score);
			SaveLocal();
		}

		public bool GetTimeHighScore(SteamItemData workshopItem, out float score) {
			bool success = timeScores.TryGetValue(workshopItem.id, out score);
			return success;
		}
		
		public bool GetCourseHighScore(SteamItemData workshopItem, out int score) {
			bool success = courseScores.TryGetValue(workshopItem.id, out score);
			return success;
		}

		public void SaveLocal() {
			string scoresJson = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(GetSaveFolder()+FILE_NAME, scoresJson);
		}

		private string GetSaveFolder() {
			string path = Application.persistentDataPath + "/" + FOLDER_NAME;
			Directory.CreateDirectory(path);
			return path+"/";
		}


	}
}
using System.Collections.Generic;
using Backend.Level;
using Backend.Serialization;
using Backend.Submittable;
using Newtonsoft.Json;
using Steamworks;

namespace Backend.Course {
	public class Course : ISteamSerializable {
		
		[JsonProperty]
		public LevelInfo[] holes { get; private set; }
		public string title;
		public string description;

		public string fileExtension { get => "golfcourse"; }
		public string saveFolderName { get => "courses"; }

		public Course(string title, string description, LevelInfo[] holes) {
			this.title = title;
			this.description = description;
			this.holes = holes;
		}

		public void Save() {
			
		}

		
	}
}
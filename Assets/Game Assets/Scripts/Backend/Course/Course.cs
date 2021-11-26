using System.Collections.Generic;
using Backend.Level;
using Backend.Serialization;
using Backend.Submittable;
using Steamworks;

namespace Backend.Course {
	public class Course : ISteamSerializable {
		
		private LevelInfo[] holes;

		public string fileExtension { get; }
		public string saveFolderName { get; }

		public Course(LevelInfo[] holes) {
			this.holes = holes;
		}

		public void Save() {
			throw new System.NotImplementedException();
		}

		
	}
}
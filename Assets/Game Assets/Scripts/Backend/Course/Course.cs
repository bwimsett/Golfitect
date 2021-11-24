using System.Collections.Generic;
using Steamworks;

namespace Backend.Course {
	public class Course {

		public string name;
		public string description;
		private List<PublishedFileId_t> holes;

		public void AddHole(PublishedFileId_t holeID) {
			if (holes.Contains(holeID)) {
				return;
			}
			
			holes.Add(holeID);
		}

	}
}
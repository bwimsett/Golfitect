using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWLocaliser {
	[Serializable]
	public struct LocString {
		
		public string key;
		public Dictionary<string, object> args;
		public Dictionary<string, string> locArgIds;

		public LocString(string key, Dictionary<string, object> args = null, Dictionary<string, string> locArgIds = null) {
			this.key = key;
			this.args = args;
			this.locArgIds = locArgIds;
		}

	}
}
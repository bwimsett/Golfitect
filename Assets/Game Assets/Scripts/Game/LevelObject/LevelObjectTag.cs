using UnityEngine;

namespace Backend.Level {
	[CreateAssetMenu(fileName = "[TAG]", menuName ="Level Object Tag")]
	public class LevelObjectTag : ScriptableObject {

		public string name;
		public string id;

	}
}
using UnityEngine;

namespace Game {
	public class CameraController : MonoBehaviour {
		
		public Camera camera { get; private set; }

		public void Initialise() {
			this.camera = GetComponent<Camera>();
		}

	}
}
using Backend.Managers;
using UnityEngine;

namespace Game {
	public class Ball : MonoBehaviour {

		[SerializeField] private Rigidbody rigidbody;
		public GameObject mouseDragPlane;

		public void Swing(Vector3 swingVector) {
			rigidbody.AddForce(swingVector*10);
			GameManager.courseTracker.AddShot();
		}

	}
}
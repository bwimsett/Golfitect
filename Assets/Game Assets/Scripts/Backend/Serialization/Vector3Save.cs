using System;
using UnityEngine;


namespace Backend.Serialization {
	[Serializable]
	public class Vector3Save {

		public float x, y, z;

		public Vector3Save(Vector3 vector3) {
			this.x = vector3.x;
			this.y = vector3.y;
			this.z = vector3.z;
		}

		public Vector3 Vector3() {
			return new Vector3(x, y, z);
		}

	}
}
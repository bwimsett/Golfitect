using UnityEngine;

namespace Backend.Level {
	public class Flag : LevelObject {

		private FlatGround splitGround;

		public override void LevelBuilderHover(RaycastHit hit) {
			if (hit.collider == null) {
				ResetSplitGround();
			}
			
			LevelObject hitObject = hit.collider.GetComponent<LevelObject>();

			// Holepunch if the object is flat ground
			if (!hitObject || !(hitObject is FlatGround)) {
				ResetSplitGround();
				return;
			}

			FlatGround ground = (FlatGround)hitObject;

			if (!ground.isTemporary) {
				ResetSplitGround();
				splitGround = ground;
				ground.TempSplit(hit.point);
			}
		}

		private void ResetSplitGround() {
			if (splitGround) {
				splitGround.ResetTempSplit();
				splitGround = null;
			}
		}
		
		public override void ConstructLevelObject() {
			if (splitGround) {
				splitGround.ConfirmSplit();
			}
		}

		
	}
}
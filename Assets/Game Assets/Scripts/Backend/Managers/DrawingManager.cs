using Backend.Level;
using Backend.Managers;
using Shapes;
using UnityEngine;

public class DrawingManager : ImmediateModeShapeDrawer {

	[SerializeField] private LevelGrid levelGrid;
	[SerializeField] private Camera mainCamera;
	
	public override void DrawShapes(Camera mainCamera) {
		if( mainCamera != this.mainCamera ) // only draw in the player camera
			return;
		
		using (Draw.Command(mainCamera)) {
			levelGrid.DrawGrid();
			if (GameManager.currentLevel.ball) {
				GameManager.currentLevel.ball.DrawHandle();
			}
		}
	}
}

using Backend.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.GUI.LevelBuilder.BuildOptions {
	public class BuildMode_BuildOptions_Option : MonoBehaviour {

		private LevelObject levelObject;
		[SerializeField] private Image icon;
		
		public void SetLevelObject(LevelObject levelObject) {
			this.levelObject = levelObject;
			RuntimePreviewGenerator.BackgroundColor = Color.clear;
			RuntimePreviewGenerator.OrthographicMode = true;
			RuntimePreviewGenerator.Padding = 0;
			Rect rect = new Rect(0,0, icon.rectTransform.rect.width, icon.rectTransform.rect.height);
			RuntimePreviewGenerator.GenerateModelPreviewAsync(texture => {
				icon.sprite = Sprite.Create(texture, rect, Vector2.zero);
			}, levelObject.transform, Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height), true);
		}

		public void OnClick() {
			LevelManager.levelInputManager.levelBuilderTool.SetActive(true);
			LevelManager.levelInputManager.levelBuilderTool.SetLevelObject(levelObject);
		}
		
		
	}
}
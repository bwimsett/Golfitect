using Backend.Managers;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings_Checkbox : MonoBehaviour {

	[SerializeField] private Image background, icon;
	[SerializeField] private Sprite trueSprite, falseSprite;
	
	public bool value { get; private set; }
	
	public void SetValue(bool value) {
		this.value = value;

		if (value) {
			icon.sprite = trueSprite;
			background.color = GameSceneManager.colorBank.GetColor("generic_green");
		}
		else {
			icon.sprite = falseSprite;
			background.color = GameSceneManager.colorBank.GetColor("generic_red");
		}
	}

}

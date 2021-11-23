using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Backend.Level {
	public class BWConsole_OutputLine : MonoBehaviour {

		[SerializeField] private TextMeshProUGUI text;

		public void SetText(string text) {
			this.text.text = text;
		}

		public void SetTextColor(Color color) {
			text.color = color;
		}

		public void Reset() {
			text.color = Color.white;
		}

	}
}
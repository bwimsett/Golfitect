using System.Collections.Generic;
using Game_Assets.Scripts.GUI.PlayMode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game_Assets.Scripts.GUI {
	public class GUIColorBank : ScriptableObject {

		[SerializeField] public SerializedDictionary<string, Color> colors;

		public Color GetColor(string ID) {
			Color color = Color.magenta;
			bool success = colors.TryGetValue(ID, out color);
			if (!success) {
				Debug.LogError("Could not find color with ID: "+ID);
			}
			return color;
		}

	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spinner : MonoBehaviour {

	public TextMeshProUGUI valueText;
	public Button incrementButton, decrementButton;
	
	public int value { get; private set; }
	private int min = 0, max;

	public Action<int> OnValueChanged;
	
	public void SetMinMax(int min, int max) {
		this.min = min;
		this.max = max;

		// Clamp current value to within the min/max range
		SetValue(value);
	}

	public void SetValue(int value) {
		this.value = Mathf.Min(max, Mathf.Max(min, value));
		OnValueChanged?.Invoke(this.value);
		Refresh();
	}
	
	
	public void Increment() {
		SetValue(value+1);
		OnValueChanged?.Invoke(value);
	}

	public void Decrement() {
		SetValue(value-1);
		OnValueChanged?.Invoke(value);
	}

	public void Refresh() {
		valueText.text = "" + value;
		incrementButton.interactable = value < max;
		decrementButton.interactable = value > min;
	}

}

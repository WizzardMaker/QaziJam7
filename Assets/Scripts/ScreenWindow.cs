using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenWindow : MonoBehaviour {

	public Dictionary<string, Button> buttons;

	// Use this for initialization
	void Awake () {

		buttons = new Dictionary<string, Button>();

		foreach(Button b in GetComponentsInChildren<Button>(true)) {
			buttons.Add(b.gameObject.name, b);
		}
	}
	
	public Button GetButton(string name) {
		return buttons[name];
	}
}

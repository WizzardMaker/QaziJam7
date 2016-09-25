using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {

	static List<ScreenWindow> screens;

	static int activeScreen = 0;

	// Use this for initialization
	void Start () {
		screens = new List<ScreenWindow>();
		screens.AddRange(GetComponentsInChildren<ScreenWindow>());

		DeactivateAll();
	}

	public static void DeactivateAll() {
		foreach (ScreenWindow s in screens)
			s.gameObject.SetActive(false);
	}
	
	public static void SetActiveScreen(int id) {
		if (id == -1) {
			DeactivateAll();
			return;
		}

		if (id >= screens.Count)
			throw new IndexOutOfRangeException();

		screens[activeScreen].gameObject.SetActive(false);

		activeScreen = id;

		screens[activeScreen].gameObject.SetActive(true);
	}

	public static void SetActiveScreen(string name) {
		if (name == "None") {
			DeactivateAll();
			return;
		}

		int it = 0;
		bool found = false;
		foreach(ScreenWindow s in screens) {
			if (s.gameObject.name == name) {
				found = true;
				break;
			}

			it++;

		}

		if (found == false)
			throw new KeyNotFoundException();

		screens[activeScreen].gameObject.SetActive(false);

		activeScreen = it;

		screens[activeScreen].gameObject.SetActive(true);
	}
}

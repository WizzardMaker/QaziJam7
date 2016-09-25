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
		screens.AddRange(GetComponentsInChildren<ScreenWindow>(true));

		DeactivateAll();
	}

	public static ScreenWindow GetActiveScreen() {
		if (activeScreen == -1)
			return null;

		return screens[activeScreen];
	}

	public static void DeactivateAll() {
		foreach (ScreenWindow s in screens)
			s.gameObject.SetActive(false);

		activeScreen = -1;
	}
	
	public static ScreenWindow SetActiveScreen(int id) {
		if (id == -1) {
			DeactivateAll();
			return GetActiveScreen();
		}

		if (id >= screens.Count)
			throw new IndexOutOfRangeException();

		if (activeScreen != -1)
			screens[activeScreen].gameObject.SetActive(false);

		activeScreen = id;

		screens[activeScreen].gameObject.SetActive(true);

		return GetActiveScreen();
	}

	public static ScreenWindow SetActiveScreen(string name) {
		if (name == "None") {
			DeactivateAll();
			return GetActiveScreen();
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

		if(activeScreen != -1)
			screens[activeScreen].gameObject.SetActive(false);

		activeScreen = it;

		screens[activeScreen].gameObject.SetActive(true);

		return GetActiveScreen();
	}
}

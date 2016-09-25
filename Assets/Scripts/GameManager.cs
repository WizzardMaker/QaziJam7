using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	bool _isGameOver = false;
	public bool isGameOver {
		get { return _isGameOver; }
		set {
			_isGameOver = value;

			ScreenController.SetActiveScreen("None");
		}
	}
	bool _isVictory = false;
	public bool isVictory {
		get { return _isVictory; }
		set {
			_isVictory = value;

			ScreenController.SetActiveScreen("None");
		}
	}

	public GameObject levelParent;
	private List<Level> levels;
	public int curLevel = 0;

	// Use this for initialization
	void Start () {
		instance = this;

		levels = new List<Level>();
		foreach(Level l in levelParent.GetComponentsInChildren<Level>(true)) {
			l.EndLevel();
			levels.Add(l);
		}

		StartLevel(0);
		StartCoroutine(CheckValues());
	}

	public void Exit() {
		Application.Quit();
		Debug.Log("Exit!");
	}

	public void NextLevel() {
		//Toggle Victory Screen
		isVictory = true;

		ScreenWindow v = ScreenController.SetActiveScreen("Victory");

		

		v.GetButton("Next").onClick.AddListener(() =>{
			StartLevel(curLevel + 1); isVictory = false;
		});


		//StartLevel(curLevel+1);
	}
	public void RestartLevel() {
		StartLevel(curLevel);
		isGameOver = false;
	}

	public void StartLevel(int level) {
		if (level >= levels.Count) {
			//Final level
		} else {
			Debug.Log((level + 1) + "/" + levels.Count);


			levels[curLevel].EndLevel();
			curLevel = level;

			//Wait for the user to hit "Next Level"
			levels[curLevel].StartLevel();
		}
	}

	public static void BroadcastAll(string function, object msg = null) {
		GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(function, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	IEnumerator CheckValues() {
		for (;;) {
			Time.timeScale = isGameOver || isVictory ? 0 : 1;

			yield return null;
		}
	}

	void OnGameOver() {
		isGameOver = true;

		ScreenController.SetActiveScreen("GameOver");

		//Temp!
		//RestartLevel();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public bool isGameOver = false;

	public GameObject levelParent;
	private List<Level> levels;
	public int curLevel = 0;

	// Use this for initialization
	void Start () {
		instance = this;

		levels = new List<Level>();
		foreach(Level l in levelParent.GetComponentsInChildren<Level>()) {
			l.EndLevel();
			levels.Add(l);
		}

		StartLevel(0);
	}

	public void NextLevel() {
		StartLevel(curLevel+1);
	}
	public void RestartLevel() {
		StartLevel(curLevel);
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

	void OnGameOver() {
		isGameOver = true;

		//Temp!

		RestartLevel();
	}
}

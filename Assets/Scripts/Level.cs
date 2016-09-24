using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public GameObject start;
	public GameObject cameraStart;
	public Goal goal;

	public void StartLevel() {
		Player p = Player.instance;

		if (cameraStart == null)
			cameraStart = (from Transform c in transform.GetComponentsInChildren<Transform>()
						  where c.tag == "CameraSpawn"
						  select c).First().gameObject;

		if (start == null)
			start = (from Transform c in transform.GetComponentsInChildren<Transform>()
						   where c.tag == "Spawn"
						   select c).First().gameObject;
		if (goal == null)
			goal = GetComponentInChildren<Goal>();

		goal.parent = this;

		Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionCurves>().saturation = 0;

		gameObject.SetActive(true);

		p.transform.position = start.transform.position;
		StopPlayer();

		Camera.main.transform.position = cameraStart.transform.position;
		Camera.main.transform.rotation = cameraStart.transform.rotation;
	}

	public void StopPlayer() {
		Player p = Player.instance;

		p.GetComponent<Rigidbody>().isKinematic = true;
		p.GetComponent<Rigidbody>().isKinematic = false;
	}

	public void FinishLevel() {
		//GameManager.instance.NextLevel();
		StartCoroutine(Finish());
	}

	float fadeDuration = 1;
	IEnumerator Finish() {
		for (float f = 0; f <= 2; f += 0.1f) {
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ColorCorrectionCurves>().saturation = f;
			yield return new WaitForSeconds(fadeDuration/10);
		}

		GameManager.instance.NextLevel();
	}

	public void EndLevel() {
		gameObject.SetActive(false);
	}
}

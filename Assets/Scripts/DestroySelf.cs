using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroyIn());
	}

	IEnumerator DestroyIn() {
		while (GetComponent<AudioSource>().isPlaying)
			yield return null;

		Destroy(gameObject);
	}
}

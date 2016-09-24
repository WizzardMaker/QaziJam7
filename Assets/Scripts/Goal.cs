using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public Level parent;


	public void OnTriggerEnter(Collider other) {
		Player p = other.GetComponent<Player>() ?? other.GetComponentInParent<Player>() ?? other.GetComponentInChildren<Player>();
		if (p != null) {
			parent.FinishLevel();
		}
	}
}

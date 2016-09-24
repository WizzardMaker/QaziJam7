using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour {

	// Use this for initialization
	protected void Start() {

	}

	// Update is called once per frame
	protected void Update() {
		OnUpdate();
	}

	public void OnTriggerEnter(Collider other) {
		Player p = other.GetComponent<Player>() ?? other.GetComponentInParent<Player>() ?? other.GetComponentInChildren<Player>();
		if (p != null){
			OnPlayerCollideEnter(p);
		}
	}

	public void OnTriggerStay(Collider other) {
		Player p = other.GetComponent<Player>();
		if (p != null) {
			OnPlayerCollideStay(p);
		}
	}

	public abstract void OnPlayerCollideEnter(Player player);
	public abstract void OnPlayerCollideStay(Player player);

	protected virtual void OnUpdate() { }
}

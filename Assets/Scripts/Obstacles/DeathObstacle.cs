using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathObstacle : ObstacleBase {

	public override void OnPlayerCollideEnter(Player player) {
		//Debug.Log("Dead!");
		GameManager.BroadcastAll("OnGameOver");
	}

	public override void OnPlayerCollideStay(Player player) {}


}

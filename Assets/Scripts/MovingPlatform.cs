﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	static List<MovingPlatform> active = new List<MovingPlatform>();

	Transform platform;

	int curPoint = -1;
	List<Transform> pathPoints;

	public float platformSpeed;

	public LoopMode mode;

	public enum LoopMode {
		PingPong,
		Loop
	}
	

	Vector3 direction;
	Transform destination;

	void Start() {
		GetData();

		SetDestination();

		active.Add(this);
	}

	public static void ResetPlatform() {
		foreach (MovingPlatform i in active) {
			i.platform.transform.position = i.pathPoints[0].position;
			i.curPoint = 1;
			i.destination = i.pathPoints[i.curPoint];
			i.direction = (i.destination.position - i.platform.position).normalized;
		}
	}

	void FixedUpdate() {
		platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * platformSpeed * Time.fixedDeltaTime);

		if (Vector3.Distance(platform.position, destination.position) < platformSpeed * Time.fixedDeltaTime + 0.5f) {
			SetDestination();
		}
	}

	void GetData() {
		Transform parent = null;
		foreach (Transform t in transform) {
			if (t.name == "Object")
				platform = t;

			if (t.name == "Points") {
				parent = t;
			}


			if (parent != null && platform != null)
				break;
		}

		pathPoints = new List<Transform>();

		foreach (Transform c in parent) {
			pathPoints.Add(c);
		}
	}

	void OnDrawGizmos() {
		GetData();

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(pathPoints[0].position, platform.localScale);

		if(!UnityEditor.EditorApplication.isPlaying)
			platform.position = pathPoints[0].position;

		bool s = true;
		Transform prev = pathPoints[0];
		foreach (Transform t in pathPoints) {
			if (s) {
				s = false;
				continue;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(t.position, platform.localScale);

			Gizmos.DrawLine(prev.position, t.position);
			prev = t;
		}

		if(mode == LoopMode.Loop)
			Gizmos.DrawLine(prev.position, pathPoints[0].position);
	}


	bool isReturning = false;
	void SetDestination() {
		if (!isReturning)
			curPoint++;
		else
			curPoint--;

		if (curPoint >= pathPoints.Count) {
			if (mode == LoopMode.Loop)
				curPoint = 0;
			else {
				isReturning = true;
				curPoint -= 2;
			}
		}
		if(curPoint < 0) {
			isReturning = false;
			curPoint = 1;
		}

		destination = pathPoints[curPoint];
		direction = (destination.position - platform.position).normalized;

	}

}

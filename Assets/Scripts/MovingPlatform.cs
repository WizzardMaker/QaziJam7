using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	
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

	}

	void FixedUpdate() {
		platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * platformSpeed * Time.fixedDeltaTime);

		if (Vector3.Distance(platform.position, destination.position) < platformSpeed * Time.fixedDeltaTime) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public static Player instance;

    public float shootSteps = 10;
    public float speed = 5;
    public float angle = 45;

	public bool hasAborted = false;

	Vector3 velocity;


	bool isGrounded {
		get {
			return Physics.Raycast(transform.position, -Vector3.up, 1);
		}
	}

	bool canMove {
		get {
			return isGrounded && rig.velocity.magnitude < 0.25;
		}
	}

    Rigidbody rig;

    Vector3 oldPos;

    // Use this for initialization
    void Awake() {
		instance = this;

        oldPos = transform.position;
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time) {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {

		LineRenderer line = GetComponentInChildren<LineRenderer>();

		if (startVelocity.magnitude < 0.05f) {
			line.SetPositions(new Vector3[0]);
			line.numPositions = 0;
			return;
		}

        Vector3 prev = start;

        List<Vector3> points = new List<Vector3>();


        points.Add(start);

        for (int i = 1; ; i++) {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            if (Physics.Linecast(prev, pos,1 << 8)) break;
            //Debug.DrawLine(prev, pos, Color.red);

            points.Add(pos);
            //Debug.Log(pos);
            prev = pos;
        }

        line.numPositions = points.Count;

        line.SetPositions(points.ToArray());
    }

	void Aim() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + Vector3.forward);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit,Mathf.Infinity,1 << 10)) {
			Vector3 speed = hit.point - transform.position;

			speed.y = 0;
			speed.x = Mathf.Clamp(speed.x, -8, 8);
			speed.z = Mathf.Clamp(speed.z, -8, 8);

			speed.y = -angle * (speed.magnitude/10);

			velocity = -speed;
		}
	}

    // Update is called once per frame
    void Update() {
        rig = GetComponent<Rigidbody>();


		if (Input.GetMouseButton(0) && !hasAborted && canMove) {
			Aim();

			if (Input.GetKey(KeyCode.Escape))
				hasAborted = true;
		}else if (canMove && !Input.GetMouseButtonUp(0)) {
			velocity = Vector3.zero;
		}

		if (Input.GetMouseButtonUp(0) && canMove) {
			if(!hasAborted)
				rig.AddForce(velocity, ForceMode.Impulse);

			hasAborted = false;
		}

		if (rig.velocity.magnitude < 0.25f) {
			PlotTrajectory(transform.position, velocity, .1f, 3);
			oldPos = transform.position;
		} else {
			PlotTrajectory(oldPos, velocity, .1f, 3);
		}

	}
}

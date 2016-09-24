using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float shootSteps = 10;
    public float speed = 5;
    public float angle = 45;

	Vector3 velocity;


    Rigidbody rig;

    Vector3 oldPos;

    // Use this for initialization
    void Start() {
        oldPos = transform.position;
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time) {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {
        Vector3 prev = start;

        List<Vector3> points = new List<Vector3>();


        points.Add(start);

        for (int i = 1; ; i++) {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            //if (Physics.Linecast(prev, pos)) break;
            //Debug.DrawLine(prev, pos, Color.red);

            points.Add(pos);
            //Debug.Log(pos);
            prev = pos;
        }

        LineRenderer line = GetComponent<LineRenderer>();
        line.numPositions = points.Count;

        line.SetPositions(points.ToArray());
    }

	void Aim() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + Vector3.forward);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Vector3 speed = hit.point - transform.position;

			speed.y = 0;
			speed.x = Mathf.Clamp(speed.x, -8, 8);
			speed.y = Mathf.Clamp(speed.y, -8, 8);

			speed.y = -angle * (speed.magnitude/10);

			velocity = -speed;
		}
	}

    // Update is called once per frame
    void Update() {
        rig = GetComponent<Rigidbody>();

        if (rig.velocity.magnitude < 0.25f) {
            PlotTrajectory(transform.position, velocity, .1f, 2);
            oldPos = transform.position;
        } else {
            PlotTrajectory(oldPos, velocity, .1f, 2);
        }

		if (Input.GetMouseButton(0))
			Aim();

		if (Input.GetMouseButtonUp(0))
			rig.AddForce(velocity, ForceMode.Impulse);
    }
}

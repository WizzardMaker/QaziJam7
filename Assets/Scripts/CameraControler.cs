using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

	public float rotateSpeed = 2;
	public float speed = 2;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

		if(Input.GetMouseButton(1) && !Input.GetMouseButton(2))
			transform.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed, Input.GetAxis("Mouse X") * rotateSpeed, 0);

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);


		transform.position +=
			transform.right * Input.GetAxis("Horizontal") * speed
			+ transform.forward * Input.GetAxis("Vertical") * speed
			+ Vector3.up * Input.GetAxis("UpAndDown") * speed;
		if (Input.GetMouseButton(2)) {
			transform.position += transform.right * Input.GetAxis("Mouse X") * -speed + transform.up * Input.GetAxis("Mouse Y") * -speed;
		}
	}
}

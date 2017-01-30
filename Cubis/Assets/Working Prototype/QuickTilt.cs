using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTilt : MonoBehaviour {

	public float smooth = 2.0F;
	public float tiltAngle = 60.0F;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float tiltAroundZ = Input.GetAxis("Mouse X") * tiltAngle;
		float tiltAroundX = Input.GetAxis("Mouse Y") * tiltAngle;
		Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
	}
}

/*using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public GameObject targetObject;
	private float targetAngle = 0;
	const float rotationAmount = 1.5f;
	public float rDistance = 1.0f;
	public float rSpeed = 1.0f;
	Vector3 direction;
	bool rotating = false;
	
	// Update is called once per frame
	void Update()
	{
		
		// Trigger functions if Rotate is requested
		if (!rotating){
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				rotating = true;
				direction = Vector3.left;
				targetAngle -= 90.0f;
			} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				rotating = true;
				direction = Vector3.right;
				targetAngle += 90.0f;
			}else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				rotating = true;
				direction = Vector3.up;
				targetAngle += 90.0f;
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				rotating = true;
				direction = Vector3.down;
				targetAngle -= 90.0f;
			}
		}
		
		if(targetAngle !=0)
		{
			Rotate();
		} else rotating = false;
	}
	
	protected void Rotate()
	{
		
		float step = rSpeed * Time.deltaTime;
		float orbitCircumfrance = 2F * rDistance * Mathf.PI;
		float distanceDegrees = (rSpeed / orbitCircumfrance) * 360;
		float distanceRadians = (rSpeed / orbitCircumfrance) * 2 * Mathf.PI;
		
		if (targetAngle>0)
		{
			transform.RotateAround(targetObject.transform.position, direction, -rotationAmount);
			targetAngle -= rotationAmount;
		}
		else if(targetAngle <0)
		{
			transform.RotateAround(targetObject.transform.position, direction, rotationAmount);
			targetAngle += rotationAmount;
		}
		
	}
}
*/
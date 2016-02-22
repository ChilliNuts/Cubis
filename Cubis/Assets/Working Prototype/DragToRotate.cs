using UnityEngine;
using System.Collections;

public class DragToRotate : MonoBehaviour {

	public int speed;
	public float lerpSpeed, minSwipeLength;
	
	private Vector3 dragStart, dragStop, dragDir;
	private Quaternion toRotation;
	private bool isRotating = false, finishedRotation = false;
	private CreateCuboid cuboid;
	private PlayManager playManager;

	void Start(){
		cuboid = FindObjectOfType<CreateCuboid>();
		playManager = FindObjectOfType<PlayManager>();
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)){
			dragStart = Input.mousePosition;

		}
		if (Input.GetMouseButton (0)) {
			dragStop = Input.mousePosition;
			dragDir = dragStart - dragStop;

			//toRotation = Quaternion.Euler ((-dragDir.y * speed) / Screen.height, (dragDir.x * speed) / Screen.width, 0);

		}
		if (Input.GetMouseButtonUp (0)){
			// if mouseX > mouseY...

			toRotation = Quaternion.Euler (GetRotationDirection(dragDir));
			finishedRotation = false;
			isRotating = true;
		}

		if(isRotating && transform.rotation != toRotation){
			transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, Time.deltaTime * lerpSpeed);
		} else if(isRotating && !finishedRotation){
			finishedRotation = true;
		}
		if(finishedRotation){
			transform.rotation = toRotation;
			cuboid.ToggleParent ();
			toRotation = Quaternion.Euler(0, 0, 0);
			transform.rotation = Quaternion.identity;
			cuboid.ToggleParent ();
			isRotating = false;
			playManager.CreatePlayableArea ();
		}

	}

	Vector3 GetRotationDirection(Vector3 dragDir){
		if (Mathf.Abs (dragDir.x) > Mathf.Abs (dragDir.y)) {
			if(dragDir.x > minSwipeLength){
				return new Vector3(0, 90f, 0);
			}
			else if(dragDir.x < -minSwipeLength){
				return new Vector3(0, - 90f, 0);
			}
		}else if (Mathf.Abs (dragDir.y) > Mathf.Abs (dragDir.x)) {
			if(dragDir.y > minSwipeLength){
				return new Vector3(- 90f ,0 , 0);
			}
			else if(dragDir.y < -minSwipeLength){
				return new Vector3(+ 90f ,0 , 0);
			}
		}
		return Vector3.zero;
	}
}

//public float smooth = 2.0F;
//public float tiltAngle = 30.0F;
//float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
//float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
//Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
//transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

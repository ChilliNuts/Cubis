using UnityEngine;
using System.Collections;

public class DragToRotate : MonoBehaviour {

	public int speed;
	public float lerpSpeed, minSwipeLength;
	public static bool isRotating = false;
	
	private Vector3 dragStart, dragStop, dragDir;
	private Quaternion toRotation;
	private CreateCuboid cuboid;
	private PlayManager playManager;


	private float xDeg;
	private float yDeg;
	private Quaternion fromDragRotation;
	private Quaternion toDragRotation;
	private bool clickedDown = false;

	public float smooth = 2.0F;
	public float tiltAngle = 60.0F;

	void Start(){
		cuboid = FindObjectOfType<CreateCuboid>();
		playManager = FindObjectOfType<PlayManager>();
	}

	void Update () {
		
		if (!isRotating && !PlayManager.lockInput) {
			if (Input.GetMouseButtonDown (0)) {
				dragStart = Input.mousePosition;
				clickedDown = true;
			
			}
			if (Input.GetMouseButton (0) && clickedDown) {
				dragStop = Input.mousePosition;
				dragDir = dragStart - dragStop;
				if(dragDir.magnitude >= 5f){
					PlayManager.lockInput = true;
					TiltCuboid ();
				}

				//toRotation = Quaternion.Euler ((-dragDir.y * speed) / Screen.height, (dragDir.x * speed) / Screen.width, 0);
			
			}
			if (Input.GetMouseButtonUp (0) && clickedDown) {
				clickedDown = false;
				if (!isRotating) {
					toRotation = Quaternion.Euler (GetRotationDirection (dragDir));
					isRotating = true;
					StartCoroutine (SnapToRotation ());
				}
			
			}
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

	void TiltCuboid(){

		float tiltAroundZ = Mathf.Clamp(dragDir.x/minSwipeLength,-1,1)  * tiltAngle;
		float tiltAroundX = Mathf.Clamp(dragDir.y/minSwipeLength,-1,1) * tiltAngle;
		Quaternion target = Quaternion.Euler(-tiltAroundX, tiltAroundZ, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
	}

	IEnumerator SnapToRotation(){
		if (dragDir.magnitude >= minSwipeLength) {
			playManager.DeselectCube ();
		}
		while(transform.rotation != toRotation){
			transform.rotation = Quaternion.Lerp (transform.rotation, toRotation, Time.deltaTime * lerpSpeed);
			yield return null;
		} 
			
		transform.rotation = toRotation;
		cuboid.ToggleParent ();
		toRotation = Quaternion.Euler(0, 0, 0);
		transform.rotation = Quaternion.identity;
		cuboid.ToggleParent ();
		isRotating = false;
		playManager.CreatePlayableArea();
		PlayManager.lockInput = false;
	}
}

//public float smooth = 2.0F;
//public float tiltAngle = 30.0F;
//float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
//float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
//Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
//transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);


//if(isRotating && transform.rotation != toRotation){
//	transform.rotation = Quaternion.Slerp (transform.rotation, toRotation, Time.deltaTime * lerpSpeed);
//} else if(isRotating && !finishedRotation){
//	finishedRotation = true;
//}
//if(finishedRotation){
//	transform.rotation = toRotation;
//	cuboid.ToggleParent ();
//	toRotation = Quaternion.Euler(0, 0, 0);
//	transform.rotation = Quaternion.identity;
//	cuboid.ToggleParent ();
//	isRotating = false;
//	playManager.CreatePlayableArea ();
//}

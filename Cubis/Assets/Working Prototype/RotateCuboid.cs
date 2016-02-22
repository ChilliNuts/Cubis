using UnityEngine;
using System.Collections;


public class RotateCuboid : MonoBehaviour {

		PlayManager playManager;


		Quaternion[] orientations = new Quaternion[] {
		Quaternion.LookRotation(Vector3.forward, Vector3.right),
		Quaternion.LookRotation(Vector3.forward, -Vector3.right),
		Quaternion.LookRotation(Vector3.forward, Vector3.up),
		Quaternion.LookRotation(Vector3.forward, -Vector3.up),
		Quaternion.LookRotation(-Vector3.forward, Vector3.right),
		Quaternion.LookRotation(-Vector3.forward, -Vector3.right),
		Quaternion.LookRotation(-Vector3.forward, Vector3.up),
		Quaternion.LookRotation(-Vector3.forward, -Vector3.up),
		Quaternion.LookRotation(Vector3.right, Vector3.forward),
		Quaternion.LookRotation(Vector3.right, -Vector3.forward),
		Quaternion.LookRotation(Vector3.right, Vector3.up),
		Quaternion.LookRotation(Vector3.right, -Vector3.up),
		Quaternion.LookRotation(-Vector3.right, Vector3.forward),
		Quaternion.LookRotation(-Vector3.right, -Vector3.forward),
		Quaternion.LookRotation(-Vector3.right, Vector3.up),
		Quaternion.LookRotation(-Vector3.right, -Vector3.up),
		Quaternion.LookRotation(Vector3.up, Vector3.right),
		Quaternion.LookRotation(Vector3.up, -Vector3.right),
		Quaternion.LookRotation(Vector3.up, Vector3.forward),
		Quaternion.LookRotation(Vector3.up, -Vector3.forward),
		Quaternion.LookRotation(-Vector3.up, Vector3.right),
		Quaternion.LookRotation(-Vector3.up, -Vector3.right),
		Quaternion.LookRotation(-Vector3.up, Vector3.forward),
		Quaternion.LookRotation(-Vector3.up, -Vector3.forward)
	};
	
	int[,] neighborIndices = new int[24, 4];
	
	int orientationIndex;
	Quaternion orientation1;
	Quaternion orientation2;
	bool rotating;
	float rotationTimer;
	
	void Start()
	{
		Quaternion[] rotations = new Quaternion[] {
			Quaternion.AngleAxis(-90, Vector3.up),
			Quaternion.AngleAxis(90, Vector3.up),
			Quaternion.AngleAxis(-90, Vector3.right),
			Quaternion.AngleAxis(90, Vector3.right)
		};
		
		for (int i = 0; i < 24; ++i) {
			for (int j = 0; j < 4; ++j) {
				var currentOrientation =
					rotations[j] * orientations[i];
				float maxDot = 0f;
				int neighborIndex = 0;
				for (int k = 0; k < 24; ++k) {
					float dot = Mathf.Abs(Quaternion.Dot(
						currentOrientation, orientations[k]));
					if (dot > maxDot) {
						maxDot = dot;
						neighborIndex = k;
					}
				}
				neighborIndices[i, j] = neighborIndex;
			}
		}
		
		transform.rotation = orientations[orientationIndex];
		playManager = GetComponent<PlayManager>();
	}
	
	void Update()
	{
		const float rotationTime = 0.3f;
		if (rotating) {
			rotationTimer += Time.deltaTime;
			if (rotationTimer >= rotationTime) {
				transform.rotation = orientation2;
				playManager.CreatePlayableArea ();
				rotating = false;
			} else {
				float t = rotationTimer / rotationTime;
				transform.rotation = Quaternion.Lerp(
					orientation1, orientation2, t);
			}
		} else {
			int rotation = -1;
			if (!PlayManager.lockInput) {
				if (Input.GetKeyDown(KeyCode.RightArrow)) {
					rotation = 0;
				} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
					rotation = 1;
				} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
					rotation = 2;
				} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
					rotation = 3;
				}
			}
			if (rotation != -1) {
				orientationIndex =
					neighborIndices[orientationIndex, rotation];
				orientation1 = transform.rotation;
				orientation2 = orientations[orientationIndex];
				rotationTimer = 0f;
				rotating = true;
			}
		}
	}
}




	/*private float yDegree;
	private float xDegree;
	public float speed = 10f;
	bool yRotate;
	bool xRotate;
	
	void Update() 
	{
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			yRotate = true;
			xRotate = false;
			yDegree += 90f;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow)){
			yRotate = true;
			xRotate = false;
			yDegree -= 90f;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			yRotate = false;
			xRotate = true;
			xDegree += 90f;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow)){
			yRotate = false;
			xRotate = true;
			xDegree -= 90f;
		}

	


		if (yRotate){
			transform.rotation = Quaternion.Slerp(transform.rotation, 
			                                      Quaternion.Euler(0, yDegree, 0), 
			                                      Time.deltaTime * speed);
		}else if (xRotate){
			transform.rotation = Quaternion.Slerp(transform.rotation, 
			                                      Quaternion.Euler(xDegree, 0, 0), 
			                                      Time.deltaTime * speed);
		}
	}
}


/*public float target = 270.0F;
	public float speed = 90.0F;


	void Update() {
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			target += 90f;
		}
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, target, speed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}






public float smooth = 2.0F;
	public float tiltAngle = 90.0F;
	float tiltAroundY;
	float tiltAroundX;
	void Update() {

		if (Input.GetKeyUp(KeyCode.D)){
			 tiltAroundY += tiltAngle;
		}
		if (Input.GetKeyUp(KeyCode.A)){
			tiltAroundY -= tiltAngle;
		}
		if (Input.GetKeyUp(KeyCode.W)){
			tiltAroundX += tiltAngle;
		}
		if (Input.GetKeyUp(KeyCode.S)){
			tiltAroundX -= tiltAngle;
		}

		Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundY, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
	}
*/
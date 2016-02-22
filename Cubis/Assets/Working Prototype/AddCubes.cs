using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddCubes : MonoBehaviour {


	bool cubeSpawned = false;
	public GameObject cube;
	CreateCuboid cuboid;

	// Use this for initialization
	void Start () {
		cuboid = FindObjectOfType<CreateCuboid>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Space)){
			AddCube ();
		}
	}

	public void AddCube(){
		CheckColorsInPlay(GetComponent<Cube>());
		Vector3[] sides = new Vector3[] {Vector3.up, Vector3.down, Vector3.left, Vector3.right, 
										 Vector3.forward, Vector3.back};

		int chanceToSpawnCube = 50 / cuboid.allCubes.Count;
		foreach(Vector3 side in sides){
			int randomSpawn = Random.Range(1, 100);
			RaycastHit hit;
			if(Physics.Raycast (transform.position, side, out hit, 1f)){

				print ("hit another cube");
			} else {

				if(randomSpawn < chanceToSpawnCube && cubeSpawned == false){
					print ("Spawn Cube");
					InstantiateCube(transform.position, side);
					cubeSpawned = true;
				}
				print ("didnt hit anything");
			}
		}
		cubeSpawned = false;
	}

	void InstantiateCube(Vector3 origin, Vector3 side){

		Vector3 startPos = new Vector3(origin.x + side.x, origin.y + side.y, origin.z + side.z);
		print (startPos);
		GameObject newCube = Instantiate (cube, startPos, Quaternion.identity)as GameObject;
		newCube.transform.parent = cuboid.transform;
		Cube c = newCube.GetComponent<Cube>();
		cuboid.allCubes.Add (c);
		StartCoroutine (GrowCube (c));
	}
	IEnumerator GrowCube(Cube c){
		Vector3 finalScale = new Vector3(0.9f, 0.9f, 0.9f);
		float elapsedTime = 0;
		float time = 0.5f;
		
		while (elapsedTime < time){
			c.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f,  0.1f), finalScale, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		c.transform.localScale = finalScale;
	}
	void CheckColorsInPlay(Cube cube){

		foreach(Color color in cube.colors){
			int amountThisColor = 0;
			foreach(Cube c in cuboid.allCubes){
				if (color == c.myColor){
					amountThisColor++;
				}
			}
			if (amountThisColor == 0){
				cube.colors.Remove(color);
			}
		}
	}
}

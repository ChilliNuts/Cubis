using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour {


	CreateCuboid cuboid;
	UIManager uIManager;
	List<Cube> seenCubes = new List<Cube>();
	List<Cube> matchedList = new List<Cube>();
	List<Cube> matchMaking = new List<Cube>();
	int amountToMatch = 3;
	public bool isMatching = false;
	bool matchFound = false;
	public Cube lastCube;
	Vector3 cube1Start, cube1End, cube2Start, cube2End;
	Cube cube1, cube2;
	public bool isSwapping = false;
	public float matchTimer = 0f;
	public int freeMoves = 0;
	int numberOfCubesMatched = 1;
	public static bool lockInput = false;



	// Use this for initialization
	void Start () {
		cuboid = GetComponent<CreateCuboid>();
		uIManager = FindObjectOfType<UIManager>();
		uIManager.UpdateText (uIManager.freeMovesText, "Free Moves: "+freeMoves);
		CreatePlayableArea ();
	}
	
	// Update is called once per frame
	void Update () {
		print(isMatching);
		if (isMatching){
			PlayManager.lockInput = true;
			Invoke ("CheckForMatches", matchTimer);
		}else {
			PlayManager.lockInput = false;
			matchTimer = 0f;
		}
	}


	public void SelectCube(Cube currentCube){
		if (lastCube == null){ // new selection
			lastCube = currentCube;
		}else if(lastCube == currentCube){ // deselect
			lastCube = null;
		}else if(lastCube.IsNeighborsWith(lastCube, currentCube)){
			SwapCubes(lastCube, currentCube);
			CheckForMatches ();
			if(!matchFound){
				if (freeMoves <= 0) {
					SwapCubes(lastCube, currentCube);
					StartCoroutine(ShakeCube (lastCube));
					StartCoroutine(ShakeCube (currentCube));
				}else {
					freeMoves--;
					uIManager.UpdateText (uIManager.freeMovesText, "Free Moves: "+freeMoves);
				}
			}
			lastCube.ToggleSelector ();
			currentCube.ToggleSelector ();
			lastCube = null;
		}else{
			lastCube.ToggleSelector ();
			lastCube = currentCube;
		}
	}

	void SwapCubes(Cube cube1, Cube cube2){
		Vector3 cube1Start = cube1.transform.position;
		Vector3 cube2Start = cube2.transform.position;
		cube1.transform.position = cube2Start;
		cube2.transform.position = cube1Start;
	}
	IEnumerator ShakeCube(Cube cube){
		float counter = 0f;
		Vector3 cubePos = cube.transform.localPosition;
		float maxX = 0.07f;
		float maxY = 0.07f;
		float maxZ = 0.07f;
		float ShakeTime = 0.75f;
		lockInput = true;
		while(true){
			counter += Time.deltaTime;
			if(counter >= ShakeTime){
				cube.transform.localPosition = cubePos;
				lockInput = false;
				yield break;
			} else {
				cube.transform.localPosition = cubePos + new Vector3((ShakeTime - counter) * Random.Range(-maxX, maxX), 
							                           (ShakeTime - counter) * Random.Range(-maxY, maxY), 
							                           (ShakeTime - counter) * Random.Range(-maxZ, maxZ));
			}
			yield return null;
		}
	}

	void CheckForMatches(){

		//CreatePlayableArea ();

		foreach(Cube cube in seenCubes){

			if (cube != null){
				CheckInDirection(cube, Vector3.up);
				FinalizeMatches ();

				CheckInDirection(cube, Vector3.right);
				FinalizeMatches ();

//				CheckInDirection(cube, Vector3.down);
//				FinalizeMatches ();
//
//				CheckInDirection(cube, Vector3.left);
//				FinalizeMatches ();

			}
		}

		if (matchedList.Count > 0){
			DestroyMatchedCubes ();
			matchTimer = 0.75f;
		}else isMatching = false;
		print("checked for matches");
	}

	void FinalizeMatches (){
		if(numberOfCubesMatched == 3){

			freeMoves++;
			uIManager.UpdateText (uIManager.freeMovesText, "Free Moves: "+freeMoves);
			numberOfCubesMatched = 1;
		} else if(numberOfCubesMatched == 4){
			freeMoves--;
			uIManager.UpdateText (uIManager.freeMovesText, "Free Moves: "+freeMoves);
		}
		numberOfCubesMatched = 1;
		if (matchMaking.Count >= amountToMatch) {
			matchFound = true;

			foreach (Cube c in matchMaking) {
				if (!matchedList.Contains (c)) {
					matchedList.Add (c);
				}
			}
		}
		matchMaking.Clear ();
	}

	void DestroyMatchedCubes(){
		foreach(Cube cube in matchedList){
			cuboid.allCubes.Remove (cube);
			cube.DestroyCubeFX ();
			Destroy (cube.gameObject);
		}
		matchedList.Clear();
	}


	void CheckInDirection(Cube cube, Vector3 direction){
		if(!matchMaking.Contains(cube)){
			matchMaking.Add (cube);
		}
		Vector3 sight = new Vector3(cube.transform.position.x + direction.x, cube.transform.position.y + direction.y, -10);
		Vector3 dir = (cube.transform.position + direction) - sight;
		RaycastHit hit;

		if(Physics.Raycast (sight, dir, out hit, 1000f) 
		   && new Vector3(Mathf.Abs(hit.transform.position.x), Mathf.Abs(hit.transform.position.y), 0) 
		   == new Vector3(Mathf.Abs(cube.transform.position.x + direction.x), Mathf.Abs(cube.transform.position.y + direction.y), 0)){

			Cube nextCube = hit.collider.GetComponent<Cube>();
			if (nextCube == null) return;
			else if (nextCube.myColor != cube.myColor) return;
			else if (matchMaking.Contains(nextCube)) return;
			else {
				matchMaking.Add (nextCube);
				numberOfCubesMatched++;
			}
			CheckInDirection(nextCube, direction);
		}
	}
	
	public void CreatePlayableArea(){
		seenCubes.Clear ();
		foreach(Cube c in cuboid.allCubes){
			if (c != null){
				Vector3 sight = new Vector3(c.transform.position.x, c.transform.position.y, -10);
				Vector3 dir = c.transform.position - sight;
				RaycastHit hit;
				if(Physics.Raycast (sight, dir, out hit, 10000f) && hit.transform == c.transform){
					seenCubes.Add (c);
				}
			}
		}
		isMatching = true;
		matchFound = false;
		print("created play area");
		print(seenCubes.Count);
	}

	public void DeselectCube(){
		if (lastCube != null) {
			lastCube.ToggleSelector ();
			lastCube = null;
			print("deselcted");
		}
	}
}

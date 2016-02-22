/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCuboid : MonoBehaviour {
	
	public List<Cube> cubes = new List<Cube>();
	public GameObject cubePrefab;
	public Cube lastCube;
	public float width = 4;
	public float height = 4;
	public float depth = 4;
	Vector3 cube1Start, cube1End, cube2Start, cube2End;
	public bool isSwapping = false;
	Cube cube1, cube2;
	//float moveTime = 5f;
	public int amountToMatch = 3;
	int checkingForMatches = 2;
	public bool settingBoard = true;
	public List<Cube> seenCubes = new List<Cube>();
	public List<Vector3> seenCubePos = new List<Vector3>();
	
	
	// Use this for initialization
	void Start () {
		
		for(float x = 0; x < width; x++){
			
			for(float y = 0; y < height; y++){
				
				for(float z = 0; z < depth; z++){
					GameObject cube = Instantiate(cubePrefab,new Vector3(x-width/2+0.5f, y-height/2+0.5f, z-depth/2+0.5f), Quaternion.identity)as GameObject;
					cube.transform.parent = gameObject.transform;
					cubes.Add(cube.GetComponent<Cube>());
				}
			}
		}
	}
	
	void Update(){
		if (isSwapping){
			moveCube (cube1, cube1End, cube1Start);
			moveCube (cube2, cube2End, cube2Start);
			if (Vector3.Distance(cube1.transform.position, cube1End) < 0.1f || 
			    Vector3.Distance(cube2.transform.position, cube2End) < 0.1f){
				cube1.transform.position = cube1Start;
				cube2.transform.position = cube2Start;
				cube1.SwapColor (cube1, cube2, cube1.myColor, cube2.myColor);
				cube1.ToggleSelector ();
				cube2.ToggleSelector ();
				lastCube = null;
				isSwapping = false;
				checkingForMatches = 2;
			}
		}
		if(checkingForMatches >= 0){
			CreatePlayableArea();
			foreach(Cube c in cubes){
				if (c != null){
					CheckForNeabyMatches (c);
				}
			}
			checkingForMatches--;
		}
	}
	
	
	void CheckMatch(){
		List<Cube> cube1List = new List<Cube>();
		List<Cube> cube2List = new List<Cube>();
		ConstructMatchList (cube1.myColor, cube1, cube1.xCoord, cube1.yCoord, cube1.zCoord, ref cube1List);
		FixMatchList (cube1, cube1List);
		ConstructMatchList (cube2.myColor, cube2, cube2.xCoord, cube2.yCoord, cube2.zCoord, ref cube2List);
		FixMatchList (cube2, cube2List);
	}
	
	void FixMatchList(Cube cube, List<Cube> listToFix){
		List<Cube> xMatch = new List<Cube>();
		List<Cube> yMatch = new List<Cube>();
		List<Cube> zMatch = new List<Cube>();
		
		for(int i = 0; i < listToFix.Count; i++){
			if(cube.xCoord == listToFix[i].xCoord){
				xMatch.Add (listToFix[i]);
			}
			else if (cube.yCoord == listToFix[i].yCoord){
				yMatch.Add (listToFix[i]);
			}
			else if(cube.zCoord == listToFix[i].zCoord){
				zMatch.Add (listToFix[i]);
			}
		}
		
		if(xMatch.Count >= amountToMatch){
			for(int i = 0; i < xMatch.Count; i++){
				xMatch[i].isMatched = true;
			}
		}
		if(yMatch.Count >= amountToMatch){
			for(int i = 0; i < yMatch.Count; i++){
				yMatch[i].isMatched = true;
			}
		}
		if(zMatch.Count >= amountToMatch){
			for(int i = 0; i < zMatch.Count; i++){
				zMatch[i].isMatched = true;
			}
		}
	}
	
	void CheckForNeabyMatches(Cube c){
		List<Cube> nearbyCubeList = new List<Cube>();
		ConstructMatchList (c.myColor, c, c.xCoord, c.yCoord, c.zCoord, ref nearbyCubeList);
		FixMatchList (c, nearbyCubeList);
	}
	
	void ConstructMatchList(Color color, Cube cube, int xCoord, int yCoord, int zCoord, ref List<Cube> matchList){
		if(cube == null){
			return;
		}else if(cube.myColor != color){
			return;
		}else if(matchList.Contains(cube)){
			return;
		} else{
			matchList.Add (cube);
		}
		
		if (xCoord == cube.xCoord || yCoord == cube.yCoord || zCoord == cube.zCoord){
			foreach(Cube c in cube.neighbors){
				ConstructMatchList (color, c, xCoord, yCoord, zCoord, ref matchList);
			}
		}
	}
	
	void moveCube(Cube cubeToMove, Vector3 toPos, Vector3 fromPos){
		//cubeToMove.transform.position = Vector3.Slerp (fromPos, toPos, Time.deltaTime*moveTime);
		cubeToMove.transform.position = toPos;
		
	}
	
	
	public void SwapCube(Cube currentCube){
		if (lastCube == null){
			lastCube = currentCube;
		}else if(lastCube == currentCube){
			lastCube = null;
		}else{
			if(lastCube.IsNeighborsWith(currentCube)){
				isSwapping = true;
				cube1 = lastCube;
				cube2 = currentCube;
				cube1Start = lastCube.transform.position;
				cube1End = currentCube.transform.position;
				cube2Start = currentCube.transform.position;
				cube2End = lastCube.transform.position;
				
			}else{
				lastCube.ToggleSelector ();
				lastCube = currentCube;
			}
		}
	}
	public void CreatePlayableArea(){
		seenCubes.Clear ();
		seenCubePos.Clear ();
		foreach(Cube c in cubes){
			if (c != null){
				Camera camera = FindObjectOfType<Camera>();
				Vector3 dir = c.transform.position - camera.transform.position;
				RaycastHit hit;
				if(Physics.Raycast (camera.transform.position, dir, out hit, 300f) && hit.transform == c.transform){
					seenCubes.Add (c);
					print (seenCubes.Count);
				}
			}
		}
		for(int i = 0; i < seenCubes.Count; i++){
			Cube c = seenCubes[i];
			seenCubePos.Add(c.transform.position);
			c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, -2f);
		}
	}
	public void ResetSeenCubesPos(){
		for(int i = 0; i < seenCubes.Count; i++){
			if (seenCubes[i] != null){
				seenCubes[i].transform.position = seenCubePos[i];
				print ("resetting positions");
			}
		}
	}
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayGameLogic : MonoBehaviour {

	public ArrayCubes[,] playableCubes;
	public ArrayCubes deadCube;

	Create create;
	CubeFaces cFace;

	// Use this for initialization
	void Start () {
		create = FindObjectOfType<Create>();
		cFace = new CubeFaces();
		//SetPlayableArray();

	}
	
	// Update is called once per frame
	void Update () {
		

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			cFace.RotLeft();
			SetPlayableArray();
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			cFace.RotRight();
			SetPlayableArray();
			}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			cFace.RotUp();
			SetPlayableArray();
			}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			cFace.RotDown();
			SetPlayableArray();
		}

	}

	void CheckForMatches(Cube c, Vector3 dir){
		for(int i = 0; i < playableCubes.Length; i++){
			
		}
	}

	void SetPlayableArray(){
		playableCubes = new ArrayCubes[create.widthX,create.heightY];
		for(int i = 0; i < create.widthX; i++){
			for(int j = 0; j < create.heightY; j++){
				int k = 0;

				Vector3 V = ReOrderijk(i,j,k);
				//print(V);
				int vX = (int)V.x;
				int vY = (int)V.y;
				int vZ = (int)V.z;

				ArrayCubes c = create.cubeArray[vX, vY, vZ];

				while (!(playableCubes [i, j] == c || playableCubes [i, j] == deadCube)) {
					c = create.cubeArray[vX, vY, vZ];

					if (c.active) {
						playableCubes [i, j] = c;
					}else if(vZ < create.depthZ){
						vZ += cFace.ZDirection();
						print(vZ);
					}
					if(vZ >= create.depthZ || vZ < 0){
						// Dead square
						playableCubes [i, j] = deadCube;
					}
				}
			}
		}
		print(playableCubes[0,0].x+", "+playableCubes[0,0].y+", "+playableCubes[0,0].z);
	}
	Vector3 ReOrderijk(int i, int j, int k){
		switch(cFace.cameraSide){
		case 1: {return new Vector3(create.widthX-1 - k , j, i);}
		case 2: {return new Vector3(k, j, i);}
		case 3: {return new Vector3(i, create.heightY-1 - k, j);}
		case 4: {return new Vector3(i, k, j);}
		case 5: {return new Vector3(i, k, create.depthZ-1 - k);}
		case 6: {return new Vector3(i, j, k);}
		default: {
				Debug.LogError("Cube face outside of range");
				break;
			}
		}
		return new Vector3(i, j, k);
	}
}

class CubeFaces {

	//Sides are fixed in virtual space, the number tell the game logic what face the camera is looking at.

	public int sideA = 1;
	public int sideB = 2;
	public int sideC = 3;
	public int sideD = 4;
	public int sideE = 5;
	public int sideF = 6;

	int faceA = 1;
	int faceB = 2;
	int faceC = 3;
	int faceD = 4;
	int faceE = 5;
	int faceF = 6;

	public int cameraSide = 6;

	public void RotRight(){
		sideA = faceF;
		sideB = faceE;
		sideE = faceA;
		sideF = faceB;
		LockCubeSides();
	}
	public void RotLeft(){
		sideA = faceE;
		sideB = faceF;
		sideE = faceB;
		sideF = faceA;
		LockCubeSides();
	}
	public void RotUp(){
		sideC = faceF;
		sideD = faceE;
		sideE = faceC;
		sideF = faceD;
		LockCubeSides();
	}
	public void RotDown(){
		sideC = faceE;
		sideD = faceF;
		sideE = faceD;
		sideF = faceC;
		LockCubeSides();
	}
	void LockCubeSides(){
		faceA = sideA;
		faceB = sideB;
		faceC = sideC;
		faceD = sideD;
		faceE = sideE;
		faceF = sideF;
		cameraSide = sideF;
	}
	public int ZDirection(){
		if((cameraSide % 2) == 0){
			return 1;
		}else return -1;
	}
}

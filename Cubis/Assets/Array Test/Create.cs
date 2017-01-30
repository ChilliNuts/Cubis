using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour {

	public ArrayCubes[,,] cubeArray; 
	Vector3[] cubePositions;
	public ArrayCubes cubePrefab;
	public Color[] randomColorSeed;

	public int widthX = 3;
	public int heightY = 3;
	public int depthZ = 3;

	void Awake(){
		cubePositions = new Vector3[widthX*heightY*depthZ];
		cubeArray = new ArrayCubes[widthX, heightY, depthZ];

		for(int x = 0; x < widthX; x++){

			for(int y = 0; y < heightY; y++){

				for(int z = 0; z < depthZ; z++){
					cubePositions[z] = new Vector3(x,y,z);
					ArrayCubes cube = Instantiate(cubePrefab,new Vector3(x-widthX/2+0.5f, y-heightY/2+0.5f, z-depthZ/2+0.5f), Quaternion.identity);
					cube.gameObject.transform.parent = gameObject.transform;
					cube.x = x;
					cube.y = y;
					cube.z = z;
					cubeArray[x,y,z] = cube;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

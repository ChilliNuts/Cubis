using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCuboid : MonoBehaviour {

	public List<Cube> allCubes = new List<Cube>();
	public GameObject cubePrefab;

	public float width = 3;
	public float height = 3;
	public float depth = 3;

	private Transform parent;



	// Use this for initialization
	void Awake () {
		parent = transform.parent;
		transform.localScale = new Vector3(width,height,depth);

		for(float x = 0; x < width; x++){

			for(float y = 0; y < height; y++){
			
				for(float z = 0; z < depth; z++){
					GameObject cube = Instantiate(cubePrefab,new Vector3(x-width/2+0.5f, y-height/2+0.5f, z-depth/2+0.5f), Quaternion.identity)as GameObject;
					cube.transform.parent = gameObject.transform;
					allCubes.Add(cube.GetComponent<Cube>());
					}
				}
			}
	}

	public void ToggleParent(){
		if(transform.parent == null){
			transform.parent = parent;
		} else if(transform.parent == parent){
			transform.parent = null;
		}
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class ArrayCubes : MonoBehaviour {

	public int x;
	public int y;
	public int z;
	public Color myColor;
	public bool active;

	MeshRenderer myskin;
	Create create;


	// Use this for initialization
	void Start () {
		myskin = GetComponent<MeshRenderer>();
		create = FindObjectOfType<Create>();
		SetRandomColor();
	}
	
	// Update is called once per frame
	void Update () {
		if(!active){
			myskin.enabled = false;
			GetComponent<BoxCollider>().enabled = false;
		}
	}

	void OnMouseOver(){
		if (Input.GetMouseButtonUp(0)) {
			active = false;
			//ToggleSelector ();
			//playManager.SelectCube (this);
		}
	}

	void SetRandomColor(){
		myColor = create.randomColorSeed[Random.Range(0, create.randomColorSeed.Length)];
		myskin.material.color = myColor;
	}
}

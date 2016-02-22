using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {

	public List<Color> colors = new List<Color>();
	public Color myColor;
	public GameObject selector;
	public bool isSelected = false;
	public GameObject destroyFX;
	public bool isSpecial;

	PlayManager playManager;
	List<Cube> neighbors = new List<Cube>();
	


	void Start(){
		playManager = FindObjectOfType<PlayManager>();
		SetColor ();
	}
	
	void OnMouseOver(){
		if (!PlayManager.lockInput && Input.GetMouseButtonUp(0)) {
			ToggleSelector ();
			playManager.SelectCube (this);
		}
	}

	public bool IsNeighborsWith(Cube lastCube, Cube currentCube){
		CheckForNeighbor(lastCube, currentCube, Vector3.up);
		CheckForNeighbor(lastCube, currentCube, Vector3.down);
		CheckForNeighbor(lastCube, currentCube, Vector3.left);
		CheckForNeighbor(lastCube, currentCube, Vector3.right);
		if(neighbors.Contains (currentCube)){
			neighbors.Clear ();
			Debug.Log ("is neighbor");
			return true;
		}else{
			neighbors.Clear ();
			return false;
		}
	}
	
	public void ToggleSelector(){
		isSelected = !isSelected;
		selector.SetActive (isSelected);
	}

	public void SetColor(){
		this.gameObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Count)];
		myColor = this.gameObject.GetComponent<Renderer>().material.color;
	}

	public void DestroyCubeFX(){
		Instantiate(destroyFX, transform.position, Quaternion.identity);

	}

	void CheckForNeighbor(Cube lastCube, Cube currentCube, Vector3 direction){
		Vector3 sight = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y, -10);
		Vector3 dir = (lastCube.transform.position + direction) - sight;
		RaycastHit hit;

		if(Physics.Raycast (sight, dir, out hit, 1000f) 
		   && new Vector3(Mathf.Abs(hit.transform.position.x), Mathf.Abs(hit.transform.position.y), 0) 
		   == new Vector3(Mathf.Abs(currentCube.transform.position.x), Mathf.Abs(currentCube.transform.position.y), 0)){
			neighbors.Add (hit.collider.GetComponent<Cube>());

		}
	}
	void SpecialDesrtuct1(){

	}
}

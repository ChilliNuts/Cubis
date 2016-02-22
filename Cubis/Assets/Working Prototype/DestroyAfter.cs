using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

	public float destroyAfter;
	
	// Update is called once per frame
	void Update () {
		Invoke ("Destroy", destroyAfter);
	}
	void Destroy(){
		Destroy (gameObject);
	}
}

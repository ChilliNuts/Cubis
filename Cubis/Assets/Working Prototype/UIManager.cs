using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public Text freeMovesText;
	

	public void UpdateText(Text text, string newString){
		text.text = newString;
	}
}

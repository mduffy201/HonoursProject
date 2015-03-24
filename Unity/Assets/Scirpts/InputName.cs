using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputName : MonoBehaviour {

	string username;
	InputField inputField;
	Text text;
	TitleContols titlecontrols;
	// Use this for initialization
	void Start () {
		inputField = gameObject.GetComponent<InputField> ();
		titlecontrols = GameObject.Find ("TitleLogic").GetComponent<TitleContols>();
	}
	
	// Update is called once per frame
	void Update () {
		username = inputField.text;
		titlecontrols.username = username;
	}
}

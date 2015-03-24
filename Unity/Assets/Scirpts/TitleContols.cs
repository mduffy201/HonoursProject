using UnityEngine;
using System.Collections;


public class TitleContols : MonoBehaviour {

	//public GameObject canvas;

	private UnityEngine.UI.InputField inputField;
	private int toolbarInt = 0;
	private string[] toolbarStrings = {"EASY","NORMAL","HARD"};


	public string username;


	void OnGUI() {

		toolbarInt = GUI.Toolbar (new Rect(Screen.width/2-170, Screen.height/2-50, 250, 50), toolbarInt, toolbarStrings);

	}
	public void StartGame(){
		PlayerPrefs.SetString("name",username);
		PlayerPrefs.SetInt("difficulty", toolbarInt);
		Debug.Log("Space Down");
		Application.LoadLevel("Level");
	}
}

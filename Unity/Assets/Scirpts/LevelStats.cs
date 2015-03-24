using UnityEngine;
using System.Collections;

public class LevelStats :MonoBehaviour {

	private string db_url = "http://localhost/GameTest/";
	//private string db_url = "http://192.168.1.10/GameTest/";
	//Player data
	public string name;
	public int difficulty;


	//Level Data
	public string Axiom = "AAAA";
	public int gap_max_length = 5;
	public float enemySpeed = 2.0f;


	//Constructor
	//Upload to Database
	public int jumps = 0;
	public int deaths = 0;
	public int kills = 0;

	public Time level_time;
	public float time_moving_right;
	public float time_moving_left;

	private void Awake(){
		name = PlayerPrefs.GetString ("name");
		difficulty = PlayerPrefs.GetInt ("difficulty");
		DontDestroyOnLoad (this);
	}
	private void Start(){
		//jumps = 0;
	
	}

	private void Update(){
		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log("S DOWN");
			SaveData();
				}
		//Debug.Log("JUMPS: " + jumps.ToString());
		//Debug.Log("Time since load: " +  Time.timeSinceLevelLoad.ToString());
		//Debug.Log("Time moving right: " +  time_moving_right.ToString());
	}
	public void SaveData(){
		StartCoroutine (SaveDatatoDB ());
	}
	IEnumerator SaveDatatoDB(){
		Debug.Log ("SENDING FORM");
		WWWForm form = new WWWForm ();
		form.AddField("name", name);
		form.AddField ("axiom", Axiom);

		WWW webRequest = new WWW (db_url + "SaveLevel.php", form);

		yield return webRequest;
		Debug.Log (webRequest.text.ToString());
	}
}

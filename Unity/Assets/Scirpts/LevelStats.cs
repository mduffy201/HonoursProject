using UnityEngine;
using System.Collections;

public class LevelStats :MonoBehaviour {

	private string db_url = "http://localhost/GameTest";
	//Player data
	public string name = "Mark";
	//Level Data
	public string Axiom = "AAAA";
	//Constructor
	//Upload to Database


	public void SaveData(){
		StartCoroutine (SaveDatatoDB ());
	}
	IEnumerator SaveDatatoDB(){
		WWWForm form = new WWWForm ();
		form.AddField("name", name);
		form.AddField ("axiom", Axiom);

		WWW webRequest = new WWW (db_url + "SaveLevel.php", form);

		yield return webRequest;
	}
}

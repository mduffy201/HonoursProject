using UnityEngine;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log (collider.name);
		if (collider.name == "EndTrigger") {
						Debug.Log ("Level Finish Triggered");
			Application.LoadLevel("TitleScreen");
				}
		if (collider.name == "EnemyKillCheck") {
			Debug.Log ("Enemy Killed by head");

			Destroy(collider.gameObject.transform.parent.gameObject);

			//Application.LoadLevel("TitleScreen");

		}
		if (collider.name == "EdgeCheckLeft" || collider.name == "EdgeCheckRight") {
			Debug.Log("Player Dead");		
		
		}
		}

}

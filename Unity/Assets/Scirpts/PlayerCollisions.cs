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
		if (collider.name == "EndTrigger") {
						Debug.Log ("Level Finish Triggered");
			Application.LoadLevel("TitleScreen");
				}
		}
}

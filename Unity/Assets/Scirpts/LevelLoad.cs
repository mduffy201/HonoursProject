using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {

	public GameObject player_spawn;
	public GameObject player;

	void Awake(){
		player_spawn = GameObject.Find ("PlayerSpawn");
		player = (GameObject)Instantiate((GameObject)Resources.Load("Player/Player"), player_spawn.transform.position, Quaternion.identity);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

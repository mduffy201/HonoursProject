using UnityEngine;
using System.Collections;

public class EnemyLoader : MonoBehaviour {

	
	public GameObject enemy_spawn;
	public GameObject enemy;
	
	void Awake(){
		enemy_spawn = GameObject.Find ("EnemySpawn");
		enemy = (GameObject)Instantiate((GameObject)Resources.Load("Enemy/Enemy"), enemy_spawn.transform.position, Quaternion.identity);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class EnemySpawnScript : MonoBehaviour {

//Attached to enemy spawn object
int number;
int cooldown;
//types
int power;
int speed;
	
	//public GameObject enemy_spawn;
	public GameObject enemy;
	
	void Awake(){

		//enemy_spawn = (GameObject)Resources.Load ("Enemy/EnemySpawn");
		enemy = (GameObject)Instantiate((GameObject)Resources.Load("Enemy/Enemy"), gameObject.transform.position, Quaternion.identity);
		enemy.GetComponent<Enemy> ().SetType (Enemy.EnemyType.Shooter);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

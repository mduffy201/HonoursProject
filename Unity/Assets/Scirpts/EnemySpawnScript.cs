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
	public GameObject player;
	public float p_dist;

	void Awake(){

		player = GameObject.FindGameObjectWithTag ("Player");
	
	}

	private void Spawn(){
		enemy = (GameObject)Instantiate((GameObject)Resources.Load("Enemy/Enemy"), gameObject.transform.position, Quaternion.identity);
		enemy.GetComponent<Enemy> ().SetType (Enemy.EnemyType.Walker);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		p_dist=  transform.position.x - player.transform.position.x;
		if (transform.position.x - player.transform.position.x < 5) {
			Spawn ();		
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemySpawnScript : MonoBehaviour {

//Attached to enemy spawn object
public int numberOf;
public int cooldown;
//types
int power;
public float speed;
	
	//public GameObject enemy_spawn;
	public GameObject enemy;
	public GameObject player;
	public float p_dist;

	void Awake(){

		player = GameObject.FindGameObjectWithTag ("Player");
	
	}

	private void Spawn(){
		Debug.Log ("FOUND SPEED spawner " + speed.ToString ());
		enemy = (GameObject)Instantiate((GameObject)Resources.Load("Enemy/Enemy"), gameObject.transform.position, Quaternion.identity);
		enemy.GetComponent<Enemy> ().SetType (Enemy.EnemyType.Walker);
		enemy.GetComponent<Enemy> ().speed = speed;
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

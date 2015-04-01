using UnityEngine;
using System.Collections;

public class EnemySpawnScript : MonoBehaviour
{

//Attached to enemy spawn object
		public int numberOf;
		public int cooldown;

//types
		//int power;
		//public float speed;
	private float spawn_range = 10.0f;
		public Enemy.EnemyType type;
		//public GameObject enemy_spawn;
		private GameObject enemy;
	private GameObject player;
	private float p_dist;

		void Awake ()
		{
		//Debug.Log ("Enemy type: " + type.ToString());
				player = GameObject.FindGameObjectWithTag ("Player");
				//type = Enemy.EnemyType.Walker;
		}
		// Update is called once per frame
		void Update ()
		{
		//Debug.Log ("Enemy type: " + type.ToString());
				p_dist = transform.position.x - player.transform.position.x;
				if (transform.position.x - player.transform.position.x < spawn_range) {
						Spawn ();		
				}
		}

		private void Spawn ()
		{

				enemy = (GameObject)Instantiate ((GameObject)Resources.Load ("Enemy/Enemy"), gameObject.transform.position, Quaternion.identity);
				enemy.GetComponent<Enemy> ().SetType (type);
				Destroy (gameObject);
		}
	
		
}

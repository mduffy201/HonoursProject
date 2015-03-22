using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

		//Get spawner game object
		//Get script attached
		EnemySpawnScript es;
		public GameObject enemy_spawn;
		private GameObject enemySpawnPoint;
		private Tile[,] levelMap;
		public float start_range = 10.0f;
	public float spawn_inverse_frequency = 5.0f;

		void Awake ()
		{
				//Debug.Log ("Enemy loader awake");
				enemy_spawn = (GameObject)Resources.Load ("Enemy/EnemySpawn");
				
		}

		// Use this for initialization
		void Start ()
		{
	
		}

		public void LoadMap (Tile[,] levelIn)
		{
				levelMap = levelIn;
		}

		// Update is called once per frame
		void Update ()
		{
				/*if (Input.GetKeyDown (KeyCode.E)) {
		//find spawn tiles
			LoadSpawnPoints();*/
				// spawn
				//}

	
		}

		public void LoadEnemySpawns ()
		{
				LoadSpawnPoints ();
		}

		private void LoadSpawnPoints ()
		{
				float frequency_counter = spawn_inverse_frequency;

				int level_length = levelMap.GetLength (0);
				int level_height = levelMap.GetLength (1);

				for (int i = 0; i < level_length; i++) {
						for (int j = 0; j< level_height; j++) {
								if (levelMap [i, j].isEnemySpawn ()) {
							
										if (i > start_range && frequency_counter >= spawn_inverse_frequency) {		
												enemySpawnPoint = (GameObject)Instantiate (enemy_spawn, new Vector3 (levelMap [i, j].tilePos.x, levelMap [i, j].tilePos.y, 0.0f), Quaternion.identity);
						frequency_counter = 0;
					}			
					else{
						frequency_counter++;
					}
								}
						}
		
				}

		}
}

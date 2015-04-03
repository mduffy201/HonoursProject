using UnityEngine;
using System.Collections;

public class LevelLogic: MonoBehaviour
{

	//megaman - cutman
	//megman - bombsman
	//megman - gutsman
	//megaman - elecman
	//megaman - iceman
	//megman - fireman





		private LevelManager levelManager;
		private EnemyManager enemyManager;
		private PlayerManager playerManager;
		private Tile[,] levelMap;
		//public GameObject player_spawn;
		//public GameObject player;

		//Generation variables
		
		bool isTemporary = true;
		bool reStart = true;
		

		void Awake ()
		{
				if (GameObject.FindObjectsOfType (typeof(LevelLogic)).Length > 1 && isTemporary)
						Destroy (gameObject);
				else {
						isTemporary = false;
						DontDestroyOnLoad (gameObject);
				}
		}

		// Use this for initialization
		void Start ()
		{
		Debug.Log ("Level logic start");
				levelManager = GameObject.Find ("LevelLogic").GetComponent<LevelManager> ();
				enemyManager = GameObject.Find ("LevelLogic").GetComponent<EnemyManager> ();
				playerManager = GameObject.Find ("LevelLogic").GetComponent<PlayerManager> ();

				//Form level
				levelManager.InitLevelMap ();

				
				

				//Update Each Tile in map - includeds asigning values through CA
				

				levelManager.UpdateGapsAndPlatforms ();
				levelManager.UpdateLevelMap ();
				//Draw level
				levelMap = levelManager.GetLevelMap ();
				levelManager.DrawLevelMap ();

				//Get level map tile array
				//levelMap = levelManager.GetLevelMap ();



				enemyManager.LoadMap (levelMap);
				playerManager.LoadMap (levelMap);

						
				//Player must be loaded after level for raycasting to work
				playerManager.LoadPlayerSpawn ();
				playerManager.LoadEndPoint ();
				enemyManager.LoadEnemySpawns ();	

			
		}



		// Update is called once per frame
		void Update ()
		{
				if (GameObject.FindGameObjectWithTag ("Player") == null) {
					//	Debug.Log ("restart?");
						Start ();
				}
		}
}

using UnityEngine;
using System.Collections;

public class LevelLogic: MonoBehaviour
{


		private LevelManager levelManager;
		private EnemyManager enemyManager;
		private PlayerManager playerManager;
		private Tile[,] levelMap;
		public GameObject player_spawn;
		public GameObject player;

		//Generation variables
		public string Axiom = "AAAB";
		int gap_number = 10;
		int gap_average_length = 3;
		bool isTemporary = true;
		bool reStart = true;
		//int platform_number


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
				levelManager = GameObject.Find ("LevelLogic").GetComponent<LevelManager> ();
				enemyManager = GameObject.Find ("LevelLogic").GetComponent<EnemyManager> ();
				playerManager = GameObject.Find ("LevelLogic").GetComponent<PlayerManager> ();



				//Send level parameters for generation
				levelManager.InitGenValues (Axiom, gap_number, gap_average_length);

				//Form level
				levelManager.InitLevelMap ();

				//Draw level
				

				//Update Each Tile in map - includeds asigning values through CA
				levelManager.UpdateLevelMap ();	

				levelManager.UpdateGapsAndPlatforms ();
				levelManager.DrawLevelMap ();
				



				//Get level map tile array
				levelMap = levelManager.GetLevelMap ();



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
						Debug.Log ("restart?");
						Start ();
				}
		}
}

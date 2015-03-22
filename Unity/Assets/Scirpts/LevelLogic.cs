using UnityEngine;
using System.Collections;

public class LevelLogic: MonoBehaviour {


	private TileManager levelManager;
	private EnemyManager enemyManager;
	private PlayerManager playerManager;

	private Tile[,] levelMap;

	public GameObject player_spawn;
	public GameObject player;

	//Generation variables
	public string Axiom = "AAAB";
	int gap_number = 10;
	int gap_average_length = 3;

	//int platform_number

	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("LevelLogic").GetComponent<TileManager> ();
		enemyManager = GameObject.Find ("LevelLogic").GetComponent<EnemyManager> ();
		playerManager  = GameObject.Find ("LevelLogic").GetComponent<PlayerManager> ();



		//Send level parameters for generation
		levelManager.InitGenValues (Axiom, gap_number, gap_average_length);

		//Form level
		levelManager.InitLevelMap ();

		//Draw level
		levelManager.DrawLevelMap ();

		//Get level map tile array
		levelMap = levelManager.GetLevelMap();



		enemyManager.LoadMap (levelMap);
		playerManager.LoadMap (levelMap);

		levelManager.UpdateLevelMap();	
		levelManager.DrawLevelMap();
		levelManager.UpdateGaps();
		levelManager.UpdatePlatforms();
		playerManager.LoadPlayerSpawn();
		playerManager.LoadEndPoint();
	}

	//Player must be loaded last for raycasting to work






	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
						//find spawn tiles
			enemyManager.LoadEnemySpawns();		
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			levelManager.UpdateLevelMap();	
			levelManager.DrawLevelMap();
		
		}
		if(Input.GetKeyDown(KeyCode.P)){
			playerManager.LoadPlayerSpawn();
			playerManager.LoadEndPoint();
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			//levelManager.UpdateGaps();
			levelManager.UpdatePlatforms();
		
		}
			// spawn
	}
}

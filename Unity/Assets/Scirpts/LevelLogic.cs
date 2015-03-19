using UnityEngine;
using System.Collections;

public class LevelLogic: MonoBehaviour {


	private TileGrid levelManager;
	private EnemyLoader enemyLoader;

	private Tile[,] levelMap;

	public GameObject player_spawn;
	public GameObject player;

	//Generation variables
	public string Axiom = "AAAB";

	void Awake(){
		player_spawn = GameObject.Find ("PlayerSpawn");
		//player = (GameObject)Instantiate((GameObject)Resources.Load("Player/Player"), player_spawn.transform.position, Quaternion.identity);
	}

	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("LevelLogic").GetComponent<TileGrid> ();
		enemyLoader = GameObject.Find ("LevelLogic").GetComponent<EnemyLoader> ();

		levelManager.InitGenValues (Axiom);
		levelManager.InitLevelMap ();
		levelManager.DrawLevelMap ();
		levelMap = levelManager.GetLevelMap();
		enemyLoader.LoadMap (levelMap);
	}








	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
						//find spawn tiles
			enemyLoader.LoadEnemySpawns();		
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			levelManager.UpdateLevelMap();	
			levelManager.DrawLevelMap();
		
		}
			// spawn
	}
}

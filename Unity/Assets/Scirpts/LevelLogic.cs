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


		private AudioSource audio_source;
		private AudioClip music;
		private LevelManager levelManager;
		private EnemyManager enemyManager;
		private PlayerManager playerManager;
		//private LevelStats level_stats;
		private Tile[,] levelMap;
		//public GameObject player_spawn;
		//public GameObject player;

		//Generation variables
		
		bool isTemporary = true;
		bool reStart = true;
	public bool new_level = true;
		void Awake ()
		{
				audio_source = GetComponent<AudioSource> ();	

				//Debug.Log ("Level Logic Awake");
				if (GameObject.FindObjectsOfType (typeof(LevelLogic)).Length > 1 && isTemporary)
						Destroy (gameObject);
				else {
						isTemporary = false;
						DontDestroyOnLoad (gameObject);
				}
				//level_stats = gameObject.GetComponent<LevelStats> ();
		}

		private void SelectMusic ()
		{
				Random.seed = System.DateTime.Now.Second;

				int result = Random.Range (0, 6);
				//result = 0;
				switch (result) {
				case 0:
						music = (AudioClip)Resources.Load ("Music/megaman - cutman");
						break;
				case 1:
			music = (AudioClip)Resources.Load ("Music/megman - bombsman");
						break;
				case 2:
			music = (AudioClip)Resources.Load ("Music/megman - gutsman");
						break;
				case 3:
			music = (AudioClip)Resources.Load ("Music/megaman - elecman");
						break;
				case 4:
			music = (AudioClip)Resources.Load ("Music/megaman - iceman");
						break;
				case 5:
			music = (AudioClip)Resources.Load ("Music/megman - fireman");
						break;
				}

		//if(!audio_source.isPlaying){
			//music = (AudioClip)Resources.Load ("Music/megaman - cutman");
			audio_source.clip = music;
			audio_source.loop = true;
			audio_source.Play ();
		//}
		}
		// Use this for initialization
		public void LoadLevel ()
		{

		}

		void Start ()
		{
//		Debug.Log ("Level Logic Start");
		if (new_level) {
						SelectMusic ();
			new_level = false;
				}
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

		public void Reload ()
		{

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
						//Reload();
				}
		}
}

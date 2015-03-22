using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	GameObject player_spawn;
	GameObject end_trigger;
	private Tile[,] levelMap;
	private GameObject playerSpawnPoint;
	private GameObject endTriggerPoint;
	void Awake(){
		//Debug.Log ("Player Manager Awake");
		player_spawn = (GameObject)Resources.Load ("Player/PlayerSpawn");
		end_trigger = (GameObject)Resources.Load ("Triggers/EndTrigger");
	}

	public void LoadMap(Tile[,] levelIn){
		levelMap = levelIn;
	}

	public void LoadPlayerSpawn(){
		int level_length = levelMap.GetLength (0);
		int level_height = levelMap.GetLength (1);
		
		for (int i = 0; i < level_length; i++) {
			for (int j = 0; j< level_height; j++) {
				if (levelMap [i, j].isPlayerSpawn ()) {
					playerSpawnPoint = (GameObject)Instantiate (player_spawn, new Vector3 (levelMap [i, j].tilePos.x, levelMap [i, j].tilePos.y, 0.0f), Quaternion.identity);
				}
			}
			
		}
	}
	public void LoadEndPoint(){
		int level_length = levelMap.GetLength (0);
		int level_height = levelMap.GetLength (1);
		
		for (int i = 0; i < level_length; i++) {
			for (int j = 0; j< level_height; j++) {
				if (levelMap [i, j].isEndSpawn()) {
					endTriggerPoint = (GameObject)Instantiate (end_trigger, new Vector3 (levelMap [i, j].tilePos.x, levelMap [i, j].tilePos.y, 0.0f), Quaternion.identity);
				}
			}
			
		}
	}

}

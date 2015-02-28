using UnityEngine;
using System.Collections;

//using System;

public class TileGrid : MonoBehaviour
{

		// A vector2 that contains the x width and y height of the tile map
		public Vector2 worldSize;
	
		// Array containing data for all tiles within the world map
		public Tile[,] levelMap;
	
		// Object that we use to create our map container
		private GameObject tileParent;
		private string lSystem;
		int pattern_horizontal = 15;
		int pattern_vertical = 5;
		int pattern_per_section = 4;
		int level_sections = 4;
		int total_patterens;
		Tile[,] pattern;
		//Tile[][,] level_patterns; 
		Tile[,] full_level;



		// Use this for initialization
		void Start ()
		{

				total_patterens = level_sections * pattern_per_section;
				pattern = new Tile[pattern_horizontal, pattern_vertical];
				full_level = new Tile[pattern_horizontal * level_sections * pattern_per_section, pattern_vertical];
				//level_patterns = new Tile[total_patterens][pattern_horizontal, pattern_vertical];




				// Create an empty object with the name "Map" to act as the parent for our tiles 
				tileParent = new GameObject ();
				tileParent.name = "Map";
		
		
				// Set the length of the array to be equal to the vector2 world size
				levelMap = new Tile[(int)worldSize.x, (int)worldSize.y];
		
				// For each row of tile
				for (int i = 0; i < worldSize.x; i++) {
						// For each row, cycle through an entire column
						for (int j = 0; j < worldSize.y; j++) {
								// Now we combine the loops' indexes to make sure each slot has a null tile
								levelMap [i, j] = new Tile ();
								levelMap [i, j].tilePos = new Vector2 (i, j);
								levelMap [i, j].tileType = Tile.TileType.Sky;
						}
				}
				InitFullLevel ();
				CreateLSystemString ();
				GeneratePatternsUsingLSyem ();
				DrawFullMap ();
				LoadNeighbours ();






				//initTileMap ();
				//LoadNeibours ();
				//	LoadNeibours ();
				// Draw Generated Map
				//DrawTileMap ();
		
				// Just an example using GetTile
				print (GetTile (3, 2).tileType);
		}

		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.G)) {
						Debug.Log ("GENERATE");		
						UpdateTileMap ();
				}
		}

	
		// A method for getting data from a tile at a specific coordinate (isn't necessary, but it looks cleaner this way)
		public Tile GetTile (int xPos, int yPos)
		{
				// All this does is return a Tile from the worldMap array.
				// This could be done without the method with just the array, but this is quicker to write and looks cleaner
				return levelMap [xPos, yPos];
		}

		private void LoadNeighbours ()
		{
				int level_length = full_level.GetLength (0);
				int level_height = full_level.GetLength (1);
		
				//			Debug.Log ("Level length: " + level_length);

		int iLevel_length = level_length - 1;
		int iLevel_height = level_height - 1;

				//CORNER TILES
				//bottomleft
				full_level [0, 0].SetNeighbours (null, null, full_level [0, 1], full_level [1, 0], null, null, full_level [1, 1], null);
				//topleft
				full_level [0, iLevel_height].SetNeighbours (full_level [0, iLevel_height - 1], null, null, full_level [1, iLevel_height], null, null, null, full_level [1, iLevel_height - 1]);
				//bottomright
				full_level [iLevel_length, 0].SetNeighbours (null, full_level [iLevel_length - 1, 0], full_level [iLevel_length, 1], null, null, full_level [iLevel_length - 1, 1], null, null);
				//topright
				full_level [0, iLevel_height].SetNeighbours (full_level [iLevel_length, iLevel_height - 1], full_level [iLevel_length - 1, iLevel_height], null, null, full_level [iLevel_length - 1, iLevel_height - 1], null, null, null);

				



				//Go up each column, left to right
				for (int i =0; i < level_length; i++) {
						for (int j =0; j < level_height; j++) {

								//LEFT EDGE TILES			
								if (i == 0 && j != 0 && j != iLevel_height) {
									//Set non-edge tiles
									full_level [i, j].SetNeighbours (
										//Straight 
										full_level [i, j - 1], //0
										null,
										full_level [i, j + 1], //2
										full_level [i + 1, j], //3
										//Diagonals
										null,
										null,
										full_level [i + 1, j + 1], //6
										full_level [i + 1, j - 1] //7
										);
								} 
								//Right EDGE TILES
								else if (i == iLevel_length && j != 0 && j != iLevel_height) {
									full_level [i, j].SetNeighbours (
										//Straight 
										full_level [i, j - 1], //0
										full_level [i - 1, j], //1
										full_level [i, j + 1], //2
										null,
										//Diagonals
										full_level [i - 1, j - 1], //4
										full_level [i - 1, j + 1], //5
										null,
										null
										);
								}
								//TOP EDGE TILES
								else if (j == iLevel_height && i != 0 && i != iLevel_length) {
									full_level [i, j].SetNeighbours (
										//Straight 
										full_level [i, j - 1], //0
										full_level [i - 1, j], //1
										null,
										full_level [i + 1, j], //3
										//Diagonals
										full_level [i - 1, j - 1],//4
										null,
										null,
										full_level [i + 1, j - 1]//7
										);

								}
								//BOTTOM EDGE TILE
								else if (j == 0 && i != 0 && i != iLevel_length) {
									//Set non-edge tiles
									full_level [i, j].SetNeighbours (
										//Straight 
										null,
										full_level [i - 1, j], //1
										full_level [i, j + 1], //2
										full_level [i + 1, j],//3
										//Diagonals
										null,
										full_level [i - 1, j + 1], //5
										full_level [i + 1, j + 1], //6
										null
										);
								

				
				}else {
					//Eliminate corners
					if(i != 0 && i != iLevel_length && j != 0 && j != iLevel_height){
										//Set non-edge tiles
										full_level [i, j].SetNeighbours (
											//Straight 
											full_level [i, j - 1],
											full_level [i - 1, j],
											full_level [i, j + 1],
											full_level [i + 1, j],
											//Diagonals
											full_level [i - 1, j - 1],
											full_level [i - 1, j + 1],
											full_level [i + 1, j + 1],
											full_level [i + 1, j - 1]
										);
					}

								}
			

						}
				}
		}

		public void UpdateTileMap ()
		{
				int level_length = full_level.GetLength (0);
				int level_height = full_level.GetLength (1);
		

		
				//Go up each column, left to right
				for (int i =0; i < level_length; i++) {
						for (int j =0; j < level_height; j++) {
								//Update each tile
								full_level [i, j].UpdateTile ();
						}
				}		
				//re load neibours
				LoadNeighbours ();
				//Re draw
				DrawFullMap ();
		}

		
		//=======================================================================================================================
		private void InitFullLevel ()
		{
				Debug.Log ("LevelLength: " + full_level.GetLength (0));
				Debug.Log ("LevelHeight: " + full_level.GetLength (1));
				for (int i = 0; i < full_level.GetLength(0); i++) {
						// For each row, cycle through an entire column
						for (int j = 0; j < full_level.GetLength(1); j++) {
								// Now we combine the loops' indexes to make sure each slot has a null tile
								full_level [i, j] = new Tile ();
								full_level [i, j].tilePos = new Vector2 (i, j);
								full_level [i, j].tileType = Tile.TileType.Sky;
						}
				}
		}

		private void CreateLSystemString ()
		{
				string Axiom = "AAAB";
				string temp = "";
				string result = "";
				int iterations = 2;
				//rules { A= AB, B=BC, C=AA}

				temp = Axiom;
				for (int j = 0; j < iterations; j++) {
						result = "";
						for (int i = 0; i < temp.Length; i++) {

								switch (temp [i]) {
								case 'A':
										result += "AB";
										break;
								case 'B':
										result += "BC";
										break;
								case 'C':
										result += "AA";
										break;
								}
		
								//Debug.Log ("L-System: " + Axiom);
						}
						temp = result;

						//Debug.Log ("L-System: " + result);
						lSystem = result;
				}
		}

		private void GeneratePatternsUsingLSyem ()
		{

				string patternstring = lSystem;

				for (int i = 0; i < patternstring.Length; i++) {
				
						switch (patternstring [i]) {

						case 'A':
								GenAPattern (i + 1);


								break;
						case 'B':
								GenBPattern (i + 1);
								break;
						case 'C':
								GenCPattern (i + 1);
								break;

						}
				}
		}

		private void GenAPattern (int x_placement)
		{
				for (int i = 0; i < pattern_horizontal; i++) {
						int place = (x_placement - 1) * pattern_horizontal + i;
						int j = 0;
						Random.seed = (int)place;
						int random_value = (int)(Random.value * 10) * i;
						int tile_state = random_value % 10;
				
						//int place = (x_placement - 1) * pattern_horizontal + i;
					
						//Debug.Log ("Random value: " + random_value);			
						if (tile_state == 0 || tile_state == 1) {
								//						Debug.Log ("switched: " + Random.value * 100);
								full_level [place, j].SwitchState ();
						}
				}
		}

		private void GenBPattern (int x_placement)
		{
				for (int i = 0; i < pattern_horizontal; i++) {
						for (int j = 0; j < pattern_vertical; j++) {
								
								if (j == 0 || j == 2) {
										int place = (x_placement - 1) * pattern_horizontal + i + j;

										Random.seed = (int)place;
										int random_value = (int)(Random.value * 10) * i;
										int tile_state = random_value % 4;
			
										
										//Debug.Log ("Random value: " + random_value);			
										if (tile_state == 0 || tile_state == 1) {
												//						Debug.Log ("switched: " + Random.value * 100);
												full_level [place, j].SwitchState ();
										}
								}
						}
				}
		}

		private void GenCPattern (int x_placement)
		{
				for (int i = 0; i < pattern_horizontal; i++) {
						for (int j = 0; j < pattern_vertical; j++) {
								if (j == 0 || j == 2 || j == 4) {
										int place = (x_placement - 1) * pattern_horizontal + i + j;
								
										Random.seed = (int)place;
										int random_value = (int)(Random.value * 10) * i;
										int tile_state = random_value % 4;
			
						
			
										//Debug.Log ("Random value: " + random_value);			
										if (tile_state == 0 || tile_state == 1) {
												//						Debug.Log ("switched: " + Random.value * 100);
												full_level [place, j].SwitchState ();
										}
								}
						}
				}
		}

		private void DrawFullMap ()
		{

				// Loop structure is the same as the loops above ^^^
				for (int i = 0; i < full_level.GetLength(0); i++) {
						for (int j = 0; j < full_level.GetLength(1); j++) {
				
								if (full_level [i, j].state == 0) {
										full_level [i, j].tileType = Tile.TileType.Sky;
					
								} else if (full_level [i, j].state == 1) {
										full_level [i, j].tileType = Tile.TileType.Platform;
								}
				
				
				
								// Check to see if the tile type is null or not
								// If the tile is null, we know not to try to draw a sprite for it
								if (full_level [i, j].tileType != Tile.TileType.Null) {
										// If the tile isn't null, it must exist so let's do some stuff
										// We're going through the Resources folder, within the Main Map directory and finding a sprite that matches
										// the TYPE of tile that we've found. ***Make sure the name of the sprite matches the tile types***
										GameObject tileObject;
										// We're creating an instance of the sprite object that we find based on the tiletype compared to the object name, storing it in tileSprite
										tileObject = (GameObject)Instantiate (Resources.Load ("Tiles/Tile"), full_level [i, j].tilePos, Quaternion.identity);
										tileObject.GetComponent<TileObject> ().SetTile (full_level [i, j]);
										//bullet.GetComponent<Bullet>().SetDir(newdir)
										// Now set the newly created sprite's parent to be the map parent that we created earlier 
										tileObject.transform.parent = tileParent.transform;
								}
						}
				}

		}
		
}
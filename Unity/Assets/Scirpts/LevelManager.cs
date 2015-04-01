using UnityEngine;
using System.Collections;

//using System;

public class LevelManager : MonoBehaviour
{
		LevelStats level_stats;

		//Input for generation
		
		private string axiom;
		private int	gap_number;
		[SerializeField]
		int
				gap_number_Actual;
		[SerializeField]
		private float
				gap_average_length;
		[SerializeField]
		private float
				gap_average_length_Actual;
		[SerializeField]
		private int
				plaform_number_Actual;

		// Object that we use to create our map contai
		private GameObject tileParent;
		private string lSystem;
		private int pattern_horizontal = 15;
		private int pattern_vertical = 5;
		private int pattern_per_section = 4;
		private int level_sections = 4;
		private Tile[,] full_level;
		private GapPlatformManager gapPlaformManager;
		//private PlatformManager platformManager;
		
		void Awake ()
		{
				
				level_stats = gameObject.GetComponent<LevelStats> ();
				//Debug.Log (level_stats.Axiom);
		
		}

		public void UpdateGapsAndPlatforms ()
		{
				gapPlaformManager.LoadMap (full_level);

				//gapPlaformManager.findAverage ();
		

				gapPlaformManager.RemoveSingleSpaceGaps ();
				gapPlaformManager.DrillUp ();

				full_level = gapPlaformManager.GetUpdatedMap ();
				gapPlaformManager.LoadMap (full_level);
				gapPlaformManager.RegisterPlatforms ();
				gapPlaformManager.RemoveSingleSpacePlaforms ();

				full_level = gapPlaformManager.GetUpdatedMap ();
				gapPlaformManager.LoadMap (full_level);

				gapPlaformManager.ReduceToMax ();
				full_level = gapPlaformManager.GetUpdatedMap ();



				plaform_number_Actual = gapPlaformManager.GetTotalPlatforms ();
				gap_average_length_Actual = gapPlaformManager.gap_average;
				gap_number_Actual = gapPlaformManager.GetTotalGaps ();

				
				//Reduce number of gaps
				if (gap_number_Actual > level_stats.gap_number) {
				
						gapPlaformManager.ReduceGaps (level_stats.gap_number);
						full_level = gapPlaformManager.GetUpdatedMap ();
						plaform_number_Actual = gapPlaformManager.GetTotalPlatforms ();
						gap_average_length_Actual = gapPlaformManager.gap_average;
						gap_number_Actual = gapPlaformManager.GetTotalGaps ();
				}

				//Increase number of gaps
				if (gap_number_Actual < level_stats.gap_number) {
						gapPlaformManager.IncreaseGaps (level_stats.gap_number);
				}

				gap_average_length_Actual = gapPlaformManager.findAverage ();
				if (gap_average_length_Actual < gap_average_length) {
						Debug.Log ("incrase average! " + gap_average_length_Actual.ToString () +
								" < " + gap_average_length.ToString ());
						while (gap_average_length_Actual < gap_average_length) {
								Debug.Log ("Inside while");
								gapPlaformManager.IncreaseAverage ();
								gap_average_length_Actual = gapPlaformManager.findAverage ();
						}

				} else if (gap_average_length_Actual > gap_average_length) {

			Debug.Log ("reduce average! " + gap_average_length_Actual.ToString () +
			           " < " + gap_average_length.ToString ());

						while (gap_average_length_Actual > gap_average_length) {
								gapPlaformManager.ReduceAverage ();
								gap_average_length_Actual = gapPlaformManager.findAverage ();
				
						}
				}

				
				gap_average_length_Actual = gapPlaformManager.findAverage ();
				full_level = gapPlaformManager.GetUpdatedMap ();
		}

		
	
		//Get current level of tiles
		public Tile[,] GetLevelMap ()
		{
				return full_level;
		}

		// A method for getting data from a tile at a specific coordinate (isn't necessary, but it looks cleaner this way)
		public Tile GetTile (int xPos, int yPos)
		{
				// All this does is return a Tile from the worldMap array.
				// This could be done without the method with just the array, but this is quicker to write and looks cleaner
				return full_level [xPos, yPos];
		}

		private void InitGenValues ()
		{
				axiom = level_stats.Axiom;
				gap_number = level_stats.gap_number;
				gap_average_length = level_stats.gap_average_length;
		}
	
		//Create level
		public void InitLevelMap ()
		{
				InitGenValues ();
				gapPlaformManager = new GapPlatformManager ();
				//platformManager = new PlatformManager ();
		
				//total_patterens = level_sections * pattern_per_section;
				full_level = new Tile[pattern_horizontal * level_sections * pattern_per_section, pattern_vertical];
		
				// Create an empty object with the name "Map" to act as the parent for our tiles 
				tileParent = new GameObject ();
				tileParent.name = "Map";

				//	Debug.Log ("ERROR FIND 1");
				InitFullLevel ();
				//	Debug.Log ("ERROR FIND 2");
				CreateLSystemString ();
				//Debug.Log ("ERROR FIND 3");
				GeneratePatternsUsingLSyem ();
				//Debug.Log ("ERROR FIND 4");
				LoadNeighbours ();
				//DrawFullMap ();
		
		
		}
		//Initialise Array of tiles objects
		private void InitFullLevel ()
		{
				//Debug.Log ("LevelLength: " + full_level.GetLength (0));
				//Debug.Log ("LevelHeight: " + full_level.GetLength (1));
				for (int i = 0; i < full_level.GetLength(0); i++) {
						// For each row, cycle through an entire column
						for (int j = 0; j < full_level.GetLength(1); j++) {

								float x_pos = i * 0.5f;
								float y_pos = j * 1.0f;

								// Now we combine the loops' indexes to make sure each slot has a null tile
								full_level [i, j] = new Tile ();
								full_level [i, j].tilePos = new Vector2 (x_pos, y_pos);
								full_level [i, j].tileType = Tile.TileType.Sky;
								full_level [i, j].level_length = full_level.GetLength (0);
								full_level [i, j].level_height = full_level.GetLength (1);
								full_level [i, j].placeInArray = new Vector2 (i, j);
						}
				}
		}
		//Run update on each tile object in array
		public void UpdateLevelMap ()
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
		}


		//Draw level map
		public void DrawLevelMap ()
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
							
										GameObject tileObject;
										
										tileObject = (GameObject)Instantiate (Resources.Load ("Tiles/Tile"), full_level [i, j].tilePos, Quaternion.identity);
										tileObject.GetComponent<TileObject> ().SetTile (full_level [i, j]);
										tileObject.transform.parent = tileParent.transform;
								}
						}
				}

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
				full_level [iLevel_length, iLevel_height].SetNeighbours (full_level [iLevel_length, iLevel_height - 1], full_level [iLevel_length - 1, iLevel_height], null, null, full_level [iLevel_length - 1, iLevel_height - 1], null, null, null);
		
		
		
		
		
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
					
					
					
								} else {
										//Eliminate corners
										if (i != 0 && i != iLevel_length && j != 0 && j != iLevel_height) {
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
	
		//Function set - Construct level layout using l-system==============================
		private void CreateLSystemString ()
		{
		
				string temp = "";
				string result = "";
				int iterations = 2;
				//rules { A= AB, B=BC, C=AA}
		
				temp = axiom;
				for (int j = 0; j < iterations; j++) {
						result = "";
						for (int i = 0; i < temp.Length; i++) {
				
								switch (temp [i]) {
								case 'A':
										result += "AB";
										break;
								case 'B':
										result += "DC";
										break;
								case 'C':
										result += "BA";
										break;
								case 'D':
										result += "CD";
										break;
								}
						}
						temp = result;
						lSystem = result;
				}
		}
	
		private void GeneratePatternsUsingLSyem ()
		{
		
				string patternstring = lSystem;

//				Debug.Log ("ERROR FIND:" + patternstring);
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
						case 'D':
								GenDPattern (i + 1);
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
						int tile_state = random_value % 4;
			
						
						//	Debug.Log(tile_state.ToString());
						//Debug.Log ("Random value: " + random_value);
						//int tile_state = (int)Random.Range (0.0f, 9.0f);
						int pos = (x_placement - 1) * pattern_horizontal + i;
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
				
								if (j == 0 || j == 1) {
										
										int place = (x_placement - 1) * pattern_horizontal + i + j;
										Random.seed = (int)place;
										int random_value = (int)(Random.value * 10) * i;
										int tile_state = random_value % 4;

										//int tile_state = (int)Random.Range (0.0f, 9.0f);
										int pos = (x_placement - 1) * pattern_horizontal + i;
										//Debug.Log ("Random value: " + random_value);			
										if (tile_state == 0 || tile_state == 1) {
												//						Debug.Log ("switched: " + Random.value * 100);
												full_level [pos, j].SwitchState ();
										}
								}
						}
				}
		}
	
		private void GenCPattern (int x_placement)
		{
				for (int i = 0; i < pattern_horizontal; i++) {
						for (int j = 0; j < pattern_vertical; j++) {
								if (j == 0 || j == 1 || j == 2) {
										


										int seed = (x_placement - 1) * pattern_horizontal + i + j;
										Random.seed = (int)seed;
										int random_value = (int)(Random.value * 10) * i;
										int tile_state = random_value % 4;


										
										int pos = (x_placement - 1) * pattern_horizontal + i;

										if (tile_state == 0 || tile_state == 1) {
											
												full_level [pos, j].SwitchState ();
										}
								}
						}
				}
		}

		private void GenDPattern (int x_placement)
		{
//		Debug.Log ("GEN D");
				for (int i = 0; i < pattern_horizontal; i++) {
						for (int j = 0; j < pattern_vertical; j++) {
								if (j == 3 || j == 1) {
					
					
					
										int seed = (x_placement - 1) * pattern_horizontal + i + j;
										Random.seed = (int)seed;
										int random_value = (int)(Random.value * 10) * i;
										int tile_state = random_value % 4;
					
					
					
										int pos = (x_placement - 1) * pattern_horizontal + i;
					
										if (tile_state == 0 || tile_state == 1) {
						
												full_level [pos, j].SwitchState ();
										}
								}
						}
				}
		}
		//===================================================================
		
}
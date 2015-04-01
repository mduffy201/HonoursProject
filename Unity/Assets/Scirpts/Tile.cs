using UnityEngine;
using System.Collections;

public class Tile
{
		private int alive = 1;
		const int dead = 0;
		public bool isSpriteSet = false;
		// Create an enum that will contain each possible tile type
		public enum TileType
		{
				Platform,
				Sky,
				Start,
				Finish,
				Null // Null is required for my generation method
		}

		private enum Direction
		{
				Down,
				Left,
				Up,
				Right,
				Down_Left,
				Up_Left,
				Up_Right,
				Down_Right
		
		}

		public enum PlatformPos
		{
				Middle,
				Center,
				Right,
				Left
		}

		public enum Neighbour
		{
				Down,
				Left,
				Up,
				Right
		}
		private Tile[] tile_neighbours;
		public int state = 0;
		public bool edge = false;
		// Create a variable that will hold the tile type for each tile that is created
		// We can then check each tile to see if we can walk on it or if something else should happen
		public TileType tileType;
		public PlatformPos platformPos;

		// A variable that will hold the current position of a tile
		public Vector2 tilePos;
		public Vector2 placeInArray;
		public int level_length;
		public int level_height;
		//Can an enemy spawn on this tile?
		private bool enemySpawn;
		//Can the Player spawn on this tile
		private bool playerSpawn;
		//Can level end spawn here
		private bool endPoint;
		private bool atEdge;
		// Basic Empty constructor that sets the default type to null so that they will not be created
		public Tile ()
		{

				tileType = TileType.Null;
				platformPos = PlatformPos.Middle;
				tile_neighbours = new Tile [8];
				enemySpawn = false;
				playerSpawn = false;
				atEdge = false;
				endPoint = false;
				level_length = 0;
				level_height = 0;
				placeInArray = new Vector2 (0, 0);
		}

		public bool isEnemySpawn ()
		{
				return enemySpawn;
		}

		public bool isPlayerSpawn ()
		{
				return playerSpawn;
		}

		public bool isEndSpawn ()
		{
				return endPoint;
		}

		public void SetNeighbours (Tile tileDown, Tile tileLeft, Tile tileUp, Tile tileRight,
	                          Tile tileDownLeft, Tile tileUpLeft, Tile tileUpRight, Tile tileDownRight)
		{
			
				tile_neighbours [(int)Direction.Down] = tileDown; //Down
				tile_neighbours [(int)Direction.Left] = tileLeft; //Left
				tile_neighbours [(int)Direction.Up] = tileUp; //Up
				tile_neighbours [(int)Direction.Right] = tileRight; //Right
				tile_neighbours [(int)Direction.Down_Left] = tileDownLeft; //Down-Left
				tile_neighbours [(int)Direction.Up_Left] = tileUpLeft; //Up-Left 
				tile_neighbours [(int)Direction.Up_Right] = tileUpRight; //Up-Right
				tile_neighbours [(int)Direction.Down_Right] = tileDownRight; //Down-Right

				for (int i = 0; i < tile_neighbours.Length; i++) {
						if (tile_neighbours [i] == null) {
								atEdge = true;
						}		
				}
				/*if (tileDown == null & tileLeft == null) {
						if (tile_neighbours [0] == null) {
								Debug.Log ("LEFT BOTTOM CONER REACHED!!!: ");
						}
				}*/
				SetSprite ();
		}

		private void SetSprite ()
		{
				if (state == 1) {
						if (tile_neighbours [(int)Direction.Up] == null || tile_neighbours [(int)Direction.Up].state == 0) {
								platformPos = PlatformPos.Center;
						}
						if (tile_neighbours [(int)Direction.Up].state == alive) {
								platformPos = PlatformPos.Middle;
						}
				}
	
		}

		public void SwitchState ()
		{
				if (state == 0) {
						state = 1;
				} else {
						state = 0;	
				}
		}

		public void UpdateTile ()
		{
				if (!atEdge) {
						EnemySpawnRule ();
						VerticalRangeRule ();
						SurroundedRule ();
						
						if (this.tilePos.y > 0) {
								SingleGapRule ();
						}

						if (this.tilePos.y == 3) {
								UpperPlatformExtendSingle ();	
						} 
						if (this.tilePos.y == 2) {
								UpperAccessRule ();
								//UpperPlatformExtendSingle ();	
						} 



						if (this.placeInArray.x == level_length - 1 ||
								this.placeInArray.x == level_length - 2 ||
								this.placeInArray.x == level_length - 3) {
								EndSpawnRule ();		
						}
				}
				

				if (this.tilePos.x == 0 ||
		    this.tilePos.x == 1 ||
		    this.tilePos.x == 2 ||
		    this.tilePos.x == 3) {
						PlayerSpawnRule ();		
				} 


			
				SetSprite ();
		}

		private void VerticalRangeRule ()
		{
		
				if (state == 1) {
						if (tile_neighbours [(int)Direction.Down].state == alive &&
								tile_neighbours [(int)Direction.Right].state == alive &&
								tile_neighbours [(int)Direction.Down_Right].state == alive &&
			   
								tile_neighbours [(int)Direction.Up].state == dead &&
								tile_neighbours [(int)Direction.Left].state == dead &&
								tile_neighbours [(int)Direction.Up_Left].state == dead &&
								tile_neighbours [(int)Direction.Up_Right].state == dead &&
								tile_neighbours [(int)Direction.Down_Left].state == dead) {
								state = dead;
						}
				}
		
		}

		private void SurroundedRule ()
		{
				//If tile dead and surrounded by alive on above and below rows, make alive
				if (state == 0) {
						if (tile_neighbours [(int)Direction.Down].state == alive &&
								tile_neighbours [(int)Direction.Up].state == alive &&
								tile_neighbours [(int)Direction.Left].state == alive &&
								tile_neighbours [(int)Direction.Right].state == alive &&
								tile_neighbours [(int)Direction.Down_Left].state == alive &&
								tile_neighbours [(int)Direction.Down_Right].state == alive &&
								tile_neighbours [(int)Direction.Up_Left].state == alive &&
								tile_neighbours [(int)Direction.Up_Right].state == alive) {
								state = 1;
						}	
		
		
				}
		}

		private void SingleGapRule ()
		{
				//If tile dead and surrounded by alive on above and below rows, make alive
				if (state == 0) {
						if (tile_neighbours [(int)Direction.Down].state == alive &&

								tile_neighbours [(int)Direction.Left].state == alive &&
								tile_neighbours [(int)Direction.Right].state == alive &&
								tile_neighbours [(int)Direction.Down_Left].state == alive &&
								tile_neighbours [(int)Direction.Down_Right].state == alive) {
								state = 1;
								isSpriteSet = false;
						}	
			
			
				}
		}

		private void UpperPlatformExtendSingle ()
		{
				if (state == 0) {
						if (tile_neighbours [(int)Direction.Down].state == dead &&
			    
								tile_neighbours [(int)Direction.Left].state == alive &&
								tile_neighbours [(int)Direction.Right].state == alive &&
								tile_neighbours [(int)Direction.Down_Left].state == dead &&
								tile_neighbours [(int)Direction.Down_Right].state == dead) {
								state = alive;
								isSpriteSet = false;
						}	
		
				}
				if (state == 1) {
						if (tile_neighbours [(int)Direction.Down].state == dead &&
			    
								tile_neighbours [(int)Direction.Left].state == dead &&
								tile_neighbours [(int)Direction.Right].state == dead &&
								tile_neighbours [(int)Direction.Down_Left].state == dead &&
								tile_neighbours [(int)Direction.Down_Right].state == dead) {
								state = dead;
								isSpriteSet = false;
						}	
			
				}
		}

		private void UpperAccessRule ()
		{
				if (state == 0) {
						if (tile_neighbours [(int)Direction.Down].state == alive &&
								tile_neighbours [(int)Direction.Up].state == dead &&
								tile_neighbours [(int)Direction.Left].state == dead &&
								tile_neighbours [(int)Direction.Right].state == dead &&
								tile_neighbours [(int)Direction.Down_Left].state == alive &&
								tile_neighbours [(int)Direction.Down_Right].state == dead &&
								tile_neighbours [(int)Direction.Up_Left].state == dead &&
								tile_neighbours [(int)Direction.Up_Right].state == alive) {
								state = alive;
								isSpriteSet = false;
						}	
					
				}
		}

		private void RuleFour ()
		{
				if (state == 0) {
						if (tile_neighbours [0] == null) {
								state = 1;
						}		
				}
		}

		//If tile is surrounded by only 3 alive tiles (all below) make enemy spawn point possible
		private void EnemySpawnRule ()
		{
				if (this.state == 0) {
						if (tile_neighbours [0].state == 1 &&
								tile_neighbours [4].state == 1 &&
								tile_neighbours [7].state == 1 &&
								tile_neighbours [1].state == 0 &&
								tile_neighbours [3].state == 0 && 
								tile_neighbours [2].state == 0 && 
								tile_neighbours [5].state == 0 &&
								tile_neighbours [6].state == 0) {
								enemySpawn = true;
								//Debug.Log("ENEMY TILE!!! " + tilePos.x + " " + tilePos.y);
						}
				} 
		}

		private void PlayerSpawnRule ()
		{
				if (this.state == 0) {
						if (tile_neighbours [0].state == 1) {
								playerSpawn = true;
						}
				}
		}

		private void EndSpawnRule ()
		{
				if (this.state == 0) {
						if (tile_neighbours [0].state == 1) {
								endPoint = true;
						}
				}
		}
}

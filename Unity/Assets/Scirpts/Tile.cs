using UnityEngine;
using System.Collections;

public class Tile
{

		// Create an enum that will contain each possible tile type
		public enum TileType
		{
				Platform,
				Sky,
				Start,
				Finish,
				Null // Null is required for my generation method
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
	
		// A variable that will hold the current position of a tile
		public Vector2 tilePos;


		// Basic Empty constructor that sets the default type to null so that they will not be created
		public Tile ()
		{

				tileType = TileType.Null;
				tile_neighbours = new Tile [8];

		}

		public void SetNeighbours (Tile tileDown, Tile tileLeft, Tile tileUp, Tile tileRight,
	                          Tile tileDownLeft, Tile tileUpLeft, Tile tileUpRight, Tile tileDownRight)
		{
		/*if( tileDown == null & tileLeft == null){
			Debug.Log ("LEFT BOTTOM CONER REACHED!!!");
		}*/
				tile_neighbours [0] = tileDown; //Down
				tile_neighbours [1] = tileLeft; //Left
				tile_neighbours [2] = tileUp; //Up
				tile_neighbours [3] = tileRight; //Right
				tile_neighbours [4] = tileDownLeft; //Down-Left
				tile_neighbours [5] = tileUpLeft; //Up-Left 
				tile_neighbours [6] = tileUpRight; //Up-Right
				tile_neighbours [7] = tileDownRight; //Down-Right

		if (tileDown == null & tileLeft == null) {
						if (tile_neighbours [0] == null) {
								Debug.Log ("LEFT BOTTOM CONER REACHED!!!: ");
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
				
					RuleTwo ();
					//RuleThree ();
					RuleFour ();
		}

		private void RuleOne ()
		{
				//If tile dead and surrounded by alive on above and below rows, make alive
				if (state == 0) {
						if (tile_neighbours [5].state == 1 &&
								tile_neighbours [2].state == 1 &&
								tile_neighbours [6].state == 1 &&
								tile_neighbours [4].state == 1 &&
								tile_neighbours [0].state == 1 &&
								tile_neighbours [7].state == 1) {
								state = 1;
						}	
		
		
				}
		}

		private void RuleTwo ()
		{
				if (state == 0) {
						int alive = 0;
						for (int i = 0; i < tile_neighbours.Length; i++) {
				if( tile_neighbours[i] != null){
								if (tile_neighbours [i].state == 1 ) {
										alive++;
								}
				}
						}
						if (alive >= 5) {
								state = 1;
						}
		
				}
		}
	private void RuleThree ()
	{
		if (state == 1) {
			int alive = 0;
			for (int i = 0; i < tile_neighbours.Length; i++) {
				if (tile_neighbours [i].state == 1) {
					alive++;
				}
			}
			if (alive < 1) {
				state = 0;
			}
			
		}
	}
	private void RuleFour(){
		if (state == 0) {
			if(tile_neighbours[0] == null){
				state = 1;
			}		
		}
	}
}

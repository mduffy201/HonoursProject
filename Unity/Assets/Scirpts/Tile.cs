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
		// Create a variable that will hold the tile type for each tile that is created
		// We can then check each tile to see if we can walk on it or if something else should happen
		public TileType tileType;
	
		// A variable that will hold the current position of a tile
		public Vector2 tilePos;


		// Basic Empty constructor that sets the default type to null so that they will not be created
		public Tile ()
		{
				tileType = TileType.Null;
				tile_neighbours = new Tile [4];

		}

	public void SetNeighbours(Tile tile1, Tile tile2, Tile tile3, Tile tile4){
		tile_neighbours [0] = tile1; //Down
		tile_neighbours [1] = tile2; //Left
		tile_neighbours [2] = tile3; //Up
		tile_neighbours [3] = tile4; //Right
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
		if (tile_neighbours [0] != null) {
						if (state == 1) {
								int noDead = 0;
								for (int i = 0; i < 4; i++) {
										if (tile_neighbours [i].state == 0) {
												noDead++;
										}
								}
								if (noDead > 2) {
										state = 0;
								}
		
		
						}
				}
		}
}

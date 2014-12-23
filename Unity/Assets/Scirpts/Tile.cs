using UnityEngine;
using System.Collections;

public class Tile  {

	// Create an enum that will contain each possible tile type
	public enum TileType 
	{
		Platform,
		Sky,
		Null // Null is required for my generation method
	}
	// Create a variable that will hold the tile type for each tile that is created
	// We can then check each tile to see if we can walk on it or if something else should happen
	public TileType tileType;
	
	// A variable that will hold the current position of a tile
	public Vector2 tilePos;


	// Basic Empty constructor that sets the default type to null so that they will not be created
	public Tile()
	{
		tileType = TileType.Null;
	}
}

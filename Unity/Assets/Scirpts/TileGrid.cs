using UnityEngine;
using System.Collections;

public class TileGrid : MonoBehaviour {

	// A vector2 that contains the x width and y height of the tile map
	public Vector2 worldSize;
	
	// Array containing data for all tiles within the world map
	public Tile[,] levelMap;
	
	// Object that we use to create our map container
	private GameObject tileParent;
	
	// The name of the folder that you want to load the map data from (Main Map is the default)
	// This should match the name of the folder than contains your sprite prefabs and sprite sheet for each tile set
	private string setName = "Main Map";


	// Use this for initialization
	void Start () {
		// Create an empty object with the name "Map" to act as the parent for our tiles 
		tileParent = new GameObject();
		tileParent.name = "Map";
		
		
		// Set the length of the array to be equal to the vector2 world size
		levelMap = new Tile[(int)worldSize.x,(int)worldSize.y];
		
		// For each row of tile
		for (int i = 0; i < worldSize.x; i++)
		{
			// For each row, cycle through an entire column
			for (int j = 0; j < worldSize.y; j++)
			{
				// Now we combine the loops' indexes to make sure each slot has a null tile
				levelMap[i,j] = new Tile();
				levelMap[i,j].tileType = Tile.TileType.Null;
			}
		}
		
		// Quick and dirty way to load data for the map to generate. The map is loaded based on AddTile, check the LoadBonusMap method
		// for details on how I'm creating the simple tile map
		//LoadBonusMap();
		LoadMap1 ();
		// Now we need to take the loaded data and generate sprites based on the information
		//GenerateMap();
		
		// Just an example using GetTile
		print(GetTile(3,2).tileType);
	}
	
	// AddTile is pretty straight forward, really
	public void AddTile(int xPos, int yPos, Tile.TileType type)
	{
		// Set the tile type based on the xPos and yPos (explained a bit more wit the next comment)
		levelMap[xPos,yPos].tileType = type;
		
		// I'm using xPos and yPos for the array's index as well as the actual position of the tile
		// This makes the tile map extremely simple since we now know worldMap[5,17] is at position 5x,17y.
		levelMap[xPos,yPos].tilePos = new Vector2(xPos,yPos);
	}
	
	// A method for getting data from a tile at a specific coordinate (isn't necessary, but it looks cleaner this way)
	public Tile GetTile(int xPos, int yPos)
	{
		// All this does is return a Tile from the worldMap array.
		// This could be done without the method with just the array, but this is quicker to write and looks cleaner
		return levelMap[xPos,yPos];
	}
	
	// The method that creates a sprite for each tile represented in worldMap[,]
	public void GenerateMap()
	{
		// Loop structure is the same as the loops above ^^^
		for (int i = 0; i < worldSize.x; i++)
		{
			for (int j = 0; j < worldSize.y; j++)
			{
				// Check to see if the tile type is null or not
				// If the tile is null, we know not to try to draw a sprite for it
				if (levelMap[i,j].tileType != Tile.TileType.Null)
				{
					// If the tile isn't null, it must exist so let's do some stuff
					// We're going through the Resources folder, within the Main Map directory and finding a sprite that matches
					// the TYPE of tile that we've found. ***Make sure the name of the sprite matches the tile types***
					
					// We're creating an instance of the sprite object that we find based on the tiletype compared to the object name, storing it in tileSprite
					GameObject tileSprite = (GameObject)Instantiate((GameObject)Resources.Load(setName + "/" + levelMap[i,j].tileType.ToString()), levelMap[i,j].tilePos, Quaternion.identity);
					
					// Now set the newly created sprite's parent to be the map parent that we created earlier 
					tileSprite.transform.parent = tileParent.transform;
				}
			}
		}
	}
	
	// Quick and dirty way to create map data
	public void LoadMap1()
	{
		// Set the name of the tileset that this map will use (it has to match the name of the folder within Resources for each tileset)
		//setName = "Main Map";
		// Using add tile, send the X and Y position as well as the type of tile
		AddTile (0,0,Tile.TileType.Platform);
		AddTile (0,1,Tile.TileType.Platform);
		AddTile (0,2,Tile.TileType.Platform);
		AddTile (0,3,Tile.TileType.Platform);
		AddTile (0,4,Tile.TileType.Platform);
		AddTile (1,0,Tile.TileType.Platform);
		AddTile (1,1,Tile.TileType.Platform);
		AddTile (1,2,Tile.TileType.Platform);
		AddTile (1,3,Tile.TileType.Platform);
		AddTile (1,4,Tile.TileType.Platform);
		AddTile (2,0,Tile.TileType.Platform);
		AddTile (2,1,Tile.TileType.Platform);
		AddTile (2,2,Tile.TileType.Platform);
		AddTile (2,3,Tile.TileType.Platform);
		AddTile (2,4,Tile.TileType.Platform);
		AddTile (3,0,Tile.TileType.Platform);
		AddTile (3,1,Tile.TileType.Platform);
		AddTile (3,2,Tile.TileType.Platform);
		AddTile (3,3,Tile.TileType.Platform);
		AddTile (3,4,Tile.TileType.Platform);
		AddTile (4,4,Tile.TileType.Platform);
		AddTile (5,4,Tile.TileType.Platform);
		AddTile (5,3,Tile.TileType.Platform);
		AddTile (4,3,Tile.TileType.Platform);
		AddTile (4,0,Tile.TileType.Platform);
		AddTile (4,1,Tile.TileType.Platform);
		AddTile (4,2,Tile.TileType.Platform);
		AddTile (5,5,Tile.TileType.Platform);
		AddTile (4,5,Tile.TileType.Platform);
		AddTile (3,5,Tile.TileType.Platform);
		AddTile (2,5,Tile.TileType.Platform);
		AddTile (1,5,Tile.TileType.Platform);
		AddTile (0,5,Tile.TileType.Platform);
		AddTile (5,4,Tile.TileType.Platform);
		AddTile (5,3,Tile.TileType.Platform);
		AddTile (5,2,Tile.TileType.Platform);
		AddTile (5,1,Tile.TileType.Platform);
		AddTile (5,0,Tile.TileType.Platform);
		AddTile (5,2,Tile.TileType.Platform);
		
	}
	
	// This is the same as the other map, but it could be completely different
	public void LoadBonusMap()
	{
		//setName = "Forest Map";
		// Using add tile, send the X and Y position as well as the type of tile
		/*AddTile (0,0,Tile.TileType.Grass);
		AddTile (0,1,Tile.TileType.Grass);
		AddTile (0,2,Tile.TileType.Grass);
		AddTile (0,3,Tile.TileType.Grass);
		AddTile (0,4,Tile.TileType.Grass);
		AddTile (1,0,Tile.TileType.Dirt);
		AddTile (1,1,Tile.TileType.Dirt);
		AddTile (1,2,Tile.TileType.Dirt);
		AddTile (1,3,Tile.TileType.Grass);
		AddTile (1,4,Tile.TileType.Grass);
		AddTile (2,0,Tile.TileType.Grass);
		AddTile (2,1,Tile.TileType.Grass);
		AddTile (2,2,Tile.TileType.Dirt);
		AddTile (2,3,Tile.TileType.Dirt);
		AddTile (2,4,Tile.TileType.Dirt);
		AddTile (3,0,Tile.TileType.Grass);
		AddTile (3,1,Tile.TileType.Grass);
		AddTile (3,2,Tile.TileType.Grass);
		AddTile (3,3,Tile.TileType.Water);
		AddTile (3,4,Tile.TileType.Water);
		AddTile (4,4,Tile.TileType.Water);
		AddTile (5,4,Tile.TileType.Water);
		AddTile (5,3,Tile.TileType.Water);
		AddTile (4,3,Tile.TileType.Water);
		AddTile (4,0,Tile.TileType.Grass);
		AddTile (4,1,Tile.TileType.Grass);
		AddTile (4,2,Tile.TileType.Water);
		AddTile (5,5,Tile.TileType.Grass);
		AddTile (4,5,Tile.TileType.Grass);
		AddTile (3,5,Tile.TileType.Grass);
		AddTile (2,5,Tile.TileType.Grass);
		AddTile (1,5,Tile.TileType.Grass);
		AddTile (0,5,Tile.TileType.Grass);
		AddTile (5,4,Tile.TileType.Grass);
		AddTile (5,3,Tile.TileType.Grass);
		AddTile (5,2,Tile.TileType.Grass);
		AddTile (5,1,Tile.TileType.Grass);
		AddTile (5,0,Tile.TileType.Grass);
		AddTile (5,2,Tile.TileType.Grass);*/
	}
}

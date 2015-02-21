using UnityEngine;
using System.Collections;
//using System;

public class TileGrid : MonoBehaviour {

	// A vector2 that contains the x width and y height of the tile map
	public Vector2 worldSize;
	
	// Array containing data for all tiles within the world map
	public Tile[,] levelMap;
	
	// Object that we use to create our map container
	private GameObject tileParent;



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

		// Draw Generated Map
		DrawTileMap();
		
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
	

	
	// Generate Level
	public void LoadMap1()
	{
		// Set the name of the tileset that this map will use (it has to match the name of the folder within Resources for each tileset)
		//setName = "Main Map";
		// Using add tile, send the X and Y position as well as the type of tile
		//DateTime time =  new DateTime();
		System.DateTime time = System.DateTime.Now;

		Random.seed = time.Millisecond;//(int)Time.time;
		int startpos = (int)(Random.value* 10000) %(int)worldSize.y;
		int endpos = (int)(Random.value* 10000) %(int)worldSize.y;
		Debug.Log("Start pos:  " + (Random.value* 1000));




		for (int i = 0; i < worldSize.x; i++)
		{
			for (int j = 0; j < worldSize.y; j++)
			{
				//random assignment
				 
				Random.seed = i * j-1;
				int tile_state = (int)(Random.value* 1000) %2;
				//Debug.Log("Random value: " + tt);

				if(tile_state==0){
					AddTile (i,j,Tile.TileType.Sky);}
				if(tile_state==1){
					AddTile (i,j,Tile.TileType.Platform);
				}
				if (j == startpos && i ==0){
					AddTile (i,j,Tile.TileType.Start);
				}
				if (j == endpos && i == worldSize.x-1){
					AddTile (i,j,Tile.TileType.Finish);
				}
			}
		}
	}





	// The method that creates a sprite for each tile represented in worldMap[,]
	public void DrawTileMap()
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
					GameObject tileObject;
					// We're creating an instance of the sprite object that we find based on the tiletype compared to the object name, storing it in tileSprite
					tileObject = (GameObject)Instantiate(Resources.Load("Tiles/Tile"), levelMap[i,j].tilePos, Quaternion.identity);
					tileObject.GetComponent<TileObject>().SetTile(levelMap[i,j]);
						//bullet.GetComponent<Bullet>().SetDir(newdir)
					// Now set the newly created sprite's parent to be the map parent that we created earlier 
					tileObject.transform.parent = tileParent.transform;
				}
			}
		}
	}
}

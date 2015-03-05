using UnityEngine;
using System.Collections;

public class TileObject : MonoBehaviour {

	//Atached tile script
	private Tile tile;

	//Possition in array
	public int xpos;
	public int ypos;

	//
	public bool sprite_set = false;
	private SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (sprite_set == false) {
			//Debug.Log("Tile Check: " + Tile.TileType.);
			switch(tile.tileType)
			{
			case Tile.TileType.Platform:
				gameObject.layer = 8;
				if(tile.platformPos == Tile.PlatformPos.Middle){
					spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Ground");
				}
				if(tile.platformPos == Tile.PlatformPos.Center){
				 	spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/grassMid");
				}
				sprite_set = true;
				break;
			case Tile.TileType.Sky:
				spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Sky");
				sprite_set = true;
				boxCollider.enabled = false;
				break;
			case Tile.TileType.Start:
				spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Start");
				sprite_set = true;
				boxCollider.enabled = false;
				break;
			case Tile.TileType.Finish:
				spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Finish");
				sprite_set = true;
				boxCollider.enabled = false;
				break;
			}
		
		}
	}
	public void SetTile(Tile tileIn){

		tile = tileIn;
		xpos = (int)tileIn.tilePos.x;
		ypos = (int)tileIn.tilePos.y;
	//	sprite_set = false;
	}
}

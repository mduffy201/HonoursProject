using UnityEngine;
using System.Collections;

public class TileObject : MonoBehaviour
{

		//Atached tile script
		private Tile tile;

		//Possition in array
		public int xpos;
		public int ypos;
	
		public bool sprite_set = false;
		private SpriteRenderer spriteRenderer;
		private BoxCollider2D boxCollider;

		// Use this for initialization
		void Start ()
		{
				spriteRenderer = GetComponent<SpriteRenderer> ();
				boxCollider = GetComponent<BoxCollider2D> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
				//if (sprite_set == false) {
			if (tile.isSpriteSet == false) {
						//Debug.Log("Tile Check/sprite set: x-" + xpos + " y-" + ypos);
						switch (tile.tileType) {
						case Tile.TileType.Platform:
								gameObject.layer = 8;
								if (tile.platformPos == Tile.PlatformPos.Middle) {
										spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Ground");
								}
								if (tile.platformPos == Tile.PlatformPos.Center) {
										spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/grassMid");
								}
				tile.isSpriteSet = true;
								//sprite_set = true;
								break;
						case Tile.TileType.Sky:

								spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Sky_Clear");
								//sprite_set = true;
				tile.isSpriteSet = true;
			
								boxCollider.enabled = false;
								break;
						case Tile.TileType.Start:
								spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Start");
								//sprite_set = true;
				tile.isSpriteSet = true;
								boxCollider.enabled = false;
								break;
						case Tile.TileType.Finish:
								spriteRenderer.sprite = Resources.Load<Sprite> ("Tiles/Finish");
								//sprite_set = true;
				tile.isSpriteSet = true;
								boxCollider.enabled = false;
								break;
						}
		
				}
		}

		public void SetTile (Tile tileIn)
		{

				tile = tileIn;
				xpos = (int)tileIn.placeInArray.x;
				ypos = (int)tileIn.placeInArray.y;
				//	sprite_set = false;
		}
}

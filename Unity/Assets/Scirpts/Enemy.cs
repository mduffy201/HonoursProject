using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{



		//walker
		private float maxSpeed = 5f;
		float move = -1.0f;
		//jumper
		[SerializeField]
		private float
				jump_power = 250.0f;
		[SerializeField]
		private float
				jump_angle = 90.0f;
		[SerializeField]
		private float
				jump_timer = 100.0f;

		//Shooter
		[SerializeField]
		private float
				shot_radius = 2.0f;
		[SerializeField]
		private float
				shot_timer = 100.0f;
		public Transform groundCheck; // A position marking where to check if the player is grounded.
		private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		public bool grounded = false; // Whether or not the player is grounded.

		[SerializeField]
		private LayerMask
				whatIsGround;
		public Transform edgeCheckLeft;
		public Transform edgeCheckRight;
		private SpriteRenderer spriteRenderer;

		public enum MoveDirection
		{
				Left,
				Right
		}

		public enum EnemyType
		{
				Walker,
				Jumper,
				Shooter,
				Null // Null is required for my generation method
		}
		// Create a variable that will hold the tile type for each tile that is created
		// We can then check each tile to see if we can walk on it or if something else should happen
		public EnemyType enemyType;
		public MoveDirection moveDirection;

		// A variable that will hold the current position of a tile
		public Vector2 enemyPos;

		// Use this for initialization
		void Start ()
		{
				spriteRenderer = GetComponent<SpriteRenderer> ();
				moveDirection = MoveDirection.Left;
				groundCheck = transform.Find ("GroundCheck");
				edgeCheckLeft = transform.Find ("EdgeCheckLeft");
				edgeCheckRight = transform.Find ("EdgeCheckRight");

				//Apply appropriate sprite
				if (enemyType == EnemyType.Walker) {
						spriteRenderer.sprite = Resources.Load<Sprite> ("Enemy/walker");
			
				}
				if (enemyType == EnemyType.Jumper) {
						spriteRenderer.sprite = Resources.Load<Sprite> ("Enemy/jumper");
		
				}
				if (enemyType == EnemyType.Shooter) {
						spriteRenderer.sprite = Resources.Load<Sprite> ("Enemy/shooter");
						rigidbody2D.gravityScale = 0.0f;
			
				}
				
		}

		public void SetType (EnemyType typeIn)
		{
				this.enemyType = typeIn;
	
		}

		private void FixedUpdate ()
		{
				if (enemyType == EnemyType.Walker) {
						if (moveDirection == MoveDirection.Left) {
								collider2D.enabled = false;
								RaycastHit2D hit = Physics2D.Raycast (edgeCheckLeft.position, -Vector2.up, 2.0f);
								collider2D.enabled = true;


								
								if (hit.collider == null) {
							
										move = 1.0f;
										moveDirection = MoveDirection.Right;
						
								} else {
										//Debug.Log ("ENEMY LEFT SENSE: " + hit.collider.name.ToString ());
								}
						}

						if (moveDirection == MoveDirection.Right) {
								collider2D.enabled = false;
								RaycastHit2D hit = Physics2D.Raycast (edgeCheckRight.position, -Vector2.up, 2.0f);
								collider2D.enabled = true;
			
								if (hit.collider == null) {

										move = -1.0f;
										moveDirection = MoveDirection.Left;
				
				}else {
					//Debug.Log ("ENEMY RIGHT SENSE: " + hit.collider.name.ToString ());
				}
						}
		

						// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
						grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				}
				if (enemyType == EnemyType.Jumper) {
			
						grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				}
		}
		// Update is called once per frame
		void Update ()
		{
				if (enemyType == EnemyType.Walker) {
						// Move the character
						//transform.position += new Vector3 (maxSpeed, 0.0f, 0.0f);
						if (grounded) {
								rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
						}
				}
				if (enemyType == EnemyType.Jumper) {
						if (grounded) {
								jump_timer--;

								if (jump_timer < 0) {
										rigidbody2D.AddForce (new Vector2 (-1 * jump_power, jump_power));
										jump_timer = 100.0f;
								}
						}
			
				}
				if (enemyType == EnemyType.Shooter) {
						shot_timer--;	
						if (shot_timer < 0) {

								shot_timer = 10000.0f;
								ShootMore ();
						}
		
				}
		}

		void Shoot ()
		{
				int shot = 4;
				GameObject[] bullets = new GameObject[4];
				Vector3[] shot_spawn = new Vector3[shot];
				Vector3[] shot_dir = new Vector3[shot];


				shot_spawn [0] = new Vector3 (transform.position.x, transform.position.y + shot_radius, 0.0f);
				shot_spawn [1] = new Vector3 (transform.position.x + shot_radius, transform.position.y, 0.0f);
				shot_spawn [2] = new Vector3 (transform.position.x, transform.position.y - shot_radius, 0.0f);
				shot_spawn [3] = new Vector3 (transform.position.x - shot_radius, transform.position.y, 0.0f);

				shot_dir [0] = Vector3.up;
				shot_dir [1] = Vector3.right;
				shot_dir [2] = -Vector3.up;
				shot_dir [3] = -Vector3.right;
				// b.direction = Vector3.up;
				//Instantiate ((GameObject)b, shot_spawn [0], Quaternion.identity) as GameObject;


				for (int i = 0; i < shot; i++) {
				
						bullets [i] = Instantiate ((GameObject)Resources.Load ("Bullet/BulletPrefab"), shot_spawn [i], Quaternion.identity) as GameObject;
		
						bullets [i].GetComponent<Bullet> ().SetDir (shot_dir [i]);
						//Bullet b = bullets[i].GetComponent<Bullet>();
						//b.direction = Vector3.up;
				}
				Debug.Log ("Shoot");




				//Instantiate((GameObject)Resources.Load("Bullet/Bullet"), transform.position, Quaternion.identity);
		}

		void ShootMore ()
		{
				int shot = 8;
				GameObject[] bullets = new GameObject[shot];
				Vector3[] shot_spawn = new Vector3[shot];
				Vector3[] shot_dir = new Vector3[shot];
		
		
				shot_spawn [0] = new Vector3 (transform.position.x, transform.position.y + shot_radius, 0.0f);
				shot_spawn [1] = new Vector3 (transform.position.x + shot_radius, transform.position.y, 0.0f);
				shot_spawn [2] = new Vector3 (transform.position.x, transform.position.y - shot_radius, 0.0f);
				shot_spawn [3] = new Vector3 (transform.position.x - shot_radius, transform.position.y, 0.0f);

				shot_spawn [4] = new Vector3 (transform.position.x + shot_radius, transform.position.y + shot_radius, 0.0f);
				shot_spawn [5] = new Vector3 (transform.position.x + shot_radius, transform.position.y - shot_radius, 0.0f);
				shot_spawn [6] = new Vector3 (transform.position.x - shot_radius, transform.position.y - shot_radius, 0.0f);
				shot_spawn [7] = new Vector3 (transform.position.x - shot_radius, transform.position.y + shot_radius, 0.0f);
		
				shot_dir [0] = Vector3.up;
				shot_dir [1] = Vector3.right;
				shot_dir [2] = -Vector3.up;
				shot_dir [3] = -Vector3.right;

				shot_dir [4] = new Vector3 (1.0f, 1.0f, 0.0f);
				shot_dir [5] = new Vector3 (1.0f, -1.0f, 0.0f);
				shot_dir [6] = new Vector3 (-1.0f, -1.0f, 0.0f);
				shot_dir [7] = new Vector3 (-1.0f, 1.0f, 0.0f);
				// b.direction = Vector3.up;
				//Instantiate ((GameObject)b, shot_spawn [0], Quaternion.identity) as GameObject;
		
		
				for (int i = 0; i < shot; i++) {
			
						bullets [i] = Instantiate ((GameObject)Resources.Load ("Bullet/BulletPrefab"), shot_spawn [i], Quaternion.identity) as GameObject;
			
						bullets [i].GetComponent<Bullet> ().SetDir (shot_dir [i]);
						//Bullet b = bullets[i].GetComponent<Bullet>();
						//b.direction = Vector3.up;
				}
				Debug.Log ("Shoot");


		}
}

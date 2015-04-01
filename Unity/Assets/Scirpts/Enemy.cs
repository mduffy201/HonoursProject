using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{
		private Animator anim;
		private bool facingRight = false; // For determining which way the player is currently facing.
		private PlayerMovement2D player;
		LevelStats level_stats;


		//walker
		public float speed;
		private float move = -1.0f;
		//jumper
		public float jump_power;
		public float jump_angle;
		public float jump_timer;
		private float jump_counter; 

		//Shooter
		private float shot_radius = 0.3f;
		public float	shot_timer;
		private Transform groundCheck; // A position marking where to check if the player is grounded.
		public float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		public bool grounded = false; // Whether or not the player is grounded.
		public LayerMask	whatIsGround;
	public LayerMask	whatIsPlayer;
		

		//public Vector2 VELOCITYNTHAT;
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
		private MoveDirection moveDirection;

		// A variable that will hold the current position of a tile
		private Vector2 enemyPos;
		private Vector3 edgeCheckLeft;
		private Vector3 edgeCheckRight;
		private Vector3 vedgeCheckLeft;
		private Vector3 vedgeCheckRight;

		void Awake ()
		{
				level_stats = GameObject.Find ("LevelLogic").GetComponent<LevelStats> ();
				player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement2D> ();
				anim = GetComponent<Animator> ();			
		}

		void Start ()
		{
				speed = level_stats.enemy_speed;
				jump_power = level_stats.enemy_jump_power;
				//jump_angle = level_stats.enemy_jump_angle;
				jump_timer = level_stats.enemy_jump_timer;
				shot_timer = level_stats.enemy_shot_timer;
		
				moveDirection = MoveDirection.Left;
				groundCheck = transform.Find ("GroundCheck");
				edgeCheckLeft = transform.Find ("EdgeCheckLeft").position;
				edgeCheckRight = transform.Find ("EdgeCheckRight").position;
				vedgeCheckLeft = edgeCheckLeft - transform.position;
				vedgeCheckRight = edgeCheckRight - transform.position;
				jump_counter = jump_timer;
		
			//	Debug.Log ("EE type switch" + enemyType.ToString ());
				switch (enemyType) {
			
				case EnemyType.Walker:
						anim.SetBool ("walker", true);
						anim.SetBool ("flyer", false);
						anim.SetBool ("jumper_idle", false);
						anim.SetBool ("jumper_jump", false);
			//Move_Walker ();
						break;
				case EnemyType.Jumper:
//						Debug.Log ("JUMPER SET");
						anim.SetBool ("walker", false);
						anim.SetBool ("flyer", false);
						anim.SetBool ("jumper_idle", true);
						anim.SetBool ("jumper_jump", false);
			//anim.speed = 0.0f;
			
			//Move_Jumper ();
						break;
				case EnemyType.Shooter:
						anim.SetBool ("walker", false);
						anim.SetBool ("flyer", true);
						anim.SetBool ("jumper_idle", false);
						anim.SetBool ("jumper_jump", false);
			Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
			transform.Translate(newPos);
			//Move_Shooter ();
						break;
				}
				
		}

		public void SetType (EnemyType typeIn)
		{
				this.enemyType = typeIn;
	
		}

		private void FixedUpdate ()
		{
				switch (enemyType) {
				case EnemyType.Walker:
						Move_Walker ();
						break;
				case EnemyType.Jumper:
						Move_Jumper ();
						break;
				case EnemyType.Shooter:
						Move_Shooter ();
						break;
		
				}


				
				if (transform.position.y < -5.0f || transform.position.x < -5.0f) {
						Destroy (gameObject);
				}
		}

		private void Move_Walker ()
		{
				//If enemy get stuck on tile join, apply slight upward force
				if (grounded && rigidbody2D.velocity.x == 0) {
						rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y + 1.0f);
				}
				
				// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				
				if (grounded) {
						rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y);
				} 

				RaycastHit2D hit = new RaycastHit2D ();
		
				if (moveDirection == MoveDirection.Left) {
						bool edge = false;
						bool wall = false;
						hit = new RaycastHit2D ();
						for (int i = 0; i < 3; i++) {
								if (i == 0) {
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.up, 0.5f);
										collider2D.enabled = true;
					
										if (hit.collider == null) {
						
												edge = true;
										}
								} else if (i == 1) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.right, 0.3f);
										collider2D.enabled = true;
										if (hit.collider != null) {
												//	Debug.Log ("HIT: " + hit.collider.name);
												if (hit.collider.tag == "Platform") {
														wall = true;

												}
												if (hit.collider.tag == "Enemy") {
														wall = true;
												}
												if (hit.collider.tag == "Player") {
														Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								} else if (i == 2) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, Vector2.right, 0.3f);
										collider2D.enabled = true;
										if (hit.collider != null) {

												if (hit.collider.tag == "Player") {
														Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								}
				
				
								if (edge || wall) {
										//	Debug.Log ("SWITCH TO RIGHT");
										move *= -1.0f;
										moveDirection = MoveDirection.Right;
										edge = false;
										wall = false;
										Vector3 theScale = transform.localScale;
										theScale.x *= -1;
										transform.localScale = theScale;
								}
						}
			
			
			
				} else if (moveDirection == MoveDirection.Right) {
						bool edge = false;
						bool wall = false;
						//if(!facingRight){Flip();}
						//RaycastHit2D 
						hit = new RaycastHit2D ();
			

						for (int i = 0; i < 3; i++) {
//								Debug.Log ("MOVING RIGHT");
								//Check downwards
								if (i == 0) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, -Vector2.up, 0.5f);
										collider2D.enabled = true;
					
										if (hit.collider == null) {
						
												edge = true;
										}
								}
								//Check to right
								else if (i == 1) {

										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, Vector2.right, 0.3f);
										collider2D.enabled = true;

										//	Debug.Log ("HIT: " + hit);
										if (hit.collider != null) {
												//Debug.Log ("HIT: " + hit.collider.name);
												if (hit.collider.tag == "Platform") {

														wall = true;
												} 
												if (hit.collider.tag == "Enemy") {
														wall = true;
												}
												if (hit.collider.tag == "Player") {
														//Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								} else if (i == 2) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.right, 0.3f);
										collider2D.enabled = true;
					
										//	Debug.Log ("HIT: " + hit);
										if (hit.collider != null) {

												if (hit.collider.tag == "Player") {
														//Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								}
						}
						if (edge || wall) {
								//Debug.Log ("SWITCH TO LEFT");
								move *= -1.0f;
								moveDirection = MoveDirection.Left;
								edge = false;
								wall = false;

								Vector3 theScale = transform.localScale;
								theScale.x *= -1;
								transform.localScale = theScale;
						}		
				}
		}

		private void Move_Jumper ()
		{
				RaycastHit2D hit = new RaycastHit2D ();
				//float jump_counter = jump_timer;
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				if (!grounded) {
						anim.SetBool ("jumper_idle", false);
						anim.SetBool ("jumper_jump", true);

						collider2D.enabled = false;
						hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.up, 0.4f);
						collider2D.enabled = true;
						if (hit.collider != null) {
//								Debug.Log ("HIT: " + hit.collider.name);

								if (hit.collider.tag == "Player") {
										player.Kill ();
					
								}
						}
				} else {
						anim.SetBool ("jumper_idle", true);
						anim.SetBool ("jumper_jump", false);
				}		
				jump_timer--;
		
				if (jump_timer < 0) {
						rigidbody2D.AddForce (new Vector2 (move * jump_power, jump_power));
						jump_timer = jump_counter;
				}


				
		
				if (moveDirection == MoveDirection.Left) {

						bool wall = false;
						hit = new RaycastHit2D ();
						for (int i = 0; i < 2; i++) {

								if (i == 0) {
					
										collider2D.enabled = false;
					hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.right, 0.3f);
										collider2D.enabled = true;

										if (hit.collider != null) {
												Debug.Log ("HIT: " + hit.collider.name);
												if (hit.collider.tag == "Platform") {
														wall = true;
							
												}
												if (hit.collider.tag == "Player") {
														player.Kill ();
							
												}
										}
								} else if (i == 1) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, Vector2.right, 0.5f);
										collider2D.enabled = true;
										if (hit.collider != null) {
						
												if (hit.collider.tag == "Player") {
														Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								}

								if (wall) {
										//	Debug.Log ("SWITCH TO RIGHT");
										move = 1.0f;
										moveDirection = MoveDirection.Right;
										wall = false;
										Vector3 theScale = transform.localScale;
										theScale.x *= -1;
										transform.localScale = theScale;
								}
						}
			
			
			
				} else if (moveDirection == MoveDirection.Right) {
		
						bool wall = false;
						
						hit = new RaycastHit2D ();
			
			
						for (int i = 0; i < 2; i++) {

								//Check to right
								if (i == 0) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, Vector2.right, 0.5f);
										collider2D.enabled = true;
					
//										Debug.Log ("HIT: " + hit);
										if (hit.collider != null) {
												Debug.Log ("HIT: " + hit.collider.name);
												if (hit.collider.tag == "Platform") {
							
														wall = true;
												} 
												if (hit.collider.tag == "Player") {
														player.Kill ();
							
												}
										}
								} else if (i == 1) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.right, 0.5f);
										collider2D.enabled = true;
					
										//	Debug.Log ("HIT: " + hit);
										if (hit.collider != null) {
						
												if (hit.collider.tag == "Player") {
														//Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								}
						}
						if (wall) {
								//Debug.Log ("SWITCH TO LEFT");
								move = -1.0f;
								moveDirection = MoveDirection.Left;

								wall = false;
				
								Vector3 theScale = transform.localScale;
								theScale.x *= -1;
								transform.localScale = theScale;
						}		
				}
		}
//Puase enmy movement when shooting
		int shot_pause = 10;
		bool pause = false;

		private void Move_Shooter ()
		{
				RaycastHit2D hit = new RaycastHit2D ();
				Vector3 newPos = new Vector3 ();
				rigidbody2D.isKinematic = true;

				if (pause) {
						shot_pause--;
						if (shot_pause <= 0) {
								pause = false;
								shot_pause = 10;
						}
				}


				if (moveDirection == MoveDirection.Left && !pause) {
						newPos = new Vector3 (-speed * Time.deltaTime, 0.0f, 0.0f);
				} 


				transform.Translate (newPos);

				shot_timer--;	
				if (shot_timer < 0) {
						pause = true;
						shot_timer = 100.0f;
						if (level_stats.enemy_shot_difficulty == 0) {
								Shoot ();
						} else {
								ShootMore ();
						}
				}


				
			
						bool wall = false;
						hit = new RaycastHit2D ();
						for (int i = 0; i < 2; i++) {
				
								if (i == 0) {
					
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckLeft, -Vector2.right, 0.3f);
										collider2D.enabled = true;
					
										if (hit.collider != null) {
												Debug.Log ("HIT: " + hit.collider.name);

												if (hit.collider.tag == "Player") {
														player.Kill ();
												}
										}
								} else if (i == 1) {
										collider2D.enabled = false;
										hit = Physics2D.Raycast (transform.position + vedgeCheckRight, Vector2.right, 0.5f);
										collider2D.enabled = true;
										if (hit.collider != null) {
						
												if (hit.collider.tag == "Player") {
														Debug.Log ("HIT: " + hit.collider.name);
														player.Kill ();
												}
										}
								}
						}
			
			
			
				
		}






		//Method to switch the players direction
		private void Flip ()
		{
				// Switch the way the player is labelled as facing.
				facingRight = !facingRight;
		
				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
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

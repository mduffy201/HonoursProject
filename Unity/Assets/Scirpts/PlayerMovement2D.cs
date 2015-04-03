using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
		private AudioSource audio_source;
		private AudioClip jump;
		private AudioClip enemy_death;
		LevelStats levelStats;
		private bool facingRight = true; // For determining which way the player is currently facing.
	
		
		[SerializeField]
		private LayerMask
				whatIsGround; // A mask determining what is ground to the character
		
		public bool
				grounded = true; // Whether or not the player is grounded.
		


		public float max_velocity = 10.0f;
		public float x_velocity = 0.0f;
		public float y_velocity = 0.0f;
		public float acceleration = 1.0f;
		public float drag = 0.2f;
		private float dir = 0.0f;
		public float jump_force = 20.0f;
		public float gravity = 3.0f;
		public bool wall_right = false;
		public bool wall_left = false;

		//public float terminal_velocity = 50.0f;
		public float kill_bounce = 10.0f;
		public float ray_length;
		public float ray_height;
		public float jump_timer = 0.2f;
		public float reduced_air_control = 2.0f;
		private Animator anim;
		private bool keyDown = false;

		private void Awake ()
		{
				jump = (AudioClip)Resources.Load ("SoundFX/player_jump");
				enemy_death = (AudioClip)Resources.Load ("SoundFX/enemy_death");
				levelStats = GameObject.Find ("LevelLogic").GetComponent<LevelStats> ();
				audio_source = GetComponent<AudioSource> ();		
				ray_length = 0.3f; //GetComponent<Renderer> ().bounds.size.x / 2*5;
				ray_height = 0.56f;// (GetComponent<Renderer> ().bounds.size.y/2)*5;
				anim = GetComponent<Animator> ();
		}

		private void FixedUpdate ()
		{

				//Debug.Log ("FIXED UPDATE");

				CastRays ();
				CheckFallDeath ();
				CheckHorizontalMovement ();
				CheckVerticalMovement ();
				Move ();

				if (!grounded) {
						anim.SetBool ("jumping", true);
						anim.SetBool ("moving", false);
						anim.SetBool ("idle", false);
				} else if (grounded && x_velocity != 0) {
						anim.SetBool ("moving", true);
						anim.SetBool ("jumping", false);
						anim.SetBool ("idle", false);
				} else {
						anim.SetBool ("idle", true);
						anim.SetBool ("jumping", false);
						anim.SetBool ("moving", false);
				}
				

		}

		private void CheckVerticalMovement ()
		{

				if (grounded) {
						y_velocity = 0;
				} else {
						y_velocity -= gravity;
				}
		
				if (Input.GetKey (KeyCode.UpArrow)) {
						if (grounded && !keyDown) {
								audio_source.PlayOneShot (jump, 10.0f);
								keyDown = true;
								y_velocity = jump_force;
								//grounded = false;						
								levelStats.jumps++;
						}
				} else {
						keyDown = false;		
				}
		}

		private void CheckHorizontalMovement ()
		{
				if (Input.GetKey (KeyCode.RightArrow)) {

						if (grounded && !wall_right) {
								dir = 1.0f;
								if (!facingRight) {
										Flip ();
								}
								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration;
								}
					
								levelStats.time_moving_right += Time.deltaTime;

						} else if (!grounded && !wall_right) {
								dir = 1.0f;
				
								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration / reduced_air_control;
								}
								//moving = true;
								levelStats.time_moving_right += Time.deltaTime;
						} 
			
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
				
						if (grounded && !wall_left) {
								dir = -1.0f;
								if (facingRight) {
										Flip ();
								}
								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration;
								}
							
								levelStats.time_moving_left += Time.deltaTime;
				
						} else if (!grounded && !wall_left) {
								dir = -1.0f;
				
								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration / reduced_air_control;
								}
								//moving = true;
								levelStats.time_moving_right += Time.deltaTime;
						} 
				} else {
						//keyUp = true;
						if (x_velocity > 0) {
								x_velocity = x_velocity - drag;
						} else {
								x_velocity = 0;
								//moving = false;	
								//wall_bounce = false;
						}
				}
	
		}

		private void Move ()
		{
				
				Vector3 newPos = new Vector3 (transform.position.x + x_velocity * Time.deltaTime * dir, transform.position.y + y_velocity * Time.deltaTime, transform.position.z);
				transform.position = newPos;
				
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

		int frame = 0;
		public	bool groundedR = true;
		public	bool groundedL = true;

		private void CastRays ()
		{
				frame++;
				RaycastHit2D[] hitInfo = new RaycastHit2D[6];
				float offsetx = x_velocity * Time.deltaTime;
				float offsety = 0.0f;
				if (!grounded) {
						offsety = -y_velocity * Time.deltaTime;
				}	
				//Debug.Log ("CAST RAYS");
				RaycastHit2D hit = new RaycastHit2D ();
				for (int i = 0; i < hitInfo.Length; i++) {
			
						//Check Right
						if (i == 0) {
								//Debug.Log ("CAST RAYS (RIGHT)");
								hit = Physics2D.Raycast (transform.position, Vector2.right, ray_length + offsetx, whatIsGround);
								//Debug.Log ("RIGHT RAY: " + hit.collider.name);				
								if (hit.collider == null) {
//										Debug.Log ("RIGHT RAY HIT NULL");
										wall_right = false;
								} else if (hit.collider.tag == "Enemy") {
										//Debug.Log ("Enemy Hit");
										//Die ();
								} else if (hit.collider.tag == "Platform") {
										//Debug.Log ("RIGHT RAY HIT: " + hit.collider.tag.ToString () + " " + hit.collider.transform.position.ToString () + " ");
										if (!wall_right) {
												Vector3 newPos = new Vector3 (hit.distance - ray_length, 0.0f, 0.0f);
												//Debug.Log ("MOVE TO" + newPos.ToString ());
												transform.Translate (newPos);
										}
										x_velocity = 0;
										wall_right = true;
								} 
						}

						//Check Left
						if (i == 1) {
								hit = Physics2D.Raycast (transform.position, -Vector2.right, ray_length + offsetx, whatIsGround);
								//Debug.Log ("LEFT RAY: " + hit.collider.name);	
								if (hit.collider == null) {
										//Debug.Log ("LEFT RAY HIT NULL");
										wall_left = false;
								} else if (hit.collider.tag == "Enemy") {
										//Debug.Log ("Enemy Hit");
										Die ();
								} else if (hit.collider.tag == "Platform") {
					
										//Debug.Log ("RIGHT RAY HIT: " + hit.collider.tag.ToString () + " " + hit.collider.transform.position.ToString () + " ");
										if (!wall_left) {
												Vector3 newPos = new Vector3 (-hit.distance + ray_length, 0.0f, 0.0f);
												Debug.Log ("MOVE TO" + newPos.ToString ());
												transform.Translate (newPos);
										}
										x_velocity = 0;
										wall_left = true;
										//Debug.Log ("LEFT RAY HIT: " + hit.collider.tag.ToString ());
										//velocity = 0;
										//wall_left = true;
					
								}
						}
						//Check Ceiling
						if (i == 2) {
								hit = Physics2D.Raycast (transform.position, Vector2.up, 0.5f, whatIsGround);
								//Debug.Log ("LEFT RAY: " + hit.collider.name);	
								if (hit.collider == null) {
										//Debug.Log ("LEFT RAY HIT NULL");
										//wall_left = false;
								} else if (hit.collider.tag == "Enemy") {
										Debug.Log ("Enemy Hit");
										Die ();
								} else if (hit.collider.tag == "Platform") {
										//Debug.Log ("Ceiling Hit");
										y_velocity = 0;
										//jumping = false;
										//grounded = false;
										//wall_left = true;
					
								}
				
						}
					
						//Grounded Check
						if (i == 3) {
								//Debug.Log ("GROUND CHECK1: " + frame.ToString() + ", " + offsety.ToString());
								Vector3 castPosL = new Vector3 (transform.position.x - 0.2f, transform.position.y, 0.0f);
								hit = Physics2D.Raycast (castPosL, -Vector2.up, ray_height + offsety, whatIsGround);
								//hit = Physics2D.Raycast (transform.position, -Vector2.up, ray_height + offsety, whatIsGround);
				
								if (hit.collider != null) {
										//Debug.Log ("GROUND CHECK: " + frame.ToString() + ", " + hit.collider.tag);
								
							
										if (hit.collider.tag == "Platform") {
//										Debug.Log ("Ground Hit");
					
												Vector3 newPos = new Vector3 (0.0f, -hit.distance + ray_height, 0.0f);
												//Debug.Log (newPos.ToString ());
												transform.Translate (newPos);
												groundedL = true;
												//grounded = true;
												y_velocity = 0;

										} 

								} else if (hit.collider == null) {
										//Debug.Log ("GROUND CHECK2" + frame.ToString() );
										groundedL = false;
										//grounded = false;
								}
						}
						if (i == 4) {
								//Debug.Log ("GROUND CHECK1: " + frame.ToString() + ", " + offsety.ToString());
								Vector3 castPosR = new Vector3 (transform.position.x + 0.2f, transform.position.y, 0.0f);
								hit = Physics2D.Raycast (castPosR, -Vector2.up, ray_height + offsety, whatIsGround);
				
								if (hit.collider != null) {
										//Debug.Log ("GROUND CHECK: " + frame.ToString() + ", " + hit.collider.tag);
					
					
										if (hit.collider.tag == "Platform") {
												//										Debug.Log ("Ground Hit");
						
												Vector3 newPos = new Vector3 (0.0f, -hit.distance + ray_height, 0.0f);
												//Debug.Log (newPos.ToString ());
												transform.Translate (newPos);
						
												//grounded = true;
												groundedR = true;
												y_velocity = 0;
						
										} 
					
								} else if (hit.collider == null) {
										//Debug.Log ("GROUND CHECK2" + frame.ToString() );
										//grounded = false;
										groundedR = false;
								}
						}
						if (!groundedL && !groundedR) {
								grounded = false;
						} else {
								grounded = true;
						}
				}
		
		}

		void OnTriggerEnter2D (Collider2D collider)
		{

				Debug.Log ("IN TRIGGGER" + collider.name);
				if (collider.name == "EndTrigger") {
						Debug.Log ("Level Finish Triggered");
						Application.LoadLevel ("TitleScreen");
				}
				if (collider.name == "EnemyKillCheck") {
						audio_source.PlayOneShot (enemy_death, 10.0f);
						//Debug.Log ("Enemy Killed by head");
						levelStats.kills++;
						y_velocity += kill_bounce;
						//Debug.Log("Enemy Killed: " + collider.gameObject.transform.parent.gameObject.name);

						string enemyType = collider.gameObject.transform.parent.gameObject.name;
						switch (enemyType) {
						case "Walker":
								levelStats.kills_walker++;
								break;
						case "Jumper":
								levelStats.kills_jumper++;
								break;
						case "Shooter":
								levelStats.kills_shooter++;
								break;
						}

						Destroy (collider.gameObject.transform.parent.gameObject);
			
						//Application.LoadLevel("TitleScreen");
			
				}
				if (collider.tag == "bullet") {
						Debug.Log ("SHOT " + collider.name);
						Kill (Enemy.EnemyType.Shooter);
				}
				if (collider.tag == "finish") {
						Debug.Log ("LEVEL END");
						levelStats.DisplayLevelEnd ();
				}
		}

		private void CheckFallDeath ()
		{
				if (transform.position.y < -5) {
						levelStats.deaths_by_fall++;
						Die ();		
				}
		
		}

		public void Kill (Enemy.EnemyType enemy)
		{
				switch (enemy) {
				case Enemy.EnemyType.Walker:
						levelStats.deaths_by_walker++;
						break;
				case Enemy.EnemyType.Jumper:
						levelStats.deaths_by_jumper++;
						break;
				case Enemy.EnemyType.Shooter:
						levelStats.deaths_by_shooter++;
						break;
		
				}
				//Debug.Log ("Killed!");
				Die ();
	
		}

		private void Die ()
		{
				levelStats.deaths++;
				Debug.Log ("Player Dead");		
				Application.LoadLevel (Application.loadedLevelName);
		}
}

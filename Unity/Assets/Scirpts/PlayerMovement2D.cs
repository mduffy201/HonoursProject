using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
		LevelStats levelStats;
		private bool facingRight = true; // For determining which way the player is currently facing.
	
		
		[SerializeField]
		private LayerMask
				whatIsGround; // A mask determining what is ground to the character
		[SerializeField]
		private bool
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
		private void Awake ()
		{
				levelStats = GameObject.Find ("LevelLogic").GetComponent<LevelStats> ();
				ray_length = GetComponent<Renderer> ().bounds.size.x / 2;
				ray_height = GetComponent<Renderer> ().bounds.size.y / 2;
		}

		private void FixedUpdate ()
		{

				//Debug.Log ("FIXED UPDATE");

				CastRays ();
				CheckFallDeath ();
				CheckHorizontalMovement ();
				CheckVerticalMovement ();
				Move ();
				

		}
		
		private void CheckVerticalMovement ()
		{

				if (grounded) {
						y_velocity = 0;
				} else {
						y_velocity -= gravity;
			
						/*if (transform.position.y > terminal_velocity) {
								y_velocity = 0.0f;
						}*/
				}
		
				if (Input.GetKey (KeyCode.UpArrow)) {
						if (grounded) {
								y_velocity = jump_force;
								grounded = false;						
								levelStats.jumps++;
						}
				} 
		}

		private void CheckHorizontalMovement ()
		{
				if (Input.GetKey (KeyCode.RightArrow)) {

						if (grounded && !wall_right) {
								dir = 1.0f;

								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration;
								}
					
								levelStats.time_moving_right += Time.deltaTime;

						} else if (!grounded && !wall_right) {
								dir = 1.0f;
				
								if (x_velocity < max_velocity) {
					x_velocity = x_velocity + acceleration /  reduced_air_control;
								}
								//moving = true;
								levelStats.time_moving_right += Time.deltaTime;
						} 
			
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
				
						if (grounded && !wall_left) {
								dir = -1.0f;

								if (x_velocity < max_velocity) {
										x_velocity = x_velocity + acceleration;
								}
							
								levelStats.time_moving_left += Time.deltaTime;
				
						} else if (!grounded && !wall_left) {
								dir = -1.0f;
				
								if (x_velocity < max_velocity) {
					x_velocity = x_velocity + acceleration /  reduced_air_control;
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

		private void CastRays ()
		{
				RaycastHit2D[] hitInfo = new RaycastHit2D[4];
				float offsetx = x_velocity * Time.deltaTime;
				float offsety = -y_velocity * Time.deltaTime;
				//Debug.Log ("CAST RAYS");
				RaycastHit2D hit = new RaycastHit2D ();
				for (int i = 0; i < hitInfo.Length; i++) {
			
						//Check Right
						if (i == 0) {
								//Debug.Log ("CAST RAYS (RIGHT)");
								hit = Physics2D.Raycast (transform.position, Vector2.right, ray_length + offsetx, whatIsGround);
								//Debug.Log ("RIGHT RAY: " + hit.collider.name);				
								if (hit.collider == null) {
										//Debug.Log ("RIGHT RAY HIT NULL");
										wall_right = false;
								} else if (hit.collider.tag == "Enemy") {
										//Debug.Log ("Enemy Hit");
										Die ();
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
										Debug.Log ("Ceiling Hit");
										y_velocity = 0;
										//jumping = false;
										grounded = false;
										//wall_left = true;
					
								}
				
						}
						//Grounded Check
						if (i == 3) {
								hit = Physics2D.Raycast (transform.position, -Vector2.up, ray_height + offsety, whatIsGround);
								//Debug.Log ("LEFT RAY: " + hit.collider.name);	
								if (hit.collider == null) {
										grounded = false;
										//Debug.Log ("LEFT RAY HIT NULL");
										//wall_left = false;
								} else if (hit.collider.tag == "Enemy") {
										//Debug.Log ("Enemy Hit");
										//Die ();
								} else if (hit.collider.tag == "Platform") {
//										Debug.Log ("Ground Hit");
					
										Vector3 newPos = new Vector3 (0.0f, -hit.distance + ray_height, 0.0f);
										//Debug.Log (newPos.ToString ());
										transform.Translate (newPos);
					
										grounded = true;
										y_velocity = 0;
					
										//wall_left = true;
					
								}
				
						}
				}
		
		}

		void OnTriggerEnter2D (Collider2D collider)
		{

				Debug.Log (collider.name);
				if (collider.name == "EndTrigger") {
						Debug.Log ("Level Finish Triggered");
						Application.LoadLevel ("TitleScreen");
				}
				if (collider.name == "EnemyKillCheck") {

						Debug.Log ("Enemy Killed by head");
						levelStats.kills++;
						y_velocity += kill_bounce;
						Destroy (collider.gameObject.transform.parent.gameObject);
			
						//Application.LoadLevel("TitleScreen");
			
				}
				/*if (collider.name == "EdgeCheckLeft" || collider.name == "EdgeCheckRight") {
						Debug.Log ("Player Dead");		
						Application.LoadLevel (Application.loadedLevelName);
				}*/
		}

		private void CheckFallDeath ()
		{
				if (transform.position.y < -5) {
						Die ();		
				}
		
		}

		private void Die ()
		{
				levelStats.deaths++;
				Debug.Log ("Player Dead");		
				Application.LoadLevel (Application.loadedLevelName);
		}
}

using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
	
		private bool facingRight = true; // For determining which way the player is currently facing.
	
		//[SerializeField]
		//private float
		//		maxSpeed = 10f; // The fastest the player can travel in the x axis.
		//[SerializeField]
		//private float
		//		jumpForce = 400f; // Amount of force added when the player jumps.	

		[SerializeField]
		private LayerMask
				whatIsGround; // A mask determining what is ground to the character
		[SerializeField]
		private bool
				grounded = true; // Whether or not the player is grounded.
		private Transform groundCheck; // A position marking where to check if the player is grounded.
		public float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded


		public float max_velocity = 10.0f;
		public float velocity = 0.0f;
		public float acceleration = 1.0f;
		public float drag = 0.2f;
		private float dir = 0.0f;
		public float jump_force = 20.0f;
		public float y_velocity = 0.0f;
		public float gravity = 3.0f;
		public bool moving = false;
		public bool jumping = false;
		//public bool wall_bounce = false;
		public bool wall_right = false;
		public bool wall_left = false;
		public bool keyUp = true;
		public float centreDis = 1.0f;
		public float terminal_velocity = 10.0f;
		public float kill_bounce = 10.0f;

		private void Awake ()
		{
				groundCheck = transform.Find ("GroundCheck");
		}

		private void FixedUpdate ()
		{
				Debug.Log ("FIXED UPDATE");
				CastRays ();
				CheckHorizontalMovement ();
				//Move ();
			
				//Debug.Log ("Pre: " + transform.position.ToString ());
				// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground



				
				
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				//Debug.Log ("Grounded?: " + grounded.ToString ());
				//Is Grounded? RAYCAST OR OVERLAPS
				

				if (grounded) {
						//Debug.Log ("GROUNDED AT: " + transform.position.ToString ());
						y_velocity = 0;
						jumping = false;

				} else {
						y_velocity -= gravity;
						/*if (y_velocity > -terminal_velocity) {
								y_velocity -= gravity;
						} else {
								y_velocity = -terminal_velocity;
						}*/
						if (transform.position.y > 50.0f) {
								y_velocity = 0.0f;
						}
				}
				if (Input.GetKey (KeyCode.UpArrow)) {
			
						if (grounded) {
								y_velocity = jump_force;
								grounded = false;
								jumping = true;
						}
				} 
				if (jumping) {

						y_velocity -= gravity;
			
						if (transform.position.y > 50.0f) {
								y_velocity = 0.0f;
						}
			
				}
				Move ();
				/*Vector3 newPos = new Vector3 (transform.position.x, transform.position.y + y_velocity * Time.deltaTime, transform.position.z);
				transform.position = newPos;*/

		}

		private void CastRays ()
		{
				RaycastHit2D[] hitInfo = new RaycastHit2D[2];
				//Debug.Log ("CAST RAYS");
				RaycastHit2D hit = new RaycastHit2D ();
				for (int i = 0; i < hitInfo.Length; i++) {

						//Check Right
						if (i == 0) {
								//Debug.Log ("CAST RAYS (RIGHT)");
								hit = Physics2D.Raycast (transform.position, Vector2.right, 0.5f, whatIsGround);
								//Debug.Log ("RIGHT RAY: " + hit.collider.name);				
								if (hit.collider == null) {
										//Debug.Log ("RIGHT RAY HIT NULL");
										wall_right = false;
								} else if (hit.collider.tag == "Enemy") {
										Debug.Log ("Enemy Hit");
										Die ();
								} else if (hit.collider.tag == "Platform") {
										//Debug.Log ("RIGHT RAY HIT: " + hit.collider.tag.ToString ());
										velocity = 0;
										wall_right = true;
								} 
						}
						//Check Left
						if (i == 1) {
								hit = Physics2D.Raycast (transform.position, -Vector2.right, 0.5f, whatIsGround);
								//Debug.Log ("LEFT RAY: " + hit.collider.name);	
								if (hit.collider == null) {
										//Debug.Log ("LEFT RAY HIT NULL");
										wall_left = false;
								} else if (hit.collider.tag == "Enemy") {
										Debug.Log ("Enemy Hit");
										Die ();
								} else if (hit.collider.tag == "Platform") {
										//Debug.Log ("LEFT RAY HIT: " + hit.collider.tag.ToString ());
										velocity = 0;
										wall_left = true;
					
								}
						}
						
				}
	
		}

		private void CheckHorizontalMovement ()
		{
				if (Input.GetKey (KeyCode.RightArrow)) {
						keyUp = false;
						if (grounded && !wall_right) {
								dir = 1.0f;
				
								if (velocity < max_velocity) {
										velocity = velocity + acceleration;
								}
								moving = true;
						} 
			
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						keyUp = false;
						if (grounded && !wall_left) {
								dir = -1.0f;
								if (velocity < max_velocity) {
										velocity = velocity + acceleration;
								}
								moving = true;
				
						} 
				} else {
						keyUp = true;
						if (velocity > 0) {
								velocity = velocity - drag;
						} else {
								velocity = 0;
								moving = false;	
								//wall_bounce = false;
						}
				}
	
		}

		private void Move ()
		{
				
				Vector3 newPos = new Vector3 (transform.position.x + velocity * Time.deltaTime * dir, transform.position.y + y_velocity * Time.deltaTime, transform.position.z);
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

		void OnTriggerEnter2D (Collider2D collider)
		{

				Debug.Log (collider.name);
				if (collider.name == "EndTrigger") {
						Debug.Log ("Level Finish Triggered");
						Application.LoadLevel ("TitleScreen");
				}
				if (collider.name == "EnemyKillCheck") {
						Debug.Log ("Enemy Killed by head");
						y_velocity += kill_bounce;
						Destroy (collider.gameObject.transform.parent.gameObject);
			
						//Application.LoadLevel("TitleScreen");
			
				}
				/*if (collider.name == "EdgeCheckLeft" || collider.name == "EdgeCheckRight") {
						Debug.Log ("Player Dead");		
						Application.LoadLevel (Application.loadedLevelName);
				}*/
		}

		private void Die ()
		{
				Debug.Log ("Player Dead");		
				Application.LoadLevel (Application.loadedLevelName);
		}
}

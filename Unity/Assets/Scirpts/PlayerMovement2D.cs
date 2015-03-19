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

		private void Awake ()
		{
				groundCheck = transform.Find ("GroundCheck");
		}

		private void FixedUpdate ()
		{
				//Debug.Log ("FIXED UPDATE");
				// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				//Debug.Log ("Grounded?: " + grounded.ToString ());
				//Is Grounded? RAYCAST OR OVERLAPS
				if (grounded) {
						Vector3 newPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						transform.position = newPos;
			jumping = false;
				}
				if (!grounded) {
						Vector3 newPos = new Vector3 (transform.position.x, transform.position.y - 0.1f, transform.position.z);
						transform.position = newPos;
		
				}
				
		RaycastHit2D[] hitInfo = new RaycastHit2D[2];

		RaycastHit2D hit = new RaycastHit2D();
		for (int i = 0; i < hitInfo.Length; i++) {
				
			if(i == 0){
				hit = Physics2D.Raycast (transform.position , Vector2.right, 0.5f);
				if (hit.collider == null) {
					//Debug.Log ("RIGHT RAY HIT NULL");
					wall_right = false;
				}
				else if (hit.collider.tag == "Platform") {
					//Debug.Log ("RIGHT RAY HIT: " + hit.collider.tag.ToString ());
					velocity = 0;
					wall_right = true;
					
				} 
			}
			if(i == 1){
				hit = Physics2D.Raycast (transform.position , -Vector2.right, 0.5f);
				if (hit.collider == null) {
					//Debug.Log ("LEFT RAY HIT NULL");
					wall_left = false;
				}
				else if (hit.collider.tag == "Platform") {
					//Debug.Log ("LEFT RAY HIT: " + hit.collider.tag.ToString ());
					velocity = 0;
					wall_left = true;
					
				}
			}
		}



		}

		public void Update ()
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
								moving = false;	
								//wall_bounce = false;
						}
				}
				
				if (moving) {
						Vector3 newPos = new Vector3 (transform.position.x + velocity * Time.deltaTime * dir, transform.position.y, transform.position.z);
						transform.position = newPos;
				} 






				if (Input.GetKey (KeyCode.UpArrow)) {
						if (grounded && !jumping) {
								y_velocity = jump_force;
								jumping = true;
						}		
				}

				if (jumping ) {
						Vector3 newPos = new Vector3 (transform.position.x, transform.position.y + y_velocity * Time.deltaTime, transform.position.z);
						transform.position = newPos;		
						y_velocity = (y_velocity - gravity) * Time.deltaTime;

						if (y_velocity > 0) {
								//y_velocity = (y_velocity - gravity) * Time.deltaTime;
						} else {
							//	y_velocity = 0.0f;
								//jumping = false;
						}
				}
	
		}
		/*void OnTriggerEnter2D(Collider2D collider){
		Debug.Log ("Collider: " + collider.name.ToString ());
		velocity = velocity * -1;

	}/*

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


		/*public void Move (float move, bool jump)
		{
				Debug.Log ("Move Called");
				if (grounded) {
						//Vector3 newPos = new Vector3 (transform.position.x + 0.1f * Time.deltaTime, transform.position.y, transform.position.z);
						//transform.position = newPos;
						//rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
				} 
				if (!grounded) {
						rigidbody2D.velocity = new Vector2 ((move * maxSpeed) / 2, rigidbody2D.velocity.y);
				} 
				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !facingRight)
			// ... flip the player.
						Flip ();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && facingRight)
			// ... flip the player.
						Flip ();
				// If the player should jump...
				if (grounded && jump) {
						// Add a vertical force to the player.
						grounded = false;

						rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
				}

		}*/
}

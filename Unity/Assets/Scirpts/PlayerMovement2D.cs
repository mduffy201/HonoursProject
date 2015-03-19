using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
	
		private bool facingRight = true; // For determining which way the player is currently facing.
	
		[SerializeField]
		private float
				maxSpeed = 10f; // The fastest the player can travel in the x axis.
		[SerializeField]
		private float
				jumpForce = 400f; // Amount of force added when the player jumps.	

		[SerializeField]
		private LayerMask
				whatIsGround; // A mask determining what is ground to the character
		[SerializeField]
		private bool
				grounded = true; // Whether or not the player is grounded.
		private Transform groundCheck; // A position marking where to check if the player is grounded.
		private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded




		private void Awake ()
		{

				groundCheck = transform.Find ("GroundCheck");

		}

		private void FixedUpdate ()
		{
				// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
				Debug.Log ("Grounded?: " + grounded.ToString ());
				//Is Grounded? RAYCAST OR OVERLAPS
				if (grounded) {
						Vector3 newPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
						transform.position = newPos;
				}
				if (!grounded) {
						Vector3 newPos = new Vector3 (transform.position.x, transform.position.y - 0.1f, transform.position.z);
						transform.position = newPos;
		
				}
		}

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

		public void Update ()
		{
				if (Input.GetKey (KeyCode.RightArrow)) {
						if (grounded) {
								
								dir = 1.0f;
								if (velocity < max_velocity) {
										velocity = velocity + acceleration;
								}
								moving = true;
								
						} 
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						if (grounded) {
								dir = -1.0f;
								if (velocity < max_velocity) {
										velocity = velocity + acceleration;
								}
								moving = true;
								
						} 
				} else {
						
						if (velocity > 0) {
								velocity = velocity - drag;
						} else {
								moving = false;	
						}
				}
				
				if (moving) {
						Vector3 newPos = new Vector3 (transform.position.x + velocity * Time.deltaTime * dir, transform.position.y, transform.position.z);
						transform.position = newPos;
				} 

		if (Input.GetKey (KeyCode.UpArrow)) {
			if(grounded && !jumping){
				y_velocity = jump_force;
				jumping = true;
			}		
		}
		if (jumping) {
			Vector3 newPos = new Vector3 (transform.position.x, transform.position.y + y_velocity * Time.deltaTime, transform.position.z);
			transform.position = newPos;		
		
			if(y_velocity > 0){
			y_velocity = y_velocity - gravity;
			}
			else{
				y_velocity = 0.0f;
				jumping = false;
			}
			}
	
		}

		public void Move (float move, bool jump)
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
}

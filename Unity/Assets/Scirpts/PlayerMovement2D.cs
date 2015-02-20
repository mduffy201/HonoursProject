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
				//Is Grounded? RAYCAST OR OVERLAPS

		}

		public void Move (float move, bool jump)
		{
				if (grounded) {
						rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
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

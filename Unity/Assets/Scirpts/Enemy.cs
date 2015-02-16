using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{

	//walker
		private float maxSpeed = 5f;
		float move = -1.0f;
	//jumper
	[SerializeField] private float jump_power = 250.0f;
	[SerializeField] private float jump_angle = 90.0f;
	[SerializeField] private float jump_timer = 100.0f;

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
						
								}
						}
						if (moveDirection == MoveDirection.Right) {
								collider2D.enabled = false;
								RaycastHit2D hit = Physics2D.Raycast (edgeCheckRight.position, -Vector2.up, 2.0f);
								collider2D.enabled = true;
			
								if (hit.collider == null) {

										move = -1.0f;
										moveDirection = MoveDirection.Left;
				
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

				if(jump_timer<0){
					rigidbody2D.AddForce(new Vector2( -1 * jump_power, jump_power));
					jump_timer = 100.0f;
				}
			}
			
		}
		}
}

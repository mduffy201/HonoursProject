using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

//Controls require Playermovement2D class attached to player object
[RequireComponent(typeof (PlayerMovement2D))]

public class Controls2D : MonoBehaviour {

	private PlayerMovement2D character;
	private bool jump;

	private void Awake()
	{
		character = GetComponent<PlayerMovement2D>();
	}
	
	private void Update()
	{
		if(!jump)
			// Read the jump input in Update so button presses aren't missed.
			jump = CrossPlatformInputManager.GetButtonDown("Jump");
	}
	
	private void FixedUpdate()
	{
		// Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		if (h != 0) {
					//	character.Move (h, jump);
				}
		jump = false;
	}
}

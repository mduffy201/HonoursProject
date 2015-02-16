using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform gun;
	private GameObject bullet;

	// Use this for initialization
	void Start () {
		gun = transform.Find ("GunPosition");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log("Fire Pressed");	
			bullet = (GameObject)Instantiate((GameObject)Resources.Load("Bullet/Bullet"), gun.position, Quaternion.identity);
		}
	}
}

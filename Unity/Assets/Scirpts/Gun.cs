using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform gun;
	private GameObject bullet;
	public Vector3 newdir;

	Bullet b;
	// Use this for initialization
	void Start () {
		gun = transform.Find ("GunPosition");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log("Fire Pressed");	
			newdir = Vector3.right;
			bullet = (GameObject)Instantiate(Resources.Load("Bullet/BulletPrefab"), gun.position, Quaternion.identity);
			bullet.GetComponent<Bullet>().SetDir(newdir);
			bullet.GetComponent<Bullet>().SetOwner(gameObject.name);
			//b.direction = Vector3.right;
		}
	}
}

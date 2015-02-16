using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Vector3 currentPosition;
	private Vector3 direction;
	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		currentPosition = transform.position;
		direction = new Vector3 (1.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate (new Vector3 (direction.x * speed * Time.deltaTime, 0.0f, 0.0f));
	
	}
}

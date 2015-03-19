using UnityEngine;
using System.Collections;


//Class which controls the movement and interactions of a bullet object
//attached to bullet gameObject

public class Bullet: MonoBehaviour {

	private Vector3 currentPosition;
	private Vector3 direction;
	public float speed = 1.0f;

	private BulletOwner owner;
	public enum BulletOwner{
		Player,
		Enemy
	}


	// Use this for initialization
	void Start () {
		currentPosition = transform.position;
		//direction = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate (new Vector3 (direction.x * speed * Time.deltaTime, direction.y * speed * Time.deltaTime, 0.0f));
	
	}
	public void SetDir(Vector3 v3in){
		direction = v3in;
	}
	public void SetOwner(string name){
		Debug.Log ("Bullet fired by: " + name);

		if (name == "Player(Clone)") {
			owner = BulletOwner.Player;	
		
		}
	}
	void OnTriggerEnter2D(Collider2D collider){

		if (owner == BulletOwner.Player && collider.name == "EdgeCheckLeft" || collider.name == "EdgeCheckRight") {
			Destroy(collider.gameObject.transform.parent.gameObject);
		
		}
	}
}

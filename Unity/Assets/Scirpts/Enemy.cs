using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
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
	
	// A variable that will hold the current position of a tile
	public Vector2 enemyPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

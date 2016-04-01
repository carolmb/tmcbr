using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 60; // pixels per second
	private Character character;
	private Vector2 moveVector;

	void Start() {
		Vector2 tilePos = new Vector2 (Maze.instance.beginTile.x, Maze.instance.beginTile.y);
		transform.position = Maze.instance.TileToWorldPosition (tilePos);
	}

	void Awake() {
		character = GetComponent<Character> ();
	}

	void FixedUpdate() {
		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x != 0 || moveVector.y != 0) {
			character.Move (moveVector * speed * Time.deltaTime);
		} else {
			character.Stop ();
		}
	}

}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	private Character character;
	private Vector2 moveVector;

	void Start() {
		Vector2 tilePos = new Vector2 (Maze.instance.beginTile.x, Maze.instance.beginTile.y);
		transform.position = Maze.TileToWorldPosition (tilePos);
	}

	void Awake() {
		instance = this;
		character = GetComponent<Character> ();
	}

	void FixedUpdate() {
		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x != 0 || moveVector.y != 0) {
			character.InstantMove (moveVector * character.speed * Time.deltaTime);
		} else {
			character.Stop ();
		}
	}

}

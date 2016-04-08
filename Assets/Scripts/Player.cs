using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Character))]
public class Player : MonoBehaviour {

	public static Player instance;

	private Character character;
	private Vector2 moveVector;

	void Awake() {
		instance = this;
		character = GetComponent<Character> ();
	}

	void Update() {
		moveVector.x = Input.GetAxisRaw ("Horizontal");
		moveVector.y = Input.GetAxisRaw ("Vertical");
		if (moveVector.x != 0 || moveVector.y != 0) {
			Debug.Log ("input: " + moveVector);
			float angle = Math.VectorToAngle (moveVector);
			if (TryMove (angle)) {
				Debug.Log (Maze.WorldToTilePos (transform.position));
				character.TurnTo (angle);
			} else if (TryMove (angle + 45)) {
				Debug.Log (Maze.WorldToTilePos (transform.position));
				character.TurnTo (angle + 45); 
			} else if (TryMove (angle - 45)) {
				Debug.Log (Maze.WorldToTilePos (transform.position));
				character.TurnTo(angle - 45);
			} else {
				character.TurnTo (angle);
				character.Stop ();
			}
		} else {
			character.Stop ();
		}
	}

	bool TryMove(float angle) {
		Vector2 translation = Math.AngleToVector (angle) * character.speed;
		return character.InstantMove (translation);
	}

}

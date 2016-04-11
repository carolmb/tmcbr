using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : MonoBehaviour {

	// Character
	public Character character;
	// Max way
	private int max_way;
	// First position
	private Vector3 firstPosition;
	// First direction
	private int firstDirection;
	// Way
	Queue<Vector2> way = new Queue<Vector2>();

	//
	void Awake() {
		character = GetComponent<Character>();
		max_way = 10;
	}

	void Start() {
		transform.position = new Vector2(2, 4);
		transform.position = MazeManager.TileToWorldPosition (new Vector2 (2, 4));
		calcPosition ();
	}

	void calcPosition() {
		// First tile
		Vector2 position = MazeManager.TileToWorldPosition (transform.position);
		firstDirection = character.direction;
		// Check the direction
		if (character.direction == 2) {
			position.x += 1;
		} else if (character.direction == 1) {
			position.x -= 1;
		} else if (character.direction == 0) {
			position.y -= 1;
		}
		way.Enqueue(MazeManager.WorldToTilePos(position));
		firstPosition = MazeManager.TileToWorldPosition (transform.position);
	}

	//
	void Update() {
		CheckCollision();
	}

	// 
	void CheckCollision() {
		if (character.moving == false) {
			way.Clear ();
			calcPosition ();
		}
		// If the queue isn't empty
		if (way.Count > 0) {
			// First position
			Vector2 position = way.Peek();
			if (MazeManager.WorldToTilePos(Player.instance.transform.position) == MazeManager.WorldToTilePos(position)) {
				// Wait one second
				StartCoroutine(Wait());
				// First movement
				StartCoroutine(character.MoveTo(position));
				// Try find the player
				FindPlayer (position, max_way, false);
			}
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(1);
	}

	// Try find the player
	void FindPlayer(Vector2 position, int count, bool founded) {
		Debug.Log (position);
		// If was founded
		if (count == 0 && MazeManager.WorldToTilePos (Player.instance.transform.position) 
			!= MazeManager.WorldToTilePos (position)) {
			founded = false;
			ReturnToOriginalPosition ();
		} else if (MazeManager.WorldToTilePos (Player.instance.transform.position) == MazeManager.WorldToTilePos (position)) {
			founded = true;
			Debug.Log ("Founded");
			Vector2 pos = way.Dequeue();
			// Move
			StartCoroutine(character.MoveTo(MazeManager.WorldToTilePos (pos)));
		}
		// If the way isn't empty
		if (way.Count > 0) {
			// Check the way
			if ((position.y > 1) && MazeManager.maze.tiles[(int) position.x, (int) (position.y - 1)].isWalkable == true && !founded) {
				FindPlayer(new Vector2(position.x, position.y), --count, founded);
			}
			if (MazeManager.maze.tiles[(int) position.x, (int) position.y + 1].isWalkable == true && !founded) {
				FindPlayer(new Vector2(position.x, position.y), --count, founded);
			}
			if ((position.x > 1) && MazeManager.maze.tiles[(int) (position.x - 1), (int) position.y].isWalkable == true && !founded) {
				FindPlayer(new Vector2(position.x, position.y), --count, founded);
			}
			if (MazeManager.maze.tiles[(int) position.x + 1, (int) position.y].isWalkable == true && !founded) {
				FindPlayer(new Vector2(position.x, position.y), --count, founded);
			}
		}
	}

	// 
	void Attack() {
		//
	}

	//
	void ReturnToOriginalPosition() {
		transform.position = firstPosition;
		character.direction = firstDirection;
	}
}

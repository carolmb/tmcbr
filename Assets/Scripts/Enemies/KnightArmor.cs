using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : MonoBehaviour {

	// Character
	private Character character;
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
		// First tile
		Vector2 position = transform.position;
		firstDirection = character.direction;
		// Check the direction
		if (character.direction == 2) {
			position.x += 1;
		} else if (character.direction == 1) {
			position.x -= 1;
		} else if (character.direction == 0) {
			position.y += 1;
		}
		way.Enqueue(MazeManager.WorldToTilePos(position));
		firstPosition = transform.position;
	}

	//
	void Update() {
		CheckCollision();
	}

	// 
	void CheckCollision() {
		// First position
		Vector2 position = way.Dequeue();
		if (MazeManager.WorldToTilePos(Player.instance.transform.position) == MazeManager.WorldToTilePos(position)) {
			// Wait one second
			Invoke("Wait", 1);
			// First movement
			character.MoveTo(position);
			// Try find the player
			FindPlayer (position, max_way, false);
		}
	}

	// Try find the player
	void FindPlayer(Vector2 position, int count, bool founded) {
		// If was founded
		if (count == 0 && MazeManager.WorldToTilePos (Player.instance.transform.position) != position) {
			founded = false;
			ReturnToOriginalPosition ();
		} else if (MazeManager.WorldToTilePos (Player.instance.transform.position) == position) {
			founded = true;
			// Wait one second
			Invoke("Wait", 1);
			Vector2 pos = way.Dequeue();
			// Move
			character.MoveTo(pos);
		}
		// Check the way
		if (MazeManager.maze.tiles[(int) position.x, (int) position.y - 1].isWalkable == true && !founded) {
			FindPlayer(new Vector2(position.x, position.y), --count, founded);
		}
		if (MazeManager.maze.tiles[(int) position.x, (int) position.y + 1].isWalkable == true && !founded) {
			FindPlayer(new Vector2(position.x, position.y), --count, founded);
		}
		if (MazeManager.maze.tiles[(int) position.x - 1, (int) position.y].isWalkable == true && !founded) {
			FindPlayer(new Vector2(position.x, position.y), --count, founded);
		}
		if (MazeManager.maze.tiles[(int) position.x + 1, (int) position.y].isWalkable == true && !founded) {
			FindPlayer(new Vector2(position.x, position.y), --count, founded);
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

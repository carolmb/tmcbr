using UnityEngine;
using System.Collections;
using System.Collections.Generic;

			
public class FakeKnightArmor : MonoBehaviour {

	public Sprite[] sprites;

	public int direction;

	void Start() {
		InitialDirection ();

		GetComponent<SpriteRenderer> ().sprite = sprites [direction];
	}

	void InitialDirection () {
		Vector2 pos = MazeManager.WorldToTilePos (new Vector2(transform.position.x, transform.position.y));
		if (MazeManager.maze.tiles [(int)pos.x, (int)pos.y + 1].isWall) {
			direction = 0;
		} else if (MazeManager.maze.tiles [(int)pos.x + 1, (int)pos.y].isWall) {
			direction = 1;
		} else if (MazeManager.maze.tiles [(int)pos.x - 1, (int)pos.y].isWall) {
			direction = 2;	
		}

	}
}

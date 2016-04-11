using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : MonoBehaviour {

	private Character character;
	public int vision = 10;

	void Awake () {
		transform.position = MazeManager.TileToWorldPosition (new Vector2 (2, 4)) + new Vector3(0, Tile.size / 2, 0);
		character = GetComponent<Character> ();
	}

	void Start() {
		GridPath path = FindPlayer ();
	}

	GridPath FindPlayer () {
		Tile playerTile = Player.instance.character.currentTile;
		Tile myTile = character.currentTile;
		if (PathFinder.EstimateCost (myTile, playerTile) >= vision)
			return null;
		return PathFinder.FindPath (playerTile, myTile, vision);
	}

	// Update is called once per frame
	void Update () {
		if (!character.moving) {
			GridPath path = FindPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Tile nextTile = path.PreviousSteps.LastStep;
				Vector2 nextPosition = (Vector2) MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
				character.TurnTo (nextPosition);
				StartCoroutine (character.MoveTo (nextPosition));
			}
		}
	}

}

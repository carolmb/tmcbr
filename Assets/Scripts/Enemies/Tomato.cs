using UnityEngine;
using System.Collections;

public class Tomato : Enemy {

	public GameObject tomato;

	// Use this for initialization

	void Start () {
		Spawn ();
	}
	
	void Update () {
		if (Player.instance.paused)
			return;

		if (Player.instance.repel) {
			GridPath path = PathToPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Tile nextTile = path.PreviousSteps.LastStep;
				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
				Vector2 np = new Vector2(nextPosition.x - transform.position.x, nextPosition.y - transform.position.y);
				np.x *= -1;
				np.y *= -1;
				character.TurnTo (np);
				character.MoveTo (np + (Vector2) transform.position);
			}
		}
	}

	void Spawn () {
		if (!character.moving && !character.damaging) {
			GridPath path = PathToPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Invoke ("MiniTomato", 3);
			}
		}
		Invoke ("Spawn", 6);
	}

	void MiniTomato () {
		Instantiate (tomato, character.transform.position, character.transform.rotation);
	}
}

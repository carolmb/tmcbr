using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class KnightArmor : Enemy {

	protected override void Awake () {
		base.Awake ();
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			GridPath path = PathToPlayer ();
			if (path != null && path.PreviousSteps != null) {
				Tile nextTile = path.PreviousSteps.LastStep;
				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
				character.TurnTo (nextPosition);
				//currentMovement = StartCoroutine (character.MoveTo (nextPosition));
				character.MoveTo (nextPosition);
			}
		}
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
}

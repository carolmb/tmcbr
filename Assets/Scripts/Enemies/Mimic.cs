using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mimic : Enemy {

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
				Vector2 nextPosition = (Vector2) MazeManager.TileToWorldPosition (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
				character.TurnTo (nextPosition);
				currentMovement = StartCoroutine (character.MoveTo (nextPosition));
			}
		}
	}
}

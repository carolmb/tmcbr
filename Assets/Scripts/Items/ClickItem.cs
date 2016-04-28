using UnityEngine;
using System.Collections;

public class ClickItem : MonoBehaviour {

	public int vision = 10;
	public bool Close = false;

	public ClickItem () {
		Close = false;
	}

	public void OnMouseDown () {
		if (Close == false) {
			ChaseItem ();
		}
	}

	private Tile ClosestToItem () {
		Tile playerTile = Player.instance.character.currentTile;
		Vector2 position = MazeManager.WorldToTilePos(transform.position);
		Tile t = MazeManager.maze.tiles [(int)position.x, (int)position.y];

		if (PathFinder.EstimateCost (playerTile, t) >= vision)
			return null;

		GridPath path = PathFinder.FindPath (t, playerTile, vision);
		if (path != null && path.PreviousSteps != null) {
			return path.PreviousSteps.LastStep;
		} else {
			return null;
		}
		return null;
	}

	public bool ChaseItem () {
		Tile nextTile = ClosestToItem ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			Player.instance.character.TurnTo (nextPosition);
			Player.instance.character.MoveTo (nextPosition);
			Close = false;
			return true;
		} else {
			Close = true;
			return false;
		}
	}
}

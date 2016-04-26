using UnityEngine;
using System.Collections;

public class Objects : MonoBehaviour {

	public GameObject item;
	public int vision = 10;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool MoveToItem() {
		Tile nextTile = ClosestToPlayer ();
		if (nextTile != null) {
			Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (nextTile.coordinates) + new Vector2 (0, Tile.size / 2);
			Player.instance.character.TurnTo (nextPosition);
			Player.instance.character.MoveTo (nextPosition);
			return true;
		} else {
			return false;
		}
	}

	private Tile ClosestToPlayer() {
		Tile playerTile = Player.instance.character.currentTile;
		Tile myTile = currentTile;
		if (PathFinder.EstimateCost (myTile, playerTile) >= vision)
			return null;

		GridPath path = PathFinder.FindPath (playerTile, myTile, vision);
		if (path != null && path.PreviousSteps != null) {
			return path.PreviousSteps.LastStep;
		} else {
			return null;
		}
	}

	// Tile atual do objeto
	public Tile currentTile {
		get {
			Vector2 tileCoord = MazeManager.WorldToTilePos(transform.position - new Vector3(0, Tile.size / 2, 0));
			return MazeManager.maze.tiles [(int)tileCoord.x, (int)tileCoord.y];
		}
	}
}

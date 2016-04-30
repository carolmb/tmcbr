using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

	public int moveDistance = 10; // Distância em tiles para poder andar até o obstáculo
	public float interactDistance = 32; // Distância em pixels pra poder interagir

	private Coroutine currentPath;

	public void OnMouseUp () {
		if (isNearPlayer) {
			SendMessage ("OnInteract");
		} else {
			if (currentPath != null)
				StopCoroutine (currentPath);
			currentPath = StartCoroutine(ChaseItem ());
		}
	}

	public bool isNearPlayer {
		get {
			return (transform.position - Player.instance.transform.position).magnitude <= interactDistance;
		}
	}

	public IEnumerator ChaseItem () {
		Tile objTile = MazeManager.GetTile (transform.position - new Vector3 (0, Tile.size / 2, 0));
		Tile playerTile = Player.instance.character.currentTile;
		GridPath path = PathFinder.FindPath(objTile, playerTile, moveDistance);

		if (path != null) {
			while (path.PreviousSteps != null) {
				path = path.PreviousSteps;
				if (path.PreviousSteps == null || !Player.instance.canMove || Player.instance.character.damaging)
					break;
				Tile t = path.LastStep;
				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (t.coordinates) + new Vector2 (0, Tile.size / 2);
				Player.instance.character.TurnTo (nextPosition);
				yield return Player.instance.character.MoveTo (nextPosition);
			}
		}
	}

}

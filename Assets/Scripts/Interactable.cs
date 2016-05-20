using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour {

	public int moveDistance = 10; // Distância em tiles para poder andar até o obstáculo
	public float interactDistance = 32; // Distância em pixels pra poder interagir

	private Coroutine currentPath;
	private BoxCollider2D hitBox;

	void Start () {
		BoxCollider2D[] allHitboxes = GetComponents<BoxCollider2D> ();
		hitBox = allHitboxes [allHitboxes.Length - 1];
	}

	void Update () {
		if (Player.instance.interactedPoint != Vector2.zero) {
			Rect r = new Rect ();
			r.size = hitBox.size;
			r.center = hitBox.bounds.center;
			if (r.Contains(Player.instance.interactedPoint)) {
				if (isNearPlayer) {
					SendMessage ("OnInteract");
				} else {
					if (currentPath != null)
						StopCoroutine (currentPath);
					currentPath = StartCoroutine(ChaseItem ());
				}
			}
		}
	}

	public bool isNearPlayer {
		get {
			return (transform.position - Player.instance.transform.position).magnitude <= interactDistance;
		}
	}

	public IEnumerator ChaseItem() {
		Player.instance.character.TurnTo (Player.instance.interactedPoint);
		yield return Player.instance.character.MoveTo(Player.instance.interactedPoint, true);
	}

	public IEnumerator ChaseItemOld () {
		Tile objTile = MazeManager.GetTile (transform.position - new Vector3 (0, Tile.size / 2, 0));
		Tile playerTile = Player.instance.character.currentTile;

		GridPath path = PathFinder.FindPath(objTile, playerTile, moveDistance);

		if (path != null) {
			path = path.PreviousSteps;

			while (path != null) {
				if (!Player.instance.canMove || Player.instance.character.damaging)
					break;
				Tile t = path.LastStep;
				Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (t.coordinates) + new Vector2 (0, Tile.size / 2);
				Player.instance.character.TurnTo (nextPosition);
				yield return Player.instance.character.MoveTo (nextPosition, true);
				path = path.PreviousSteps;
			}
		}
	}

}

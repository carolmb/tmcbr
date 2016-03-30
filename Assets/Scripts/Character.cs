using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D rb2D;
	private BoxCollider2D boxCollider;

	public int direction {
		get {
			return animator.GetInteger ("Direction");
		}
		set {
			animator.SetInteger ("Direction", value);
		}
	}
		
	void Awake () {
		animator = GetComponent<Animator> ();
		rb2D = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	public void Move(Vector2 translation) {
		Vector2 newPosition = (Vector2)transform.position + translation;
		CheckTiles (ref newPosition);
	}

	private void CheckTiles(ref Vector2 newPosition) {
		Vector2 tilePos = TileToWorldPosition (newPosition);
		int x = (int)tilePos.x;
		int y = (int)tilePos.y;
		foreach (Tile tile in Maze.instance[x, y].GetNeighbours()) {

		}
		//TODO: checar as colisões dos tiles sao redor
	}

	public Vector2 TileToWorldPosition(Vector2 tilePos) {
		return tilePos * Tile.size;
	}

	public Vector2 WorldToTilePos(Vector2 worldPos) {
		return new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
	}

}

using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour {

	public int state = 0; // 0 = invisível, 1 = rachado, 2 = aberto

	public Sprite brokenSprite;
	public Sprite openSprite;

	void Start () {
		transform.Translate (0, - Tile.size / 2, 100);
	}

	void OnTriggerExit2D(Collider2D other) {
		if (state < 2 && other.CompareTag ("Player")) {
			if (state == 0) {
				Break ();
			} else {
				Open (); 
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (state == 2 && other.CompareTag ("Player")) {
			StartCoroutine(Player.instance.Fall (MazeManager.GetTile(transform.position).transition));
		}
	}

	void Break() {
		GetComponent<SpriteRenderer> ().sprite = brokenSprite;
		MazeManager.GetTile (transform.position).objectName = "Crack";
		state = 1;
	}

	void Open() {
		GetComponent<SpriteRenderer> ().sprite = brokenSprite;
		MazeManager.GetTile (transform.position).objectName = "Hole";
		state = 2;
	}

}

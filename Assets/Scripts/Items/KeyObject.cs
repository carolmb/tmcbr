using UnityEngine;
using System.Collections;

public class KeyObject : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			Item item = Item.DB [9];
			SoundManager.Rose ();
			SaveManager.currentSave.bag.Add (item);
			Vector3 pos = MazeManager.WorldToTilePos (transform.position - new Vector3 (0, Tile.size, 0));
			MazeManager.maze.tiles [(int)pos.x, (int)pos.y].objectName = "";
			Destroy (gameObject);
		}
	}

}

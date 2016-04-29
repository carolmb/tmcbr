using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {

	public Sprite reflex;
	public Sprite broken;

	public AudioClip breakSound;
	public bool broke = false;

	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		transform.Translate (Tile.size / 2, 0, 0);
	}

	void Update() {
		if (broke)
			return;

		if (Player.instance.character.direction != 3)
			return;
		
		if (Mathf.Abs (Player.instance.transform.position.x - transform.position.x) < 8) {
			if (transform.position.y - Player.instance.transform.position.y < 48) {
				ShowReflex ();
			}
		}
	}

	void ShowReflex() {
		Player.instance.canMove = false;
		Player.instance.character.Stop ();
		broke = true;
		sr.sprite = reflex;
		Invoke ("Break", 0.5f);
	}

	void Break() {
		GameCamera.PlayAudioClip (breakSound);
		Player.instance.canMove = true;
		sr.sprite = broken;
		MazeManager.GetTile ((Vector2)transform.position - new Vector2 (Tile.size / 2, Tile.size / 2)).obstacleID = 5;
	}

}

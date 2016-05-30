using UnityEngine;
using System.Collections;

public class Statue : MonoBehaviour {

	public GameObject rose;
	Vector3 iniPos;
	Animator controller;

	void Start () {
		controller = GetComponent<Animator> ();
		controller.SetBool ("isDying", false);
		iniPos = transform.position;
	}

	public void Explosion() {
		SoundManager.Explosion ();
		controller.SetBool ("isDying", true);
	}

	void Die() {
		Vector2 iniPosTile = MazeManager.WorldToTilePos (iniPos - new Vector3(0, Tile.size / 2, 0));
		MazeManager.maze.tiles [(int)iniPosTile.x, (int)iniPosTile.y].obstacle = "";
		GameObject obj = Instantiate (rose) as GameObject;
		obj.transform.position = iniPos; 
		Destroy (gameObject);
	}

	void OnInteract () {
		Player.instance.Pause ();
		SoundManager.Click ();
		GameHUD.instance.riddleWindow.gameObject.SetActive (true);
	}

}

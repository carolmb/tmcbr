using UnityEngine;
using System.Collections;

public class Statue : MonoBehaviour {

	public GameObject rose;
	Vector2 iniPos;
	Animator controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<Animator> ();
		controller.SetBool ("isDying", false);
		iniPos = transform.position;
	}

	public void Explosion() {
		controller.SetBool ("isDying", true);
	}

	void Die() {
		Vector2 iniPosTile = MazeManager.WorldToTilePos (iniPos);
		MazeManager.maze.tiles [(int)iniPosTile.x, (int)iniPosTile.y - 1].objectName = "";
		GameObject obj = Instantiate (rose) as GameObject;
		obj.transform.position = iniPos; 
		Destroy (gameObject);
	}
}

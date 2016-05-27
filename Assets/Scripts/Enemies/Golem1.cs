using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem1 : Enemy {

	public GameObject rose;

	protected override void Start () {
		base.Start ();

		//MazeManager.maze.tiles [character.currentTile.x, character.currentTile.y].objectName = "Enemies/Golem1";
	}

	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else {
				ChasePlayer ();
			}
		}
	}

	protected override void OnDie() {
		MazeManager.maze.tiles [originalTile.x, originalTile.y].objectName = "";
		CaveMaze cm = (CaveMaze)MazeManager.maze;
		if (cm.type == 1) {
			List<GameObject> golens = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Golem"));
			Debug.Log (golens.Count);
			if (golens.Count == 1) {
				GameObject obj = Instantiate (rose) as GameObject;
				obj.transform.position = transform.position;
			}
		}
		base.OnDie();
	}

}

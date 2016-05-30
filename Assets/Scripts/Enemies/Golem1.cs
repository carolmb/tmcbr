using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem1 : Enemy {

	public GameObject rose;

	public bool boss = false;

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
		if (PickItem.golemCount == 4 && !SaveManager.currentSave.golemRose) {
			GameObject obj = Instantiate (rose) as GameObject;
			int width = MazeManager.maze.width;
			int height = MazeManager.maze.height;
			obj.transform.position = MazeManager.TileToWorldPos (MazeManager.maze.tiles [width / 2, height - 2].coordinates);
			SaveManager.currentSave.golemRose = true;
		}
		base.OnDie();
	}

}

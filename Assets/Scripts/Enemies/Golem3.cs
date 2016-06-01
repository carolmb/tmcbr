using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golem3 : Enemy {

	public GameObject golem2;
	public GameObject stalactite;

	protected override void Start () {
		base.Start ();
		Spawn ();
		if (SaveManager.currentSave.golemRose || (!SaveManager.currentSave.golemBossFirstTime && Random.Range(0, 100) > 15)) {
			Destroy (gameObject);
		}
		PickItem.golemCount = 0;
		SaveManager.currentSave.golemBossFirstTime = false;	
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

	void Spawn () {
		if (!character.damaging) {
			Invoke ("Earthquake", Random.Range(4, 10));
		}
		Invoke ("Spawn", 3);
	}

	void Earthquake () {
		bool lastTile = false;
		foreach(Tile t in MazeManager.maze.tiles){
			if (Random.Range (0, 100) < 10 && Random.Range(0, 100) < (lastTile ? 8 : 80)) {
				GameObject s = Instantiate (stalactite) as GameObject;
				s.transform.position = MazeManager.TileToWorldPos (t.coordinates);
				lastTile = true;
			}
			lastTile = false;
		}
		if(GameCamera.instance != null)
			GameCamera.instance.quake = true;
	}

	protected override void OnDie() {
		MazeManager.maze.tiles [originalTile.x, originalTile.y].objectName = "";
		List<Tile> t = character.currentTile.GetNeighbours4Walkeable ();
		t.Add (character.currentTile);

		GameObject spawGolem = Instantiate (golem2) as GameObject;
		Tile tile = t [Random.Range (0, t.Count)];
		spawGolem.GetComponent<Character> ().currentTile = tile;
		t.Remove (tile);

		spawGolem = Instantiate (golem2) as GameObject;
		tile = t [Random.Range (0, t.Count)];
		spawGolem.GetComponent<Character> ().currentTile = tile;

		base.OnDie ();
	}
}

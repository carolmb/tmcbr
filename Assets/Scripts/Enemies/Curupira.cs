using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Character))]
public class Curupira : Enemy {

	public static Curupira instance;

	float defaultSpeed;
	bool isChasing = false;
	public float maxDistance = 10;

	protected override void Start () {
		base.Start ();
		defaultSpeed = character.speed;
		instance = this;
	}

	void RandomMove () {
		List<Tile> possibleTiles = character.currentTile.GetNeighbours4Walkeable();
		Tile t = possibleTiles[Random.Range(0, possibleTiles.Count)];
		Vector2 nextPosition = (Vector2)MazeManager.TileToWorldPos (t.coordinates) + new Vector2 (0, Tile.size / 2);
		character.TurnTo (nextPosition);
		character.MoveTo (nextPosition, true);
	}
		
	void Update () {
		if (Player.instance.paused)
			return;

		if (!character.moving && !character.damaging) {
			if (Player.instance.repelling) {
				RunFromPlayer ();
			} else if (isChasing) {
				ChasePlayer ();
				character.speed = defaultSpeed * 3;
			} else {
				character.speed = defaultSpeed;
				RandomMove ();
			}
		}
	}

	void StartChasing () {
		ChasePlayer ();
		isChasing = true;
	}
		
	protected override void OnDamage () {
		base.OnDamage ();
		StartChasing ();
	}

	public void OnGrassCut () {
		if (PathFinder.EstimateCost(Player.instance.character.currentTile, character.currentTile) < maxDistance)
			StartChasing ();
	}

}
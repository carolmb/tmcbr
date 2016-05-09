using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMaze : ProceduralMaze {

	public CaveMaze(int id, int width, int height)  : base (id, width, height) { }

	public override string GetTheme () {
		return "Cave";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateObstacles () {

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}

			if (t.isWalkable) {
				if (EmptyRadiusToEnemies (t) && Random.Range (0, 100) < 30) {
					t.objectName = "Enemies/Bat";
				} else if (!HasObstaclesNear (t) && Random.Range (0, 100) < 50) {
					t.obstacleID = Random.Range (1, 5);
				}
			}
		}

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}
			if (t.isWall) {	
				if (Random.Range (0, 100) < 30) { //fator random
					t.obstacleID = 1;
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30) {
					t.obstacleID = 2;
					t.wallID = 0;
				}
			}
		}
	}

}

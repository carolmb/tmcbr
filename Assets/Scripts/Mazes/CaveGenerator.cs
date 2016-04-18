using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MazeGenerator {

	protected override string Theme () {
		return "Cave";
	}

	protected override Tile GetNeighbour (Tile t){
		List<Tile> n = GetVisitedNeighbours (t, 2);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateEnemies (Maze maze) {
		this.maze = maze;
		foreach (Tile t in maze.tiles) {
			if (NearBegin (t)) {
				continue;
			}

			if (NoObstaclesNear (t)) {
				if (t.isWall) {	
					if (Random.Range (1, 100) < 30) { //fator random
						t.obstacleID = 1;
						t.wallID = 0;
					} else if (Random.Range (1, 100) < 30) {
						t.obstacleID = 2;
						t.wallID = 0;
					}
				}
				if (t.isWalkable && !HasTransitionNear (t) && !maze.tiles [t.x, t.y - 1].isWall) { 
					if (Random.Range (1, 100) < 20) {
						t.objectName = "Enemies/Tomato";
					} else if (Random.Range (1, 100) < 30) { //fator random
						t.obstacleID = 1;
					} else if (Random.Range (1, 100) < 30) {
						t.obstacleID = 2;
					}
				}
			}
		}
	}

}

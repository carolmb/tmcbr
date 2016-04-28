using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestGenerator : MazeGenerator {

	protected override string Theme () {
		return "Forest";
	}

	protected override Tile GetNeighbour (Tile t){
		List<Tile> n = GetVisitedNeighbours (t, 2);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateEnemies (Maze maze) {
		this.maze = maze;
		foreach (Tile t in maze.tiles) {
			t.type = 1; // grama

			if (HasTransitionNear(t)) {
				continue;
			}
			if (Random.Range (1, 100) < 70) {
				if (t.isWalkable) {
					if (EmptyRadiusToEnemies (t)) { 
						if (Random.Range (1, 100) < 30) {
							t.objectName = "Enemies/Tomato";
						} else if (Random.Range (1, 100) < 20) {
							t.objectName = "Enemies/Butterfly";
						}
					}
				}
			} else { 
				if (t.isWalkable) {
					if (!HasObstaclesNear (t, 3)) {
						if (Random.Range (1, 100) < 50) {
							t.obstacleID = 2;
						} else if (Random.Range (1, 100) < 50) {
							t.obstacleID = 3;
						} else {
							t.obstacleID = 4;
						}
					}
				}
			}
			if (t.isWall) {	
				if (!HasObstaclesNear(t, 3) && Random.Range (1, 100) < 30) { //fator random
					t.obstacleID = 1;
				} else if (!HasObstaclesNear(t, 3) && Random.Range (1, 100) < 30) {
					t.obstacleID = 5;
					t.wallID = 0;
				}
			}
		}
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestGenerator : MazeGenerator {

	public ForestGenerator(string theme, int w, int h) : base(theme, w, h) { } 

	protected override Tile GetNeighbour(List<Tile> n){
		int i = Random.Range (0, n.Count - 1);
		return n [i];
	}

	public override void CreateEnemies (Maze maze) {
		foreach (Tile t in maze.tiles) {
			if (NearBegin (t)) {
				continue;
			}
			if (t.isWall) {	
				if (Random.Range (1, 100) < 30) { //fator random
					t.obstacleID = 1;
				} else if (Random.Range (1, 100) < 30) {
					t.obstacleID = 5;
					t.wallID = 0;
				}
			}
			if (t.isWalkable && NoObstaclesNear(t) && !HasTransitionNear(t)) { 
				if (Random.Range (1, 100) < 20) {
					t.objectName = "Tomato";
				} else if (Random.Range (1, 100) < 30) {
					t.obstacleID = 2;
				} else if (Random.Range (1, 100) < 20) {
					t.obstacleID = 3;
				} else if (Random.Range (1, 100) < 30) {
					t.obstacleID = 4;
				}
			}
		}
	}

}

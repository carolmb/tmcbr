using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestMaze : ProceduralMaze {

	public ForestMaze(int i, int w, int h) : base(i, w, h) {}

	public override string GetTheme () {
		return "Forest";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateObstacles () {

		foreach (Tile t in tiles) {
			t.type = 1; // grama

			if (HasTransitionNear(t) || t.isWall) {
				continue;
			}
			if (Random.Range (0, 100) < 75 && EmptyRadiusToEnemies (t)) {
				if (Random.Range (0, 100) < 50) {
					t.objectName = "Enemies/Tomato";
				} else if (Random.Range (0, 100) < 50) {
					t.objectName = "Enemies/Butterfly";
				}
			} else { 
				if (!HasObstaclesNear (t, 3)) {
					if (Random.Range (0, 100) < 50) {
						t.obstacleID = 2;
					} else if (Random.Range (0, 100) < 50) {
						t.obstacleID = 3;
					} else {
						t.obstacleID = 4;
					}
				}
			}

		}

		foreach (Tile t in tiles) {
			if (t.isWall) {	
				if (!HasObstaclesNear (t, 3) && Random.Range (0, 100) < 30) { //fator random
					t.obstacleID = 1;
				} else if (!HasObstaclesNear (t, 3) && Random.Range (0, 100) < 30) {
					t.obstacleID = 5;
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30 && GetAllWallNeighbours (t).Count >= 3) {
					t.wallID = 0;
				}
			}
		}
	}

}

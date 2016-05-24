using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestMaze : ProceduralMaze {

	public ForestMaze(int i, int w, int h) : base(i, w, h) {
		GenerateTiles ();
	}

	public override string GetTheme () {
		return "Forest";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	public override void CreateObstacles () {

		bool curupira = false;

		foreach (Tile t in tiles) {
			t.type = 1; // grama

			if (HasTransitionNear(t) || t.isWall) {
				continue;
			}
			if (Random.Range (0, 100) < 75 && EmptyRadiusToEnemies (t, 4)) {
				if (!curupira) {
					t.objectName = "Enemies/Curupira";
					curupira = true;
				} else if (Random.Range (0, 100) < 50) {
					t.objectName = "Enemies/Tomato";
				} else if (Random.Range (0, 100) < 100) {
					t.objectName = "Enemies/Butterfly";
				} else if (Random.Range (0, 100) < 120) {
					t.objectName = "Enemies/Mushroom";
				}
			} else { 
				if (!HasObstaclesNear (t, 3)) {
					if (Random.Range (0, 100) < 50) {
						t.obstacle = "Trunk";
					} else if (Random.Range (0, 100) < 50) {
						t.obstacle = "Log";
					} else {
						t.obstacle = "Tree";
					}
				}
			}

		}

		foreach (Tile t in tiles) {
			if (t.isWall) {	
				if (!HasObstaclesNear (t, 3) && Random.Range (0, 100) < 30) { //fator random
					t.obstacle = "Bush";
				} else if (!HasObstaclesNear (t, 3) && Random.Range (0, 100) < 30) {
					t.obstacle = "Mushroom";
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30 && GetAllWallNeighbours (t).Count >= 3) {
					t.wallID = 0;
				}
			}
		}
	}

}

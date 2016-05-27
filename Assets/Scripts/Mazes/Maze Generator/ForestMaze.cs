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
			if (Random.Range (0, 100) < 75 && EmptyRadiusToEnemies (t, 2)) {
				if (!curupira) {
					t.objectName = "Enemies/Curupira";
					curupira = true;
				} else if (Random.Range (0, 100) < 5) {
					t.objectName = "Enemies/Butterfly";
				} else if (Random.Range (0, 100) < 10) {
					t.objectName = "Enemies/Kodama";
				}
			} else { 
				if (EmptyRadiusToEnemies(t, 1)) {
					int r = Random.Range (0, 100);
					if (r < 15 && !tiles[t.x, t.y - 1].isWall) {
						t.obstacle = "Trunk";
					} else if (r < 30 && !tiles[t.x, t.y - 1].isWall) {
						t.obstacle = "Log";
					} else if (r < 45) {
						t.obstacle = "Tree";
					} else if (r < 60 && !tiles[t.x, t.y - 1].isWall) {
						if (Random.Range (0, 2) == 0)
							t.obstacle = "Mushroom";
						else
							t.obstacle = "Poison Mushroom";
					} else if (r < 75 && !tiles[t.x, t.y - 1].isWall) {
						t.obstacle = "Bush";
					}
				}
			}

		}

		foreach (Tile t in tiles) {
			if (t.isWall) {	
				if (!HasObstaclesNear (t, 1) && Random.Range (0, 100) < 30) { //fator random
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

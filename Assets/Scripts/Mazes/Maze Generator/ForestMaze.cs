using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestMaze : ProceduralMaze {

	int type; //0 -> floresta comum; 1 -> puzzle

	public ForestMaze(int i, int w, int h, int type = 0) : base(i, w, h) {
		this.type = type;
		if (type == 0)
			GenerateTiles ();
		else {
			SpecialForest ();
		}
	}

	public override string GetTheme () {
		return "Forest";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	void SpecialForest() {
		tiles = new Tile[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile (i, j);
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
					tiles [i, j].wallID = 1;
				}
			}
		}
	}

	protected void Puzzle() {
		tiles [width / 2, height / 2].obstacle = "CarnivorousPlant";
	}

	public override void CreateObstacles () {
		if (type == 1) {
			Puzzle ();
		}
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
				if (!HasObstaclesNear (t, 3) && Random.Range (0, 100) < 30) {
					t.obstacle = "Mushroom";
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30 && GetAllWallNeighbours (t).Count >= 3) {
					t.wallID = 0;
				}
			}
		}
	}

}

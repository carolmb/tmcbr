using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveMaze : ProceduralMaze {

	int type;

	public CaveMaze(int id, int width, int height, int type = 0)  : base (id, width, height) { 
		this.type = type;
		if (type == 0)
			GenerateTiles ();
		else
			SpecialCave ();
	}

	public override string GetTheme () {
		return "Cave";
	}

	protected override Tile GetNeighbour (Tile t, bool[,] visited){
		List<Tile> n = GetVisitedNeighbours (t, 2, visited);
		int i = Random.Range (0, n.Count);
		return n [i];
	}

	void SpecialCave() {
		tiles = new Tile[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile (i, j);
				if (i == 0 || i == width - 1 || j == 0 || j == height) {
					tiles [i, j].wallID = 1;
				}
			}
		}
		tiles [width / 2, height - 2].objectName = "Enemies/Golem3";
	}
		

	public override void CreateObstacles () {
		if (type == 1)
			return;
		bool boss = true;
		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}

			if (t.isWalkable) {
				if (EmptyRadiusToEnemies (t, 4) && Random.Range (0, 100) < 30) {
					t.objectName = "Enemies/Bat";
				} else if (!HasObstaclesNear (t)) {
					int r = Random.Range (0, 100);
					if (r < 20) {
						t.obstacle = "Puddle1";
					} else if (r < 15) {
						t.objectName = "Enemies/Golem1";
					} else if (r < 50) {
						t.obstacle = "Puddle2";
					} else if (r < 70) {
						t.obstacle = "Rock";
					} else if (r < 75) {
						t.obstacle = "Mushroom";
					} 
				}
			}
		}

		foreach (Tile t in tiles) {
			if (HasTransitionNear (t)) {
				continue;
			}
			if (t.isWall) {	
				if (Random.Range (0, 100) < 30) { //fator random
					t.obstacle = "Rock";
					t.wallID = 0;
				} else if (Random.Range (0, 100) < 30) {
					t.obstacle = "Mushroom";
					t.wallID = 0;
				} 
			}
		}
	}

}

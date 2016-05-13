using UnityEngine;
using System.Collections;

public class HallMaze : ProceduralMaze {

	public int type; // 0 -> correr comum; 1 -> salinha

	public HallMaze(int i, int w, int h, int type) : base(i, w, h) {
		this.type = type;
		GenerateTiles ();
	}

	public override void GenerateTiles () {
		if (type == 0)
			base.GenerateTiles ();
		else {
			GenerateRoomTiles();
		}
	}

	public override void CreateObstacles () {
		if (type == 0) {
			CreateDefaultObstacles ();
		} else {
			CreateRoomObstacles();
		}
	}

	public override string GetTheme () {
		return "Hall";
	}

	// ===============================================================================
	// Corredores aleatorios
	// ===============================================================================

	private void CreateDefaultObstacles () {
		foreach (Tile t in tiles) {
			if (!HasTransitionNear (t) && t.isWalkable) {
				if (EmptyRadiusToEnemies (t)) {
					if (GetAllWallNeighbours(t).Count > 0) { //mimics
						if (Random.Range (0, 100) < 10 && !tiles [t.x, t.y - 1].isWall) { //fator random
							t.objectName = "Enemies/Mimic";
						} else if (Random.Range (0, 100) < 30) { //fator random
							t.objectName = "Enemies/KnightArmor";
						}
					}
				} else if (!HasObstaclesNear (t)) {
					if (Random.Range (0, 100) < 15) {
						t.obstacle = "Vase";
					} else if (Random.Range (0, 100) < 15 && !tiles [t.x, t.y - 1].isWall) {
						t.obstacle = "Table";
					} else if (Random.Range (0, 100) < 15 && tiles [t.x, t.y + 1].isWall) {
						t.obstacle = "Chair";
					} else if (Random.Range (0, 100) < 50 && !tiles [t.x, t.y - 1].isWall) {
						t.obstacle = "Closed Chest";
					} else if (Random.Range (0, 100) < 60 && !tiles [t.x, t.y - 1].isWall) {
						t.obstacle = "Armour";
					}
				}
			}
		}
	}

	protected override Tile GetNeighbour(Tile t, bool[,] visited) {
		bool vertical = tiles [t.x, t.y - 1].wallID == 0 || tiles [t.x, t.y + 1].wallID == 0;
		
		if (Random.Range (0, 100) < 15) {
			vertical = !vertical;
		}
		
		if (vertical) {
			if (t.y + 2 < height - 1 && !visited [t.x, t.y + 2]) {
				return tiles [t.x, t.y + 2];
			} else if (t.y - 2 > 0 && !visited [t.x, t.y - 2]) {
				return tiles [t.x, t.y - 2];
			} else if (t.x + 2 < width - 1 && !visited [t.x + 2, t.y]) {
				return tiles [t.x + 2, t.y];
			} else {
				return tiles [t.x - 2, t.y];
			}
		} else {
			if (t.x + 2 < width - 1 && !visited [t.x + 2, t.y]) {
				return tiles [t.x + 2, t.y];
			} else if (t.x - 2 > 0 && !visited [t.x - 2, t.y]) {
				return tiles [t.x - 2, t.y];
			} else if (t.y + 2 < height - 1 && !visited [t.x, t.y + 2]) {
				return tiles [t.x, t.y + 2];
			} else {
				return tiles [t.x, t.y - 2];
			}
		}
	}

	// ===============================================================================
	// Salinhas com mesas e estantes
	// ===============================================================================

	private void CreateRoomObstacles () {
		// Mesinha
		int tableX = width / 2 - 1;
		int tableY = height / 2 - 1;
		tiles [tableX, tableY].obstacle = "Dinner3";
		tiles [tableX + 1, tableY].obstacle = "Dinner4";
		tiles [tableX, tableY + 1].obstacle = "Dinner1";
		tiles [tableX + 1, tableY + 1].obstacle = "Dinner2";

		// Estantes
		//TODO

		// Baus e mimic
		//TODO
	}

	private void GenerateRoomTiles () {
		// Criar sala vazia
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				tiles[i, j] = new Tile(i, j);
				tiles[i, j].floorID = 1;

				if (j == 0 || j == height - 1 || i == 0 || i == width - 1)
					tiles[i, j].wallID = 1;
				else
					tiles[i, j].wallID = 0;
			}
		}
	}
}

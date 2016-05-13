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
		// Gerar sequências de armaduras
		for (int k = Random.Range ((width + height) / 8, (width + height) / 4); k >= 0; k--) {
			int x = Random.Range (0, (width / ProceduralStage.expansionFactor) / 2);
			x = ProceduralStage.expansionFactor * (1 + x * 2); 
			int y = Random.Range (0, (height / ProceduralStage.expansionFactor) / 2);
			y = ProceduralStage.expansionFactor * (1 + y * 2) + 1; 

			int dx, dy;
			if (Random.Range (0, 2) == 0) {
				dx = Random.Range (0, 2) * 2 - 1; // -1 ou 1 
				dy = 0;
			} else {
				dy = Random.Range (0, 2) * 2 - 1; // -1 ou 1 
				dx = 0;
			}

			for (dx *= 2, dy *= 2; x >= 0 && x < width && y >= 0 && y < height; x += dx, y += dy) {
				Tile t = tiles [x, y];
				if (!t.isWalkable || GetAllWallNeighbours(t).Count != 1)
					break;
				if (Random.Range (0, 100) < 10) {
					t.objectName = "Enemies/KnightArmor";
				} else {
					t.obstacle = "Armor";
				}
			}
		}

		// Gerar paredes com quadros
		// TODO
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

	public void ChangeType () {
		type = 1;
		foreach (Tile t in tiles) {
			if (t.coordinates.x > 2 && t.coordinates.y > 2 && t.coordinates.x < width - 3 && t.coordinates.y < height - 3) {
				t.wallID = 0;
			}
		}
	}
}

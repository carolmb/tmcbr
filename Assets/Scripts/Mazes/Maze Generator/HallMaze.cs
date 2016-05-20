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
			x = ProceduralStage.expansionFactor * (1 + x * 2) + Random.Range(0, 2); 
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

			for (dx *= 2, dy *= 2; x > 0 && x < width - 1 && y > 0 && y < height - 1; x += dx, y += dy) {
				Tile t = tiles [x, y];
				if (!CanPutArmor(t))
					break;
				if (Random.Range (0, 100) < 10) {
					t.objectName = "Enemies/KnightArmor";
				} else {
					t.obstacle = "Armor";
				}
			}
		}

		foreach (Tile t in tiles) {
			if (t.isWall && t.y > 0 && tiles[t.x, t.y - 1].obstacle == "") {
				bool nearPainting = false;
				foreach (Tile t2 in GetAllWallNeighbours(t)) {
					if (t2.wallID > 1) {
						nearPainting = true;
						break;
					}
				}
				if (!nearPainting) {
					if (Random.Range (0, 100) < 5) {
						t.wallID = 2;
					} else if (Random.Range (0, 100) < 5) {
						t.wallID = 3;
					} else if (Random.Range (0, 100) < 5) {
						t.wallID = 4;
					} else if (Random.Range (0, 1000) < 1) {
						t.wallID = 5;
					}
				}
			}
		}

	}

	// Checa se dá pra colocar uma armadura
	private bool CanPutArmor(Tile t) {
		if (!t.isWalkable || !EmptyRadiusToEnemies(t, 1))
			return false;

		if (!(t.x > 1 && t.x < width - 2 && t.y > 1 && t.y < height - 2))
			return false;

		if (tiles [t.x - 1, t.y].isWall || tiles [t.x + 1, t.y].isWall) { // Se for vizinho a parede na horizontal
			if (!tiles [t.x, t.y + 1].isWalkable || !tiles [t.x, t.y - 1].isWalkable || 
				tiles [t.x, t.y + 2].isWall || tiles [t.x, t.y - 2].isWall)
				return false;
			else
				return true;
		} else if (tiles [t.x, t.y - 1].isWall || tiles [t.x, t.y + 1].isWall) {
			if (!tiles [t.x + 1, t.y].isWalkable || !tiles [t.x - 1, t.y].isWalkable || 
				tiles [t.x + 2, t.y].isWall || tiles [t.x - 2, t.y].isWall)
				return false;
			else
				return true;
		} else {
			return false;
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

		for (int i = 2; i <= width - 3; i++) {
			if (Random.Range (0, 100) < 50) {
				while (i <= width - 3 && Random.Range(0, 100) < 70 && tiles [i, height - 2].isWall) {
					tiles [i, height - 3].obstacle = "Shelf";
					i ++;
				}
			}
		}

		for (int i = 2; i <= width - 3; i++) {
			if (tiles [i, height - 3].obstacle == "" && Random.Range (0, 100) < 50 && tiles [i, height - 2].isWall) {
				if (Random.Range(0, 100) < 20) {
					tiles [i, height - 3].objectName = "Enemies/Mimic";
				} else {
					tiles [i, height - 3].obstacle = "Closed chest";
				}
			}
		}
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

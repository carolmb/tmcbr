using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class Maze {

	public int id;
	public Tile[,] tiles;

	public Maze(int id, int width, int height) {
		this.id = id;
		this.tiles = new Tile[width, height];
	}
		
	public abstract string GetTheme ();

	// Tamanho em tiles
	public int width { get { return tiles.GetLength (0); } }
	public int height { get { return tiles.GetLength (1); } }

	// Tamanho em coordenadas do jogo
	public float worldWidth { get { return width * Tile.size; }	}
	public float worldHeight { get { return height * Tile.size; } }

	public Vector2 center { get { return new Vector2 (width / 2, height / 2); } }

	public List<Tile> GetNeighbours (Tile tile, int delta) {
		List<Tile> neighbours = new List<Tile> ();
		for (int i = tile.x - delta; i < tile.x + delta; i++) {
			for (int j = tile.y - delta; j < tile.y + delta; j++) {
				if (i >= 0 && i < width && j >= 0 && j < height) {
					neighbours.Add (tiles [i, j]);
				}
			}
		}
		return neighbours;
	}

}

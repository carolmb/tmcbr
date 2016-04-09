using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Maze : IEnumerable {

	public int id;
	public Tile[,] tiles;

	// Para saber qual gráfico vai ser utilizado
	public string theme = "Hall";

	public Maze(int id, string theme, int width, int height) {
		this.id = id;
		this.tiles = new Tile[width, height];
		this.theme = theme;
	}

	// Tamanho em tiles
	public int width { get { return tiles.GetLength (0); } }
	public int height { get { return tiles.GetLength (1); } }

	// Tamanho em coordenadas do jogo
	public float worldWidth { get { return width * Tile.size; }	}
	public float worldHeight { get { return height * Tile.size; } }

	public Vector2 center { get { return new Vector2 (width / 2, height / 2); } }

	public IEnumerator GetEnumerator() {
		return tiles.GetEnumerator ();
	}

}

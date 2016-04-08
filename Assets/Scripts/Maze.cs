using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Maze : IEnumerable {

	// TODO: colocar também as transições do tipo Tile (representado por um x, y) -> Maze (representdo por um ID)

	public int id = -1;
	public Tile[,] tiles;
	public Vector2 beginCoord;

	// Para saber qual gráfico vai ser utilizado
	public string theme = "Hall";

	public Maze(string theme, int width, int height) {
		tiles = new Tile[width, height];
		this.theme = theme;
	}

	// Tamanho em tiles
	public int width { get { return tiles.GetLength (0); } }
	public int height { get { return tiles.GetLength (1); } }

	// Tamanho em coordenadas do jogo
	public float worldWidth { get { return width * Tile.size; }	}
	public float worldHeight { get { return height * Tile.size; } }

	public IEnumerator GetEnumerator() {
		return tiles.GetEnumerator ();
	}

}

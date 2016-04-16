using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Maze : IEnumerable {

	public int id;
	public Tile[,] tiles;

	public Tile beginMaze;
	public Tile endMaze;
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

	// Procura uma transição em todos os tiles na reta X 
	// Use x = 0 para a lateral esquerda, x = width - 1 para a lateral direita
	public Transition FindTransitionInX(int x) {
		for (int y = 1; y < height - 1; y++) {
			if (tiles [x, y].transition != null)
				return tiles [x, y].transition;
		}
		return null;
	}

	// Procura uma transição em todos os tiles na reta Y
	// Use y = 0 para a base e y = height - 1 para o topo
	public Transition FindTransitionInY(int y) {
		for (int x = 1; x < width - 1; x++) {
			if (tiles [x, y].transition != null)
				return tiles [x, y].transition;
		}
		return null;
	}

	public IEnumerator GetEnumerator() {
		return tiles.GetEnumerator ();
	}

}

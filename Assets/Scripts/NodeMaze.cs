using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeMaze {

	/*const int MIN = 10; //tamanho possuivel dos sublabirintos
	const int MAX = 15;

	Tile iniTile;
	Tile finalTile;
	string theme;
	int direction;
	//cena inicial

	public Maze[,] mazes;
	int size;

	public NodeMaze (string theme, Tile iniTile, int direction) {
		this.iniTile = iniTile;
		this.theme = theme;
		this.direction = direction;
		InicializeMazes ();
		ExpandMazes ();
	}

	void ExpandMazes() {
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				MazeGenerator.CreateEnemysHall (mazes [i, j]);
				MazeGenerator.ExpandMaze(mazes [i, j], 2, 2);
			}
		}
	}

	void InicializeMazes () {
		int id = 0;
		size = Random.Range (MIN, MAX);
		Debug.Log ("size: " + size);
		mazes = new Maze[size, size];
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++, id++) {
				mazes [i, j] = MazeGenerator.CreateMaze (id, theme, Random.Range(MIN, MAX), Random.Range(MIN, MAX));
				if (mazes [i, j] == null) {
					Debug.Log ("é null");
				}
			}
		}
		finalTile = CreateTransitions ();
	}

	string SelectNextMaze (int i, int j) {
		if (i < size && j + 1 < size) {
			int aux = Random.Range (0, 1);
			if (aux == 1) {
				return "south";
			} else {
				return "east";
			}
		} else {
			return "south";
		}
	}


	//norte-sul
	Tile CreateTransitions () {
		Tile final = null, dest;
		int globalX = 0;
		int globalY = 0;

		final = MazeGenerator.CreateExitSouth (mazes [globalX, globalY]);
		if (mazes [globalX, globalY].height > final.y) {
			mazes [globalX, globalY].tiles [0, final.y].isWall = false;
			dest = mazes [globalX, globalY].tiles [1, final.y];
		} else {
			mazes [globalX, globalY].tiles [0, mazes [globalX, globalY].height - 1].isWall = false;
			dest = mazes [globalX, globalY].tiles [1, mazes [globalX, globalY].height - 1];
		}

		Debug.Log (final.coordinates);
		while (globalY < size) {

			if (SelectNextMaze(globalX, globalY) == "east") {
				final = MazeGenerator.CreateExitEast (mazes [globalX, globalY]);		//cria saida ao leste

				globalX += 1;
				//precisa testar se está dentro das margens do lab
				if (mazes [globalX, globalY].width > final.x) {
					mazes [globalX, globalY].tiles [final.x, 0].isWall = false;
					dest = mazes [globalX, globalY].tiles [final.x, 1];
				} else {
					mazes [globalX, globalY].tiles [mazes [globalX, globalY].width - 1, 0].isWall = false;
					dest = mazes [globalX, globalY].tiles [mazes [globalX, globalY].width - 1, 1];
				}

				if (globalX < size) {
					MazeGenerator.SetTransition (final, dest, mazes [globalX - 1, globalY], mazes [globalX, globalY]);
				}
			} else {
				final = MazeGenerator.CreateExitSouth (mazes [globalX, globalY]);		//cria saida ao sul

				globalY += 1;
				//precisa testar se está dentro das margens do lab

				if (mazes [globalX, globalY].height > final.y) {
					mazes [globalX, globalY].tiles [0, final.y].isWall = false;
					dest = mazes [globalX, globalY].tiles [1, final.y];
				} else {
					mazes [globalX, globalY].tiles [0, mazes [globalX, globalY].height - 1].isWall = false;
					dest = mazes [globalX, globalY].tiles [1, mazes [globalX, globalY].height - 1];
				}
					
				if (globalY < size) {
					MazeGenerator.SetTransition (final, dest, mazes [globalX, globalY - 1], mazes [globalX, globalY]);
				}
			}
		}
	return final;
	//primeiro cria um caminho que garante uma saída para o labirinto
	//posteriormente cria caminhos para os demais sublabs	
	}*/
}

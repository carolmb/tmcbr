using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	public static Texture2D texture;

	public static Color visitedColor = Color.white;
	public static Color hiddenColor = Color.black;
	public static Color playerColor = Color.magenta;
	public static Color transitionColor = Color.blue;

	public static void Update(Tile previousTile) {
		if (texture == null) {
			GenerateTexture ();
		} else {
			Tile currentTile = Player.instance.character.currentTile;
			if (previousTile.transition != null && previousTile.transition.instant) {
				texture.SetPixel (previousTile.x + 1, previousTile.y + 1, transitionColor);
			} else {
				texture.SetPixel (previousTile.x + 1, previousTile.y + 1, visitedColor);
			}
			texture.SetPixel (currentTile.x + 1, currentTile.y + 1, playerColor);
			texture.Apply ();
		}
	}

	public static void GenerateTexture () {
		texture = new Texture2D (MazeManager.maze.width + 2, MazeManager.maze.height + 2, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Point;

		for (int i = 0; i < texture.width; i++) {
			for (int j = 0; j < texture.height; j++) {
				texture.SetPixel (i, j, hiddenColor);
			}
		}

		for (int i = 0; i < MazeManager.maze.width; i++) {
			for (int j = 0; j < MazeManager.maze.height; j++) {
				if (MazeManager.maze.tiles [i, j].transition != null && MazeManager.maze.tiles [i, j].transition.instant) {
					texture.SetPixel (i + 1, j + 1, transitionColor);
				} else if (MazeManager.maze.tiles [i, j].visited) {
					texture.SetPixel (i + 1, j + 1, visitedColor);
				}
			}
		}
		Tile t = Player.instance.character.currentTile;
		texture.SetPixel (t.x + 1, t.y + 1, playerColor);
		texture.Apply ();
	}

}

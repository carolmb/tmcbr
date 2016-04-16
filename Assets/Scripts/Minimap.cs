using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

	Image imageComp;
	Texture2D texture;

	public Color visitedColor = Color.white;
	public Color hiddenColor = Color.black;
	public Color playerColor = Color.blue;
	public Color transitionColor = Color.yellow;

	void OnEnable() {
		imageComp = GetComponent<Image> ();
		texture = new Texture2D (MazeManager.maze.width, MazeManager.maze.height);
		texture.filterMode = FilterMode.Point;

		for (int i = 0; i < texture.width; i++) {
			for (int j = 0; j < texture.height; j++) {
				if (MazeManager.maze.tiles [i, j].transition != null) {
					texture.SetPixel (i, j, transitionColor);
				} else { 
					if (MazeManager.maze.tiles [i, j].visited) {
						texture.SetPixel (i, j, visitedColor);
					} else {
						texture.SetPixel (i, j, hiddenColor);
					}
				}
				Tile t = Player.instance.character.currentTile;
				texture.SetPixel (t.x, t.y, playerColor);
			}
		}
		texture.Apply ();
		imageComp.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		imageComp.rectTransform.sizeDelta = new Vector2 (texture.width * 4, texture.height * 4);
		SetPivot (0.5f, 0.5f);
	}

	private void SetPivot(float x, float y) {
		imageComp.rectTransform.pivot = new Vector2 (x, y);
	}

	public int speed = 2;
	float px = 0.5f; 
	float py = 0.5f;

	private void Update() {
		float x = -Input.GetAxisRaw("Horizontal");
		float y = -Input.GetAxisRaw ("Vertical");
		if (x != 0 || y != 0) {
			px += x * speed / texture.width;
			px = Mathf.Min (1, px);
			px = Mathf.Max (0, px);

			py += y * speed / texture.height;
			py = Mathf.Min (1, py);
			py = Mathf.Max (0, py);

			SetPivot (px, py);
		}
	}

}

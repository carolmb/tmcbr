using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapWindow : MonoBehaviour {

	public GameObject miniMap;

	Image imageComp;

	public static Color visitedColor = Color.white;
	public static Color hiddenColor = Color.black;
	public static Color playerColor = Color.green;
	public static Color transitionColor = Color.yellow;

	void Awake() {
		imageComp = miniMap.GetComponent<Image> ();
	}

	public static void UpdateTexture(Image imageComp, int size = 4) {
		Texture2D texture = new Texture2D (MazeManager.maze.width, MazeManager.maze.height);
		texture.filterMode = FilterMode.Point;

		for (int i = 0; i < texture.width; i++) {
			for (int j = 0; j < texture.height; j++) {
				if (MazeManager.maze.tiles [i, j].transition != null && MazeManager.maze.tiles [i,j].transition.instant) {
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
		imageComp.rectTransform.sizeDelta = new Vector2 (texture.width * size, texture.height * size);
		SetPivot (imageComp, 0.5f, 0.5f);
	}

	private static void SetPivot(Image image, float x, float y) {
		image.rectTransform.pivot = new Vector2 (x, y);
	}

	public int speed = 2;
	float px = 0.5f; 
	float py = 0.5f;

	private void Update() {
		float x = -Input.GetAxisRaw("Horizontal");
		float y = -Input.GetAxisRaw ("Vertical");
		if (x != 0 || y != 0) {
			px += x * speed / imageComp.rectTransform.sizeDelta.x;
			px = Mathf.Min (1, px);
			px = Mathf.Max (0, px);

			py += y * speed / imageComp.rectTransform.sizeDelta.y;
			py = Mathf.Min (1, py);
			py = Mathf.Max (0, py);

			SetPivot (imageComp, px, py);
		}
	}

	public void Return() {
		SoundManager.Click ();
		GameHUD.instance.mainMenu.mainWindow.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}

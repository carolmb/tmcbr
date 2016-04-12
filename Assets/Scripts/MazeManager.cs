using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour {

	public static Maze maze; // Se quiser acesso a qualquer informação do labirinto, use isso

	// ===============================================================================
	// Transição entre labirintos
	// ===============================================================================

	public static Transition currentTransition {
		get { return SaveManager.currentSave.transition; }
	}

	public static void GoToMaze(Transition transition) {
		SaveManager.currentSave.transition = transition;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	// ===============================================================================
	// Criação dos tiles
	// ===============================================================================

	// Coloca o player na posição inicial
	void Start() {
		Vector2 tilePos = new Vector2 (currentTransition.tileX, currentTransition.tileY);
		Vector2 initPos = TileToWorldPosition (tilePos) + new Vector3 (0, Tile.size / 2, 0);
		Player.instance.transform.position = initPos;
		Player.instance.character.direction = currentTransition.direction;
	}

	// Resgata o labirinto atual e inicializa os tiles
	void Awake () {
		if (SaveManager.currentSave == null) {
			SaveManager.NewGame ();
		}
		maze = SaveManager.currentSave.mazes [currentTransition.mazeID];
		Debug.Log ("id: " + currentTransition.mazeID);
		foreach (Tile t in maze) {
			GameObject floor = CreateTileGraphic (t.x, t.y, "floor");
			Vector3 pos = floor.transform.position;
			pos.z = 999;
			floor.transform.position = pos;

			if (t.isWall) {
				CreateTileGraphic (t.x, t.y, "wall").transform.Translate (0, 0, 1);
			}
			if (t.obstacle >= 0) {
				CreateTileGraphic (t.x, t.y, "obstacle" + t.obstacle);
			}
			if (t.objectName != "") {
				CreateTileObject (t.x, t.y, t.objectName);
			}
		}
	}

	// Criar objeto do tile	(prefab do item/inimigo deve estar na pasta Resources/Prefabs)
	GameObject CreateTileObject(int x, int y, string prefabName) {
		GameObject prefab = Resources.Load<GameObject> ("Prefabs/" + prefabName);
		GameObject obj = Instantiate (prefab);
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y)) + new Vector3(0, Tile.size / 2, 0);
		obj.transform.SetParent (transform);
		obj.name = prefabName;
		return obj;
	}

	// Criar gráficos do tile
	GameObject CreateTileGraphic(int x, int y, string spriteName) {
		GameObject obj = new GameObject ();
		obj.name = "Tile[" + spriteName + "] (" + x + ", " + y + ")";
		obj.transform.position = TileToWorldPosition (new Vector2 (x, y));
		SpriteRenderer sr = obj.AddComponent<SpriteRenderer> ();
		sr.sprite = Resources.Load<Sprite> ("Images/Tilesets/" + maze.theme + "/" + spriteName);
		obj.transform.SetParent (transform);
		return obj;
	}

	// ===============================================================================
	// Posição dos tiles
	// ===============================================================================

	// Converte coordenada em tiles para posição de jogo (em pixels)
	public static Vector3 TileToWorldPosition(Vector2 tilePos) {
		tilePos = tilePos * Tile.size;
		return new Vector3 (tilePos.x, tilePos.y, tilePos.y);
	}

	// Converte posição de jogo (pixels) para coordenada em tiles
	public static Vector2 WorldToTilePos(Vector2 worldPos) {
		Vector2 tilePos = new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
		//Debug.Log ("world: " + worldPos);
		//Debug.Log ("tile: " + tilePos);
		return tilePos;
	}

	// Arredonda uma coordenada para um múltiplo de Tile.size
	public static int RoundToTile(float value) {
		return Mathf.RoundToInt (value / Tile.size) * Tile.size;
	}

	// Pega o tile na dada posição
	public static Tile GetTile(Vector2 position) {
		Vector2 p = WorldToTilePos(position);
		if (p.x < 0 || p.x >= maze.width || p.y < 0 || p.y >= maze.height) {
			return null;
		}
		return maze.tiles [(int)p.x, (int)p.y];
	}

	// Verifica de um ponto está colidindo com algum tile
	public static bool Collides(float x, float y) {
		Vector2 p = WorldToTilePos(new Vector2 (x, y));
		if (p.x < 0 || p.x >= maze.width || p.y < 0 || p.y >= maze.height) {
			return true;
		}
		return maze.tiles [(int)p.x, (int)p.y].isWalkable == false;
	}
		
}
	
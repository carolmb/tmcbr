using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour {

	public static Maze maze; // Se quiser acesso a qualquer informação do labirinto, use isso
	public static BoxCollider2D[,] obstacles;
	public static AudioSource musicPlayer;
	public static string lastMaze;
	public static SpriteRenderer[,] floorSprites;

	// ===============================================================================
	// Transição entre labirintos
	// ===============================================================================

	public static Tile.Transition currentTransition {
		get { return SaveManager.currentSave.transition; }
	}

	public static void GoToMaze(Tile.Transition transition) {
		lastMaze = maze != null ? maze.GetTheme () : "";
		SaveManager.currentSave.transition = transition;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	// ===============================================================================
	// Criação dos tiles
	// ===============================================================================

	// Coloca o player na posição inicial
	void Start() {
		if (Player.instance != null) {
			Vector2 tilePos = new Vector2 (currentTransition.tileX, currentTransition.tileY);
			Vector2 initPos = TileToWorldPos (tilePos) + new Vector3 (0, Tile.size / 2, 0);
			Player.instance.transform.position = initPos;
			Player.instance.character.direction = currentTransition.direction;
		}
		Minimap.GenerateTexture ();
		GameHUD.instance.UpdateMap ();
	}

	// Resgata o labirinto atual e inicializa os tiles
	void Awake () {
		Input.simulateMouseWithTouches = true;

		if (SaveManager.currentSave == null) {
			SaveManager.NewGame ();
		}
		maze = SaveManager.currentSave.mazes [currentTransition.mazeID];
		obstacles = new BoxCollider2D[maze.width, maze.height];
		floorSprites = new SpriteRenderer[maze.width, maze.height];
		foreach (Tile t in maze.tiles) {
			CreateTileFloor (t.x, t.y, t.floorID);

			if (t.wallID > 0) {
				CreateTileWall (t.x, t.y, t.wallID);
			}
			if (t.obstacle != "") {
				CreateTileObstacle (t.x, t.y, t.obstacle);
			} else if (t.objectName != "" && t.canSpawn) {
				CreateTileObject (t.x, t.y, t.objectName);
				t.lastSpawn = SaveManager.currentPlayTime;
			}
		}
		PlayMusic ();
	}

	// Cria o sprite de chão
	GameObject CreateTileFloor(int x, int y, int id) {
		SpriteRenderer floor = CreateTileGraphic (x, y, "floor" + id);
		Vector3 pos = floor.transform.position;
		pos.z += Tile.size * 2;
		floor.transform.position = pos;
		floorSprites [x, y] = floor;
		return floor.gameObject;
	}

	// Cria o sprite de parede
	GameObject CreateTileWall(int x, int y, int id) {
		SpriteRenderer g = CreateTileGraphic (x, y, "wall" + id);
		g.transform.Translate (0, 0, 1);
		return g.gameObject;
	}

	// Cria objeto do tile (prefab do item/inimigo deve estar na pasta Resources/Prefabs)
	GameObject CreateTileObject(int x, int y, string prefabName) {
		Vector3 pos = TileToWorldPos (new Vector2 (x, y)) + new Vector3(0, Tile.size / 2, Tile.size / 2);
		GameObject prefab = Resources.Load<GameObject> ("Prefabs/" + prefabName);
		if (prefab == null)
			Debug.Log ("null prefab: Prefabs/" + prefabName);
		GameObject obj = (GameObject) Instantiate(prefab, pos, Quaternion.identity);
		obj.transform.SetParent (transform);
		obj.name = prefabName;
		return obj;
	}

	// Criar prefabs de obstáculos fora da pasta de obstacles 
	GameObject CreateTileObstacle(int x, int y, string name) {
		GameObject g = CreateTileObject (x, y, "Obstacles/" + maze.GetTheme() + "/" + name);
		g.name = "Tile[" + name + "] (" + x + ", " + y + ")";
		obstacles [x, y] = g.GetComponent<BoxCollider2D> ();
		return g;
	}
		
	// Cria um objeto fazia com um sprite na posição do tile
	SpriteRenderer CreateTileGraphic(int x, int y, string spriteName) {
		GameObject obj = new GameObject ();
		obj.name = "Tile[" + spriteName + "] (" + x + ", " + y + ")";
		obj.transform.position = TileToWorldPos (new Vector2 (x, y));
		SpriteRenderer sr = obj.AddComponent<SpriteRenderer> ();
		sr.sprite = Resources.Load<Sprite> ("Images/Tilesets/" + maze.GetTheme() + "/" + spriteName);
		obj.transform.SetParent (transform);
		return sr;
	}

	// ===============================================================================
	// Posição dos tiles
	// ===============================================================================

	// Converte coordenada em tiles para posição de jogo (em pixels)
	public static Vector3 TileToWorldPos(Vector2 tilePos) {
		tilePos = tilePos * Tile.size;
		return new Vector3 (tilePos.x, tilePos.y, tilePos.y);
	}

	// Converte posição de jogo (pixels) para coordenada em tiles
	public static Vector2 WorldToTilePos(Vector2 worldPos) {
		Vector2 tilePos = new Vector2(Mathf.Round(worldPos.x / Tile.size), Mathf.Round(worldPos.y / Tile.size));
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
		Tile t = maze.tiles [(int)p.x, (int)p.y];
		if (t.isWall) {
			return true;
		}
		return CollidesObstacle (t, x, y);
	}

	// Verifica se o ponto (x, y) colide com algum obstaculo no tile t
	public static bool CollidesObstacle(Tile t, float x, float y) {
		if (obstacles [t.x, t.y] == null) {
			return false;
		}
		Vector2 center = (Vector2)TileToWorldPos (t.coordinates);
		BoxCollider2D boxCollider = obstacles [t.x, t.y];

		float left 		= center.x - boxCollider.size.x / 2 + boxCollider.offset.x;
		float right 	= center.x + boxCollider.size.x / 2 + boxCollider.offset.x;
		float bottom 	= center.y - boxCollider.size.y / 2 + boxCollider.offset.y;
		float top 		= center.y + boxCollider.size.y / 2 + boxCollider.offset.y;

		return (x <= right && x >= left) && (y <= top && y >= bottom);
	}

	// ===============================================================================
	// Musica do labirinto
	// ===============================================================================

	private void PlayMusic() {
		// Se não já foi criado
		if (musicPlayer == null) {
			GameObject go = new GameObject ();
			go.name = "Music Player";
			musicPlayer = go.AddComponent<AudioSource> ();
			musicPlayer.loop = true;
			musicPlayer.playOnAwake = false;
			DontDestroyOnLoad (go);
		}

		// 
		if (lastMaze != maze.GetTheme ()) {
			AudioClip clip = Resources.Load<AudioClip> ("Sounds/Music/" + maze.GetTheme ());
			if (clip != null) {
				musicPlayer.clip = clip;
				musicPlayer.Play ();
			} else {
				musicPlayer.Stop ();
			}
		}
	}

	void Update () {
		musicPlayer.transform.position = GameCamera.instance.transform.position;
	}

}
	
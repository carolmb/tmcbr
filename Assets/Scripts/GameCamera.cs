using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public static GameCamera instance;
	public Transform player;
	public Shader shader;
	//public Material invert;

	void Start() {
		//InvertColors ();
	}

	// Limites do cenário
	private float maxX { get { return MazeManager.maze.worldWidth - size.x / 2 - Tile.size / 2; } }
	private float minX { get { return size.x / 2 - Tile.size / 2; }	}
	private float maxY { get { return MazeManager.maze.worldHeight - size.y / 2; } }
	private float minY { get { return size.y / 2; } }

	// Para acesso à camera por outras classes
	void Awake() {
		instance = this;
	}

	// Tamanho da câmera (em coordenadas do jogo)
	public static Vector2 size {
		get {
			float h = Camera.main.orthographicSize * 2;
			float w = Camera.main.aspect * h;
			return new Vector2(w, h);
		}
	}

	// Seguir o player
	void LateUpdate() {
		Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		AdjustPosition (ref newPos);
		transform.position = newPos;
	}

	// Serve para "cortar" uma posição afim de mantê-la dentro dos limites do cenário
	public bool AdjustPosition(ref Vector3 newPos) {
		bool moved = false;
		newPos.z = transform.position.z;
		if (newPos.x >= maxX) {
			newPos.x = maxX;
		} else if (newPos.x <= minX) {
			newPos.x = minX;
		} else {
			if (transform.position.x != newPos.x)
				moved = true;
		}

		if (newPos.y >= maxY) {
			newPos.y = maxY;
		} else if (newPos.y <= minY) {
			newPos.y = minY;
		} else {
			if (transform.position.y != newPos.y)
				moved = true;
		}

		return moved;
	}
		
	// ===============================================================================
	// Sons
	// ===============================================================================

	public static void PlayAudioClip(AudioClip clip, float volume = 1) {
		GameObject go = new GameObject ();
		go.transform.SetParent (instance.transform);
		go.transform.localPosition = Vector3.zero;

		AudioSource audio = go.AddComponent<AudioSource> ();
		audio.clip = clip;
		audio.volume = volume;
		audio.Play ();

		Destroy (go, clip.length);
	}

	// ===============================================================================
	// Efeito da luz
	// ===============================================================================

	public Material lampMaterial;

	void OnRenderImage(RenderTexture source, RenderTexture dest) {
		Graphics.Blit (source, dest, lampMaterial);
	}

	public void InvertColors() {
		Camera.main.SetReplacementShader (shader, "Queue");
	}
}

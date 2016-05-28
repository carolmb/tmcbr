using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameCamera : MonoBehaviour {

	public static GameCamera instance;
	public Transform player;
	public bool quake;

	// Limites do cenário
	private float maxX { get { return MazeManager.maze.worldWidth - size.x / 2 - Tile.size / 2; } }
	private float minX { get { return size.x / 2 - Tile.size / 2; }	}
	private float maxY { get { return MazeManager.maze.worldHeight - size.y / 2; } }
	private float minY { get { return size.y / 2; } }

	// Para acesso à camera por outras classes
	void Awake() {
		quake = false;
		instance = this;
		FadeEffect.intensity = 0;
		//MushroomEffect ();
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
		if (player == null)
			return;

		Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		if(quake)
			newPos += new Vector3 (Random.Range (-10, 11), Random.Range (-10, 11), 0);

		AdjustPosition (ref newPos);
		transform.position = newPos;
		Invoke ("Quake", 2);

	}
		
	void Quake() {
		quake = false;
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
	// Efeitos
	// ===============================================================================

	private static Coroutine mushRoom;

	public void MushroomEffect() {
		if (mushRoom != null) {
			StopCoroutine (mushRoom);
		} else {
			ColorInverter.angle = 360;
			Twirl.angle = 45;
			Player.inputFactor = -1;
		}
		mushRoom = StartCoroutine (MushroomEffect_coroutine ());
	}

	private IEnumerator MushroomEffect_coroutine() {
		while (Twirl.angle > 0) {
			if (!Player.instance.paused) {
				if (Player.instance.moved) {
					yield return new WaitForSeconds (2);
				} else {
					Twirl.angle -= Time.deltaTime * 3;
				}
			}
			yield return null;
		}
		Twirl.angle = 0;
		ColorInverter.angle = 0;
		Player.inputFactor = 1;
	}

	public IEnumerator FadeOut(float speed) {
		while (FadeEffect.intensity < 1) {
			FadeEffect.intensity += Time.deltaTime * speed;
			yield return null;
		}
		FadeEffect.intensity = 1;
	}

	public IEnumerator FadeIn(float speed) {
		while (FadeEffect.intensity > 0) {
			FadeEffect.intensity -= Time.deltaTime * speed;
			yield return null;
		}
		FadeEffect.intensity = 0;
	}

}

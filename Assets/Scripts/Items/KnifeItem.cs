using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class KnifeItem : MonoBehaviour {

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 5;

	// Tempo máximo até ser destruído
	public float lifeTime = 2;

	public AudioClip sound;

	void Start () {
		GameCamera.PlayAudioClip (sound, 0.5f);

		float posx = 0, posy = 0, posz = 0;
		switch (Player.instance.character.direction) {
			case 0:
				transform.Rotate(0, 0,-120);
				//posy = Tile.size/4;
				break;
			case 1:
				transform.Rotate (0, 0, 150);
				posy = Tile.size/4;
				posx = -Tile.size/4;
				break;
			case 2:
				transform.Rotate (0, 0, -30);
				posx = Tile.size/4;
				posy = Tile.size/4;
				break;
			case 3:
				transform.Rotate (0, 0, 60);
				posy = Tile.size/1.8f;
				posz = 1;
				break;
		}
		transform.position = Player.instance.transform.position + new Vector3(posx, posy, posz);
		Destroy (gameObject, lifeTime);
	}

	void Update () {
		// Atacar
//		Tile t = MazeManager.GetTile ((Vector2)transform.position - new Vector2 (0, Tile.size)); 
		Quaternion target = Quaternion.Euler(0, 0, 0);
		switch (Player.instance.character.direction) {
		case 0:
			Debug.Log("0");
			target = Quaternion.Euler(0, 0, -30);
			break;
		case 1:
			Debug.Log("1");
			target = Quaternion.Euler(0, 0, 240);
			break;
		case 2:
			Debug.Log("2");
			target = Quaternion.Euler(0, 0, 30);
			break;
		case 3:
			Debug.Log("3");
			target = Quaternion.Euler(0, 0, 120);
			break;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 42);
		Invoke ("DestroyKnife", 0.1f);
	}

	void DestroyKnife() {
		Destroy (gameObject);
	}

	// Verifica se um inimigo foi atingido
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Enemy")) {
			Character comp = collider.GetComponent<Character> ();
			comp.StartCoroutine(comp.Damage (transform.position, damage));
			Destroy (gameObject);
		}
	}
}

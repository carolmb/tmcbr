using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Rock : MonoBehaviour {

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 5;

	// Tempo máximo até ser destruído
	public float lifeTime = 5;

	// Direção do movimento
	private Vector2 moveVector;

	void Start() {
		transform.position = Player.instance.transform.position + new Vector3(0, Tile.size / 2, 0);
		moveVector = GameManager.AngleToVector (Player.instance.character.lookingAngle) * speed;
	}

	void Update () {
		if (lifeTime > 0) {
			if (MazeManager.GetTile ((Vector2)transform.position - new Vector2(0, Tile.size)).isWall) {
				Destroy (gameObject);
			}

			Vector3 pos = transform.position;
			pos.x += moveVector.x;
			pos.y += moveVector.y;
			pos.z += moveVector.y;
			transform.position = pos;
			lifeTime -= Time.deltaTime;
		} else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Enemy")) {
			Character comp = collider.GetComponent<Character> ();
			comp.StartCoroutine(comp.Damage (transform.position, damage));
			Destroy (gameObject);
		}
	}

}

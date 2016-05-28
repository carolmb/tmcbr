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
		Vector3 pos;
		pos.x = Player.instance.transform.position.x;
		pos.z = Player.instance.transform.position.y;
		pos.y = pos.z + 8;
		transform.position = pos;

		moveVector = GameManager.AngleToVector (Player.instance.character.lookingAngle) * speed;
		Destroy (gameObject, lifeTime);
	}

	void Update () {
		Tile t = MazeManager.GetTile ((Vector2)transform.position - new Vector2 (0, Tile.size)); 
		if (t == null || t.isWall) {
			if (t != null && t.isWall) {
				SoundManager.RockCollision ();
			}
			Destroy (gameObject);
		} else {
			Vector3 pos = transform.position;
			pos.x += moveVector.x * 60 * Time.deltaTime;
			pos.y += moveVector.y * 60 * Time.deltaTime;
			pos.z += moveVector.y * 60 * Time.deltaTime;
			transform.position = pos;
			lifeTime -= Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Enemy")) {
			Character comp = collider.GetComponent<Character> ();
			comp.Damage ((Vector2) transform.position - moveVector * 10, damage);
			SoundManager.RockCollision ();
			Destroy (gameObject);
		}
	}

}

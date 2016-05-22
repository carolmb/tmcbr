using UnityEngine;
using System.Collections;

public class GolemRock : MonoBehaviour {
	// Dano causado ao inimigo
	public int damage = 2;

	// Velocidade
	public float speed = 4;

	// Direção do movimento
	private Vector2 moveVector;

	// Barulho ao colidir
	public AudioClip collisionSound;

	void Start() {
		Vector3 pos;
		pos.x = Player.instance.transform.position.x;
		pos.z = Player.instance.transform.position.y;
		pos.y = pos.z + 8;
		transform.position = pos;
		//moveVector = GameManager.AngleToVector (c.lookingAngle) * speed;
	}

	void Update () {
		Tile t = MazeManager.GetTile ((Vector2)transform.position - new Vector2 (0, Tile.size)); 
		if (t == null || t.isWall) {
			Destroy (gameObject);
		} else {
			Vector3 pos = transform.position;
			pos.x += moveVector.x * 60 * Time.deltaTime;
			pos.y += moveVector.y * 60 * Time.deltaTime;
			pos.z += moveVector.y * 60 * Time.deltaTime;
			transform.position = pos;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			Character comp = collider.GetComponent<Character> ();
			comp.Damage ((Vector2) transform.position - moveVector * 10, damage);
			//GameCamera.PlayAudioClip (collisionSound);
			Destroy (gameObject);
		}
	}
}
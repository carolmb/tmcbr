using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class PickItem : MonoBehaviour {

	public static int golemCount = 0;

	public GameObject rockq;
	public GameObject golem;

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 5;

	// Tempo máximo até ser destruído
	public float lifeTime = 0.1f;

	// O quanto a espada vai rotacionar
	private Quaternion dest;
	private Quaternion orig;
	private float rotPerc = 0;

	void Start () {
		SoundManager.Pickaxe ();
		BoxCollider2D box = GetComponentInChildren<BoxCollider2D>();

		Vector3 pos = Vector3.zero;
		switch (Player.instance.character.direction) {
		case 0: // Pra baixo
			orig = Quaternion.Euler (0, 0, 180);
			dest = Quaternion.Euler (0, 0, 270);
			box.offset.Set(0,0);
			break;
		case 1: // Pra esquerda
			transform.localScale = new Vector2(-1, 1);
			orig = Quaternion.Euler(0, 0, 0);
			dest = Quaternion.Euler(0, 0, 90);
			box.offset.Set(0,0);
			pos.y = 6;
			pos.x = -6;
			break;
		case 2: // Pra direita
			orig = Quaternion.Euler(0, 0, 0);
			dest = Quaternion.Euler(0, 0, -90);
			box.offset.Set(0,0);
			pos.x = 6;
			pos.y = 6;
			break;
		case 3: // Pra cima
			orig = Quaternion.Euler(0, 0, 90);
			dest = Quaternion.Euler(0, 0, 0);
			box.offset.Set(0,0);
			pos.y = 16;
			pos.z = 1;
			break;
		}
		transform.position = Player.instance.transform.position + pos;
		transform.rotation = orig;
		transform.SetParent (Player.instance.transform);
		Destroy(gameObject, lifeTime);
	}

	void Update () {
		rotPerc += Time.deltaTime / lifeTime;
		transform.rotation = Quaternion.Slerp(orig, dest, rotPerc);
	}

	void OnDestroy () {
		if (Player.instance != null) {
			Player.instance.canMove = true;
			Player.instance.character.Stop ();
		}
	}

	// Verifica se um inimigo foi atingido
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Enemy")) {
			Character comp = collider.GetComponent<Character> ();
			if (!comp.damaging) {
				comp.Damage (transform.position, damage);
			}
			Destroy (gameObject);
		} else if (collider.CompareTag ("Rock")) {
			SoundManager.RockCollision ();
			Vector3 pos = collider.transform.position;
			MazeManager.GetTile (pos - new Vector3 (0, Tile.size / 2, 0)).obstacle = "";
			Destroy (collider.gameObject);
			OnDestroyRock (pos);
			Destroy (gameObject);
		} else if (collider.CompareTag ("Golem")) {

			Character comp = collider.GetComponent<Character> ();
			Golem1 golem1 = collider.GetComponent<Golem1> ();
			if (comp.lifePoints == 1) {
				if (golem1 != null) {
					if (golem1.boss) {
						golemCount++;
						Debug.Log ("killed golem");
						Debug.Log (Time.time);
					}
				}
			}

			Vector2 moveVector = GameManager.AngleToVector (Player.instance.character.lookingAngle) * speed;
			comp.Damage ((Vector2) transform.position - moveVector * 10, damage);
		}
	}

	void OnDestroyRock(Vector3 position) {
		int r = Random.Range (0, 100);
		if (r < 30) {
			Instantiate (rockq, position, Quaternion.identity);
		} else if (r < 60) {
			Instantiate (golem, position, Quaternion.identity);
		}
	}

}

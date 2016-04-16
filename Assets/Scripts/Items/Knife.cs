using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Knife : MonoBehaviour {

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 5;

	// Tempo máximo até ser destruído
	public float lifeTime = 1;

	// Direção do movimento
	private Vector2 moveVector;

	void Start () {
		float posx = 0, posy = 0, posz = 0;
		switch (Player.instance.character.direction) {
			case 0:
				transform.Rotate(0,0,-90);
				break;
			case 1:
				transform.Rotate (0, 0, 180);
				posx = -(Tile.size/2);
				posy = Tile.size/2;
				break;
			case 2:
				transform.Rotate (0, 0, 0);
				posx = Tile.size/2;
				posy = Tile.size/2;
				break;
			case 3:
				transform.Rotate (0, 0, 90);
				posy = Tile.size;
				posz = 10;
				break;
		}
		transform.position = Player.instance.transform.position + new Vector3(posx, posy, posz);
		moveVector = GameManager.AngleToVector(Player.instance.character.lookingAngle) * speed;
		Destroy (gameObject, lifeTime);
	}

	void Update () {
		// Atacar
		Tile t = MazeManager.GetTile ((Vector2)transform.position - new Vector2 (0, Tile.size)); 
		if (t == null || Player.instance.character.damaging) {
			Destroy (gameObject);
		} else {
			lifeTime -= Time.deltaTime;
		}
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

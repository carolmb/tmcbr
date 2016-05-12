using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class KnifeItem : MonoBehaviour {

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 5;

	// Tempo máximo até ser destruído
	public float lifeTime = 0.1f;

	// Som da espada
	public AudioClip sound;

	// O quanto a espada vai rotacionar
	private Quaternion dest;
	private Quaternion orig;
	private float rotPerc = 0;

	void Start () {
		GameCamera.PlayAudioClip (sound, 0.5f);

		Vector3 pos = Vector3.zero;
		switch (Player.instance.character.direction) {
		case 0: // Pra baixo
			orig = Quaternion.Euler(0, 0,-135);
			dest = Quaternion.Euler(0, 0, -45);
			break;
		case 1: // Pra esquerda
			orig = Quaternion.Euler(0, 0, 135);
			dest = Quaternion.Euler(0, 0, 225);
			pos.y = 6;
			pos.x = -6;
			break;
		case 2: // Pra direita
			orig = Quaternion.Euler(0, 0, 45);
			dest = Quaternion.Euler(0, 0, -45);
			pos.x = 6;
			pos.y = 6;
			break;
		case 3: // Pra cima
			orig = Quaternion.Euler(0, 0, 45);
			dest = Quaternion.Euler(0, 0, 135);
			pos.y = 16;
			pos.z = 1;
			break;
		}
		transform.position = Player.instance.transform.position + pos;
		transform.rotation = orig;
		Destroy(gameObject, lifeTime);
	}

	void Update () {
		rotPerc += Time.deltaTime / lifeTime;
		transform.rotation = Quaternion.Slerp(orig, dest, rotPerc);
	}

	void OnDestroy () {
		Player.instance.canMove = true;
		Player.instance.character.Stop ();
	}

	// Verifica se um inimigo foi atingido
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Enemy")) {
			Character comp = collider.GetComponent<Character> ();
			if (!comp.damaging) {
				comp.Damage (transform.position, damage);
			}
			Destroy (gameObject);
		} else if (collider.CompareTag ("Grass")) {
			Destroy (gameObject);
		}
	}

}

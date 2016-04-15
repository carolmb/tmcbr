using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Knife : MonoBehaviour {

	// Dano causado ao inimigo
	public int damage = 1;

	// Velocidade
	public float speed = 10;

	// Tempo máximo até ser destruído
	public float lifeTime = 5;

	void Start () {
		//
	}

	void Update () {
		// Atacar
		Invoke ("", 1);
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

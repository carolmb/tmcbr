using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public int value = 1;

	public Color color1 = Color.blue;
	public Color color5 = Color.red;
	public Color color10 = Color.green;
	public Color color20 = Color.yellow;
	public Color color50 = Color.magenta;
	public Color color100 = Color.cyan;
	public Color color200 = Color.white;

	void Start() {
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();

		Vector3 pos = transform.position;
		pos.z = pos.y;
		transform.position = pos;

		if (value >= 200) {
			sr.color = color200;
		} else if (value >= 100) {
			sr.color = color100;
		} else if (value >= 50) {
			sr.color = color50;
		} else if (value >= 20) {
			sr.color = color20;
		} else if (value >= 10) {
			sr.color = color10;
		} else if (value >= 5) {
			sr.color = color5;
		} else {
			sr.color = color1;
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			Player.instance.IncrementCoins (value);
			Destroy (gameObject);
		}
	}

}

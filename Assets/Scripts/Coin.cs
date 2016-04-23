using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public AudioClip sound;

	private int value;

	private class CoinType {
		public int value;
		public Color color;
		public int chance;
		public CoinType(int value, Color color, int chance) {
			this.value = value;
			this.color = color;
			this.chance = chance;
		}
	}

	private static CoinType[] coinTypes = InitializeCoinTypes();

	private static CoinType[] InitializeCoinTypes() {
		CoinType[] types = new CoinType[7];
		types [0] = new CoinType (1, Color.blue, 25);
		types [1] = new CoinType (5, Color.red, 50);
		types [2] = new CoinType (10, Color.green, 70);
		types [3] = new CoinType (20, Color.yellow, 82);
		types [4] = new CoinType (50, Color.magenta, 92);
		types [5] = new CoinType (100, Color.cyan, 97);
		types [6] = new CoinType (200, Color.white, 100);
		return types;
	}

	public float lifeTime = 5.0f;

	void Start() {
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();

		Vector3 pos = transform.position;
		pos.z = pos.y;
		transform.position = pos;

		int r = Random.Range (0, 100);

		for (int i = 0; i < 7; i++) {
			if (r < coinTypes [i].chance) {
				value = coinTypes [i].value;
				sr.color = coinTypes [i].color;
				break;
			}
		}

		Destroy (gameObject, lifeTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			GameCamera.PlayAudioClip (sound);
			Player.instance.IncrementCoins (value);
			Destroy (gameObject);
		}
	}

}

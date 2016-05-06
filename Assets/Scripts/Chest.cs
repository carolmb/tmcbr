using UnityEngine;
using System.Collections;

public class Chest : CharacterBase {

	// Moedas
	public AudioClip coinSound;

	public bool open = false;

	// Barulho de abrir baú
	public AudioClip openSound;

	public Sprite[] openSprites;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		InitialDirection ();
		if (open) {
			spriteRenderer.sprite = openSprites [direction];
		}
	}

	public void OnInteract() {
		OpenChest ();
	}
	
	// Abre o baú
	public void OpenChest() {
		Tile t = MazeManager.GetTile (transform.position - new Vector3 (0, Tile.size / 2, 0));

		// Não é mais possível interagir
		Destroy (GetComponent<Interactable> ());

		// Muda o tipo do objeto
		t.chest = 2; // Baú aberto

		animator.speed = 1;
		GameCamera.PlayAudioClip (openSound);
		Invoke ("IncrementCoins", openSound.length);
	}

	void IncrementCoins() {
		// Adiciona as moedas
		int coins = Random.Range (0, 5);

		Player.instance.IncrementCoins(coins);
		GameCamera.PlayAudioClip (coinSound);
	}

}

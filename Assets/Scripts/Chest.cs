using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Moedas
	public AudioClip coinSound;

	public bool open = false;

	// Barulho de abrir baú
	public AudioClip openSound;

	public Sprite[] openSprites;

	private CharacterBase character;

	// Use this for initialization
	void Start () {
		character = GetComponent<CharacterBase> ();
		character.InitialDirection ();
		character.Stop ();
		if (open) {
			character.spriteRenderer.sprite = openSprites [character.direction];
		}
	}

	public void OnInteract() {
		OpenChest ();
	}
	
	// Abre o baú
	public void OpenChest() {
		Tile t = character.currentTile;

		// Não é mais possível interagir
		Destroy (GetComponent<Interactable> ());

		// Muda o tipo do objeto
		t.chest = 2; // Baú aberto

		character.animator.speed = 1;
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

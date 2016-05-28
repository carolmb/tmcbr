using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public bool open = false;

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
		t.obstacle = "Open Chest";

		character.animator.speed = 1;
		SoundManager.OpenChest ();
		Invoke ("IncrementCoins", 0.1f);
	}

	void IncrementCoins() {
		// Adiciona as moedas
		int coins = Random.Range (0, 5);

		Player.instance.IncrementCoins(coins);
		SoundManager.Coin ();
	}

}

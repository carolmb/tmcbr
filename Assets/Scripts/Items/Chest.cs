using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	private Animator animator;

	public Sprite[] sprites;

	// Moedas
	private int coins;

	// Barulho abrir baú
	public AudioClip openSound;
	public int direction;

	// Use this for initialization
	void Start () {
		coins = Random.Range (0, 5);
		animator = GetComponent<Animator> ();

		// TODO: mudar direção (direction e sprite)

		GetComponent<SpriteRenderer> ().sprite = sprites [direction];
	}

	public void OnInteract() {
		OpenChest ();
	}
	
	// Abre o baú
	public void OpenChest() {
		Tile t = MazeManager.GetTile (transform.position - new Vector3 (0, Tile.size / 2, 0));

		// Não é mais possível interagir
		Destroy (GetComponent<Interactable> ());

		// Adiciona as moedas
		Player.instance.IncrementCoins(coins);

		// Muda o tipo do objeto
		t.chest = 2; // Baú aberto

		animator.Play ("Open" + direction);
		GameCamera.PlayAudioClip (openSound);
	}

}

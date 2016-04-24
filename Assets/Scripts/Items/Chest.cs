using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Moedas
	private int coins;
	private bool opened;

	// Use this for initialization
	void Start () {
		opened = false;
		coins = Random.Range (0, 5);
	}
	
	// Update is called once per frame
	void Update () {
		//
	}

	// Abre o baú
	void openChest() {
		opened = true;
		Player.instance.bag.coins += coins;
	}
}

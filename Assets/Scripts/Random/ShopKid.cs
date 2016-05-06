using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopKid : MonoBehaviour {

	public int[] itemList;

	public void OnInteract() {
		ShopMenu.itemList = itemList;
		ShopMenu.instance.Open ();
	}

}


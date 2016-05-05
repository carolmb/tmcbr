using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopKid : MonoBehaviour {

	public int[] itemList;

	public void OnInteract() {
		Debug.Log ("open shop");
		ShopMenu.instance.OpenShop (itemList);
	}

}


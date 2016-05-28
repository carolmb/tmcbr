using UnityEngine;
using System.Collections;

public class DeadMaid : MonoBehaviour {

	void OnInteract () {
		if (!Bag.current.HasItem (Item.DB [8])) {
			Bag.current.Add (Item.DB [8]);
			SoundManager.Coin ();
		}
	}

}

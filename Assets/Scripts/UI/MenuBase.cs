﻿using UnityEngine;
using System.Collections;


public class MenuBase : MonoBehaviour {

	public virtual void Open () {
		GameHUD.ClickItemSound ();
		GameHUD.instance.gameObject.SetActive(false);
		Player.instance.Pause ();
		gameObject.SetActive (true);
	}

	public virtual void Close () {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
		Player.instance.Resume ();
		GameHUD.instance.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

}
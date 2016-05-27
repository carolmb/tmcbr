using UnityEngine;
using System.Collections;

public class Rose : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameHUD.instance.mainMenu.UpdateRoses (++Bag.current.roses);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			//GameCamera.PlayAudioClip (sound);
			Destroy (gameObject);
		}
	}

}

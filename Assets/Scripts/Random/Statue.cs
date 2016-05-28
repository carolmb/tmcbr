using UnityEngine;
using System.Collections;

public class Statue : MonoBehaviour {

	public GameObject rose;
	Vector2 iniPos;
	Animator controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<Animator> ();
		iniPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Explosion() {
		controller.SetBool ("isDying", true);
	}

	void Die() {
		Destroy (gameObject);
	}
	void OnDestroy() {
		GameObject obj = Instantiate (rose) as GameObject;
		obj.transform.position = iniPos; 
	}
}

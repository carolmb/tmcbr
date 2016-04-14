using UnityEngine;
using System.Collections;

public class Slingshot : Item {

	private GameObject rock;

	float lastUse = 0;
	float delay = 0.25f;

	public Slingshot(int id) : base(id, "Slingshot", false) {
		rock = Resources.Load<GameObject> ("Prefabs/Rock");
	}

	public override void OnUse () {
		if (Time.time - lastUse >= delay) {
			lastUse = Time.time;
			GameObject.Instantiate (rock);
		}
	}

}

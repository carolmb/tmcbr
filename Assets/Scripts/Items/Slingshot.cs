using UnityEngine;
using System.Collections;

public class Slingshot : Item {

	private GameObject rock;

	float lastUse = 0;
	float delay = 0.25f;

	public Slingshot(int id) : base(id, "Slingshot", 10, 5) {
		rock = Resources.Load<GameObject> ("Prefabs/Rock");
	}

	public override bool CanUse () {
		return Time.time - lastUse >= delay;
	}

	public override void OnUse () {
		AudioClip clip = Resources.Load<AudioClip> ("Sounds/throw");
		GameCamera.PlayAudioClip (clip, 0.5f);
		Player.instance.character.PlayAnimation ("Walking"); // Temporário (TODO: animação "throw")
		lastUse = Time.time;
		GameObject.Instantiate (rock);
	}

}

using UnityEngine;
using System.Collections;

public class Poison : Item {

	public Poison(int id) : base(id, "Poison", 0, 1, false, true) {}

	public override bool CanUse () {
		return true;
	}

	public override void OnUse () {
		//Player.instance.character.PlayAnimation ("Drink", false);
		GameCamera.PlayAudioClip (Resources.Load<AudioClip>("Sounds/Effects/remedio"));
		Player.instance.OnDie ();
	}

}

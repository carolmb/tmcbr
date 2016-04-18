using UnityEngine;
using System.Collections;

public class Repel : Item {

	private GameObject repel;

	public Repel(int id) : base(id, "Repel", false) {
		repel = Resources.Load<GameObject>("Prefabs/Repel");
	}

	public override void OnUse () {
		Player.instance.repelTime = 10;
	}
}

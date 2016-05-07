using UnityEngine;
using System.Collections;

public class KeyItem : Item {
	
	public KeyItem(int id, string name) : base(id, name, 0, 0, false, false) {}
	
	public override bool CanUse () {
		return false;
	}
	
	public override void OnUse () {	}

}

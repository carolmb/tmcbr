using UnityEngine;
using System.Collections;
using UnityEditor;

public class ResetPlayerPrefs {

	[MenuItem("Edit/Reset Playerprefs")] public static void DeletePlayerPrefs() { PlayerPrefs.DeleteAll(); }

}

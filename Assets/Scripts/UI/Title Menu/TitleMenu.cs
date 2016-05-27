using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleMenu : MenuBase {

	public static TitleMenu instance;

	public TitleWindow titleWindow;
	public LoadWindow loadWindow;
	public CreditsWindow creditsWindow;

	public void Awake () {
		instance = this;
		fading = false;
		titleWindow.UpdateLoadButton ();
	}

	public static bool fading = false;

	// ===============================================================================
	// Sons
	// ===============================================================================

	public AudioClip confirmSound;

	public static void ClickItemSound () {
		GameCamera.PlayAudioClip (instance.confirmSound);
	}

}

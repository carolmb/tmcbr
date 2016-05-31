using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleMenu : MenuBase {

	public static TitleMenu instance;

	public TitleWindow titleWindow;
	public LoadWindow loadWindow;
	public CreditsWindow creditsWindow;
	public TutorialWindow tutorialWindow;

	public void Awake () {
		instance = this;
		titleWindow.UpdateLoadButton ();
	}

}

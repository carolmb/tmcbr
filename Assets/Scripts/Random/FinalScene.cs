using UnityEngine;
using System.Collections;

public class FinalScene : MonoBehaviour {

	Character protagonist;
	Character maid;

	public GameObject deadMaid;
	public GameObject surprise;

	public Sprite deadDuke;
	public Sprite duke;

	public bool hasAllRoses {
		get { return Bag.current.roses == 3; }
	}

	void Start () {
		maid = GetComponent<Character> ();
		protagonist = Player.instance.character;

		GameHUD.instance.gameObject.SetActive (false);
		protagonist.direction = 3;
		Player.instance.canMove = false;
		protagonist.Stop ();

		if (hasAllRoses) {
			StartCoroutine (TrueEnding ());
		} else {
			StartCoroutine (BadEnding ());
		}
	}

	IEnumerator BadEnding () {
		float s = protagonist.speed;
		protagonist.speed = 1;
		maid.animator.enabled = false;
		maid.spriteRenderer.sprite = null;
		Instantiate (deadMaid, transform.position, Quaternion.identity);
		yield return new WaitForSeconds (1);
		yield return protagonist.Move (new Vector2 (0, 48));
		protagonist.Stop ();
		Player.instance.paused = true;
		surprise = (GameObject) Instantiate (surprise, protagonist.transform.position + new Vector3 (0, 48, -100), Quaternion.identity);
		SoundManager.Surprise ();
		yield return new WaitForSeconds (0.5f);
		Destroy (surprise);
		//yield return GameHUD.instance.dialog.ShowDialog ("What...?", "Player[sad]");
		GameHUD.instance.gameObject.SetActive (true);
		protagonist.speed = s;
		Player.instance.canMove = true;
		Player.instance.paused = false;
		maid.currentTile.obstacle = "dead maid";
	}

	IEnumerator TrueEnding () {
		protagonist.speed = 1;
		yield return protagonist.Move (new Vector2 (0, 48));
		protagonist.Stop ();
		surprise = (GameObject) Instantiate (surprise, protagonist.transform.position + new Vector3 (0, 48, -100), Quaternion.identity);
		SoundManager.Click ();
		yield return new WaitForSeconds (0.5f);
		Destroy (surprise);
		//yield return GameHUD.instance.dialog.ShowDialog ("I finally found you!", "Player[surprise]");
		maid.speed = protagonist.speed * 2;
		maid.Move (new Vector2 (0, -40));
		yield return protagonist.Move (new Vector2 (0, 20));
		protagonist.Stop ();
		maid.Stop ();

		yield return StartCoroutine (GameCamera.instance.FadeOut (10));
		SoundManager.Stab ();
		yield return new WaitForSeconds (2);
		protagonist.animator.enabled = false;
		protagonist.spriteRenderer.sprite = duke;
		yield return StartCoroutine (GameCamera.instance.FadeIn (0.3f));

		//yield return GameHUD.instance.dialog.ShowDialog ("I could smell this scent of roses from far...", "Maid[smile]");
		maid.speed = protagonist.speed;
		maid.TurnTo (180);
		yield return maid.Move (new Vector2 (-24, 0));
		maid.TurnTo (270);
		yield return maid.Move (new Vector2 (0, -40));
		maid.Stop ();
		yield return new WaitForSeconds (0.5f);
		//yield return GameHUD.instance.dialog.ShowDialog ("I'm sorry.", "Maid[sad]");
		maid.TurnTo (0);
		yield return maid.Move (new Vector2 (24, 0));
		maid.TurnTo (270);
		yield return maid.Move (new Vector2 (0, -64));
		maid.spriteRenderer.enabled = false;
		maid.animator.enabled = false;

		Coroutine c = StartCoroutine (GameCamera.instance.FadeOut (0.5f));
		yield return new WaitForSeconds (1f);
		SoundManager.DieCollision ();
		protagonist.spriteRenderer.sprite = deadDuke;
		yield return c;

		yield return new WaitForSeconds (2);
		MazeManager.GoToMaze(new Tile.Transition(SaveManager.currentSave.mazes.Length - 1, 5, 0, 3));

	}

}

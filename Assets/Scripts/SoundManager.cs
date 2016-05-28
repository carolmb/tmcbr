using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip click;
	public AudioClip surprise;
	public AudioClip[] floorSteps;
	public AudioClip[] grassSteps;
	public AudioClip openChest;
	public AudioClip openDoor;
	public AudioClip coin;
	public AudioClip cloth;
	public AudioClip playerDamage;
	public AudioClip rockCollision;
	public AudioClip explosion;
	public AudioClip knife;
	public AudioClip glassBreak;
	public AudioClip drink;
	public AudioClip spray;
	public AudioClip throwSound;
	public AudioClip fall;
	public AudioClip dieCollision;
	public AudioClip lockDoor;
	public AudioClip stab;

	private static SoundManager instance;

	private void Awake () {
		instance = this;
	}

	public static void Click () {
		PlayAudioClip (instance.click, 1); 
	}

	public static void GrassStep () {
		AudioClip i = instance.grassSteps[Random.Range(0, instance.grassSteps.Length)];
		PlayAudioClip(i, 1);
	}

	public static void FloorStep () {
		AudioClip i = instance.floorSteps[Random.Range(0, instance.floorSteps.Length)];
		PlayAudioClip(i, 1);
	}

	public static void OpenChest () {
		PlayAudioClip (instance.openChest, 1);
	}

	public static void OpenDoor() {
		PlayAudioClip (instance.openDoor, 1);
	}

	public static void Coin () {
		PlayAudioClip (instance.coin, 1);
	}

	public static void Buy () {
		PlayAudioClip (instance.coin, 1);
	}

	public static void Cloth () {
		PlayAudioClip (instance.cloth, 1);
	}

	public static void PlayerDamage () {
		// aquele choro de criança
		PlayAudioClip (instance.playerDamage, 1);
	}

	public static void RockCollision () {
		PlayAudioClip (instance.rockCollision, 1);
	}

	public static void Explosion () {
		PlayAudioClip (instance.explosion, 1);
	}

	public static void Knife () {
		PlayAudioClip (instance.knife, 1);
	}

	public static void Pickaxe () {
		PlayAudioClip (instance.knife, 1);
	}

	public static void GlassBreak () {
		// vidro quebrando
		PlayAudioClip (instance.glassBreak, 1);
	}

	public static void Drink () {
		// remédio e veneno
		PlayAudioClip (instance.drink, 1);
	}

	public static void Spray () {
		// repelente
		PlayAudioClip (instance.spray, 1);
	}

	public static void Throw () {
		// quando joga a pedra
		PlayAudioClip (instance.throwSound, 1);
	}

	public static void Fall () {
		// quando o personagem cai no furaco (barulho de desenho animado mesmo)
		PlayAudioClip (instance.fall, 1);
	}

	public static void DieCollision() {
		// quando o personagem cai no chão, na animação de morte
		PlayAudioClip (instance.dieCollision, 1);
	}

	public static void Lock () {
		// barulho que faz quando o player interage com uma porta trancada
		PlayAudioClip (instance.lockDoor, 1);
	}

	public static void Stab () {
		// quando a criada dá uma facada no duque
		PlayAudioClip (instance.stab, 1);
	}

	public static void Surprise () {
		PlayAudioClip (instance.surprise, 1); 
	}

	// Método que cria o audio source na câmera
	private static void PlayAudioClip (AudioClip clip, float volume = 1) {
		GameObject go = new GameObject ();
		go.transform.SetParent (Camera.main.transform);
		go.transform.localPosition = Vector3.zero;

		AudioSource audio = go.AddComponent<AudioSource> ();
		audio.clip = clip;
		audio.volume = volume;
		audio.Play ();

		Destroy (go, clip.length);
	}

}

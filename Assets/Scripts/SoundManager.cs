using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip click;
	public AudioClip[] floorSteps;
	public AudioClip[] grassSteps;

	private static SoundManager instance;

	private void Awake() {
		instance = this;
	}

	public static void Click() {
		PlayAudioClip (instance.click, 1); 
	}

	public static void GrassStep () {
		AudioClip i = instance.grassSteps[Random.Range(0, instance.grassSteps.Length)];
		PlayAudioClip(i, 1);
	}

	public static void FloorStep () {
		// parecida com o da grama
	}

	public static void OpenChest () {
		// abrindo baú
	}

	public static void OpenDoor() {
		// abrindo portão
	}

	public static void Coin () {
		// quando pega a moeda
	}

	public static void Buy () {
		// quando compra algo (pode ser o msm da moeda)
	}

	public static void Cloth () {
		// quando veste a capa
	}

	public static void PlayerDamage () {
		// aquele choro de criança
	}

	public static void RockCollision () {
		// quando a pedra jogada bate em alguma coisa
	}

	public static void Explosion () {
		// auto-explicativo
	}

	public static void Knife () {
		// barulho de sword slash
	}

	public static void Pickaxe () {
		// picareta (pode ser o mesmo da faca)
	}

	public static void GlassBreak () {
		// vidro quebrando
	}

	public static void Drink () {
		// remédio e veneno
	}

	public static void Spray () {
		// repelente
	}

	public static void Throw () {
		// quando joga a pedra
	}

	public static void Fall () {
		// quando o personagem cai no furaco (barulho de desenho animado mesmo)
	}

	public static void DieCollision() {
		// quando o personagem cai no chão, na animação de morte
	}


	// Método que cria o audio source na câmera
	private static void PlayAudioClip(AudioClip clip, float volume = 1) {
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

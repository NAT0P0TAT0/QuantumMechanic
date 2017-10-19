using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    private AudioSource audio;
	private float Musicvolume = 0.5f;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	void Update() {
		Musicvolume = PlayerPrefs.GetFloat("MUSICvolume", 0.5f);
		audio.volume = Musicvolume;
	}
}

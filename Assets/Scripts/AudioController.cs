using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    private AudioSource source;
    public AudioClip[] sounds;
    private float volume = 0.5f;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        volume = PlayerPrefs.GetFloat("SFXvolume");
	}

    public void PlaySound(int id){
        source.PlayOneShot(sounds[id], volume);
    }
}

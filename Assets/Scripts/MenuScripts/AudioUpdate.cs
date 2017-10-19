using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioUpdate : MonoBehaviour {

	public void UpdateVolume()
    {
        float volume = GameObject.Find("SFXSlider").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("SFXvolume", volume);
        volume = GameObject.Find("MusicSlider").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("MUSICvolume", volume);
    }
}

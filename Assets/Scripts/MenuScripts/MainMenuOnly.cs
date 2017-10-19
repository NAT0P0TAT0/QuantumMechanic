using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuOnly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad < 0.5f){
			Time.timeScale = 1;
			Destroy(GameObject.Find("MusicPlayer2"));
			foreach(GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject))){
				if (obj.name.Contains("MusicSlider")){
					obj.SetActive(true);
					obj.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MUSICvolume");
				}
				if (obj.name.Contains("SFXSlider")){
					obj.SetActive(true);
					obj.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXvolume");
				}
			}
		}
	}
}

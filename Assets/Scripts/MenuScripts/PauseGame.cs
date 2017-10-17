using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {


    //publics
    public GameObject PausePanel;

    //Methods
    //Restart
    public void Resume()
    {
        Time.timeScale = 1;
		PausePanel.SetActive(false);
    }
    //Restart
    public void Restart()
    {
        Time.timeScale = 1;
        GameObject.Find("LevelLoader").GetComponent<levelcheck>().RestartLevel();
        Time.timeScale = 1;
    }

    public void UpdateVolume()
    {
        float volume = GameObject.Find("SFXSlider").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("SFXvolume", volume);
        volume = GameObject.Find("MusicSlider").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("MUSICvolume", volume);
    }

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			if(Time.timeScale != 0){
				Time.timeScale = 0;
				PausePanel.SetActive(true);
			} else {
				Time.timeScale = 1;
                PausePanel.SetActive(false);
                GameObject.Find("OptionsPanel").SetActive(false);
			}
        }
    }
}

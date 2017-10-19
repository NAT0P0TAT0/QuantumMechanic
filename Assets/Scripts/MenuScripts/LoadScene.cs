using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

	public Transform musicprefab;

    public void LoadSceneByIndex(int SceneNumber)
    {
		Transform musicplayer = Instantiate(musicprefab, new Vector3(0, 0, 0), transform.rotation);
		musicplayer.name = "MusicPlayer2";
		DontDestroyOnLoad(musicplayer);
        SceneManager.LoadScene(SceneNumber);
    }
	
    public void LoadLatestLevel()
    {
		Transform musicplayer = Instantiate(musicprefab, new Vector3(0, 0, 0), transform.rotation);
		musicplayer.name = "MusicPlayer2";
		DontDestroyOnLoad(musicplayer);
		int savedChapter = PlayerPrefs.GetInt("PlayersChapter");
		int savedLevel = PlayerPrefs.GetInt("PlayersLevel");
		if(savedChapter == 0 || savedLevel == 0){
			SceneManager.LoadScene(1);
		} else {
		Transform levelloader = GameObject.Find("LevelContinue").transform;
		levelloader.name = "" + savedLevel;
		DontDestroyOnLoad(levelloader);
		SceneManager.LoadScene("Part" + savedChapter);
		}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void LoadSceneByIndex(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }
	
    public void LoadLatestLevel()
    {
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
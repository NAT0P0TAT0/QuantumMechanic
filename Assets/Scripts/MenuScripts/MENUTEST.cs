using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENUTEST : MonoBehaviour {

	public string[] LevelParts;
	public GUIStyle buttonstyle;
	public GUIStyle labelstyle;
	public Transform LevelContinue;
	int savedChapter = 1;
	int savedLevel = 1;
	Vector2 buttonsize = new Vector2(50, 30);
	Vector2 buttonpos = new Vector2(Screen.width-70, 20);

	// Use this for initialization
	void Start () {
		savedChapter = PlayerPrefs.GetInt("PlayersChapter");
		savedLevel = PlayerPrefs.GetInt("PlayersLevel");
	}
	
	void OnGUI() {
		buttonpos.y = 20;
		if (GUI.Button(new Rect(buttonpos.x-(buttonsize.x*0.5f), buttonpos.y, buttonsize.x*2, buttonsize.y), "Reset Save", buttonstyle)){
			PlayerPrefs.SetInt("PlayersChapter", 0);
			PlayerPrefs.SetInt("PlayersLevel", 0);
			savedChapter = 0;
			savedLevel = 0;
		}
		buttonpos.y += buttonsize.y + 20;
		for (int i = 1; i <= LevelParts.Length; i++){
			Object[] textures = Resources.LoadAll("Levels/"+LevelParts[i-1], typeof(Texture2D));
			for (int j = 1; j <= textures.Length; j++){
				if (savedChapter > i || (savedChapter == i && savedLevel >= j)){
					if (GUI.Button(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), i + "-" + j, buttonstyle)){
						Transform levelloader = Instantiate(LevelContinue, new Vector3(-99, -99, 0), transform.rotation);
						levelloader.name = "" + j;
						DontDestroyOnLoad(levelloader);
						SceneManager.LoadScene("Part" + i);
					}
				} else {
					GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), i + "-" + j, labelstyle);
				}
				buttonpos.y += buttonsize.y + 20;
			}
		}
	}
}

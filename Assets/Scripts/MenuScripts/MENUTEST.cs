using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MENUTEST : MonoBehaviour {

	public string[] LevelParts;
	public GUIStyle buttonstyle;
	public GUIStyle labelstyle;
	public Transform LevelContinue;
	//Menu Canvas
	public Canvas MainMenu;
	int savedChapter = 1;
	int savedLevel = 1;
	private float padding = 1;
	Vector2 buttonsize = new Vector2(0, 0);
	Vector2 buttonpos = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		savedChapter = PlayerPrefs.GetInt("PlayersChapter");
		savedLevel = PlayerPrefs.GetInt("PlayersLevel");
	}
	
	void OnGUI() {
		buttonsize = new Vector2(Screen.width*0.044f, Screen.width*0.035f);
		padding = Screen.width*0.015f;
		//Reset save code
		/*buttonpos.y = 100;
		if (GUI.Button(new Rect(buttonpos.x-(buttonsize.x*0.5f), buttonpos.y, buttonsize.x*2, buttonsize.y), "Reset Save", buttonstyle)){
			PlayerPrefs.SetInt("PlayersChapter", 0);
			PlayerPrefs.SetInt("PlayersLevel", 0);
			savedChapter = 0;
			savedLevel = 0;
		}
		buttonpos.y += buttonsize.y + padding;*/
		
		
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
				buttonpos.y += buttonsize.y + padding;
			}
			buttonpos.x += buttonsize.x + padding;
			buttonpos.y = (Screen.height/2) + Screen.height*0.117f;
		}
		float offset = buttonsize.x*2.5f + (padding*2);
		buttonpos.x = (Screen.width/2) - offset;
	}
}

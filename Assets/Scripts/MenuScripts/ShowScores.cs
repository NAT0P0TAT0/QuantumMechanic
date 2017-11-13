using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowScores : MonoBehaviour {

	public string[] LevelParts;
	public GUIStyle labelstyle;
	public GUIStyle labelstyle2;
	//Menu Canvas
	public Canvas MainMenu;
	int savedChapter = 1;
	int savedLevel = 1;
	private float padding = 1;
	Vector2 buttonsize = new Vector2(0, 0);
	Vector2 buttonpos = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI() {
		float screenwidth = Screen.width;
		screenwidth = 720;
		buttonsize = new Vector2(screenwidth*0.05f, screenwidth*0.05f);
		padding = screenwidth*0.075f;
		
		for (int i = 0; i <= LevelParts.Length; i++){
			if(i == 0){
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), " \n Least ability uses \n Fastest clear time ", labelstyle2);
				buttonpos.y += buttonsize.y + padding;
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), " \n Least ability uses \n Fastest clear time ", labelstyle2);
				buttonpos.y += buttonsize.y + padding;
				buttonpos.x += buttonsize.x + padding/2;
			} else {
				Object[] textures = Resources.LoadAll("Levels/"+LevelParts[i-1], typeof(Texture2D));
				for (int j = 1; j <= textures.Length; j++){
					int abilitycount = PlayerPrefs.GetInt(i+"-"+j+"-abilities");
					float levelTime = PlayerPrefs.GetFloat(i+"-"+j+"-time");
					string string1 = abilitycount + "";
					string string2 = levelTime.ToString("0.00") + "s";
					if (levelTime == 0){
						string1 = "n/a";
						string2 = "n/a";
					}
					GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), "Level " + i + "-" + j + "\n" + string1 + "\n" + string2, labelstyle);
					buttonpos.y += buttonsize.y + padding;
				}
			}
			buttonpos.x += buttonsize.x + padding;
			buttonpos.y = (Screen.height/2) - Screen.height*0.04f;
		}
		float offset = buttonsize.x*3f + (padding*3.25f);
		buttonpos.x = (Screen.width/2) - offset;
	}
}

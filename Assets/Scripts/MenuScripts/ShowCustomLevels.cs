using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

public class ShowCustomLevels : MonoBehaviour {

	public GUIStyle buttonstyle;
	public GUIStyle textstyle;
	
	private string[] levelname;
	private int visibleleveloffset = 0;
	//Menu Canvas
	public Canvas MainMenu;
	private float padding = 1;
	Vector2 buttonsize = new Vector2(0, 0);
	Vector2 buttonpos = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		GetLevels();
	}
	
	void GetLevels(){
		visibleleveloffset = 0;
		string path = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer) {
			path += "/../../";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path += "/../";
		}
		int pathindex = path.IndexOf("/Quantum Mechanic_Data");
		if(pathindex  < 0) {
			pathindex = path.IndexOf("/Assets");
		}
		if(pathindex >= 0) {
			path = path.Substring(0, pathindex);
			path += "/CustomLevels";
			DirectoryInfo dir = new DirectoryInfo(@path);
			string[] extensions = new[] { ".png", ".PNG" };
			FileInfo[] info = dir.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
			levelname = new string[info.Length];
			for (int i = 0; i < info.Length; i++){
				string folderpath = path.ToString();
				string filepath = info[i].ToString();
				folderpath = folderpath.Replace("/", "\\");
				filepath = filepath.Replace(folderpath, "");
				filepath = filepath.Replace("\\", "");
				filepath = filepath.Replace(".png", "");
				filepath = filepath.Replace(".PNG", "");
				//add level to list
				levelname[i] = filepath;
			}
			Array.Sort(levelname);
		}
	}
	
	void OnGUI() {
		float screenwidth = Screen.width;
		screenwidth = 720;
		buttonsize = new Vector2(screenwidth*0.05f, screenwidth*0.05f);
		padding = screenwidth*0.075f;
		float offset = buttonsize.x*3f + (padding*2.8f);
		float offset2 = offset - buttonsize.x*3.6f;
		
		int maxvisible = 6;
		if(levelname.Length-visibleleveloffset < maxvisible){maxvisible = levelname.Length-visibleleveloffset;}
		for (int i = -1; i < maxvisible; i++){
			if(i == -1){
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), " \n\n Least ability uses \n Fastest clear time ", textstyle);
				if (GUI.Button(new Rect(buttonpos.x-30, buttonpos.y+8, buttonsize.x+65, buttonsize.y*0.6f), "Scroll up", buttonstyle)){
					if(visibleleveloffset > 2){
						visibleleveloffset -= 3;
						break;
					}
				}
				//create new level
				if (GUI.Button(new Rect(buttonpos.x-40, buttonpos.y+75, buttonsize.x+80, buttonsize.y*0.9f), "CREATE NEW", buttonstyle)){
					SceneManager.LoadScene("LevelEditor");
				}
				//refresh level list
				if (GUI.Button(new Rect(buttonpos.x-30, buttonpos.y+110, buttonsize.x+65, buttonsize.y*0.6f), "Refresh", buttonstyle)){
					GetLevels();
				}
				buttonpos.y += buttonsize.y + padding + 15;
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), " \n\n Least ability uses \n Fastest clear time ", textstyle);
				buttonpos.y += buttonsize.y + 25;
				if (GUI.Button(new Rect(buttonpos.x-30, buttonpos.y+8, buttonsize.x+65, buttonsize.y*0.6f), "Scroll down", buttonstyle)){
					if(visibleleveloffset < levelname.Length-6){
						visibleleveloffset += 3;
						break;
					}
				}
				buttonpos.y = (Screen.height/2) - Screen.height*0.06f;
				buttonpos.x = (Screen.width/2) - offset2;
			} else {
				int abilitycount = PlayerPrefs.GetInt(levelname[i+visibleleveloffset]+"-abilities");
				float levelTime = PlayerPrefs.GetFloat(levelname[i+visibleleveloffset]+"-time");
				string string1 = abilitycount + "";
				string string2 = levelTime.ToString("0.00") + "s";
				if (levelTime == 0){
					string1 = "n/a";
					string2 = "n/a";
				}
				//show level name and score
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), levelname[i+visibleleveloffset], textstyle);
				GUI.Label(new Rect(buttonpos.x, buttonpos.y, buttonsize.x, buttonsize.y), "\n\n" + string1 + "\n" + string2, textstyle);
				//play the level
				if (GUI.Button(new Rect(buttonpos.x-30, buttonpos.y+15, buttonsize.x+65, buttonsize.y*0.6f), "Play level", buttonstyle)){
					Transform levelloader = GameObject.Find("LevelContinue").transform;
					levelloader.name = levelname[i+visibleleveloffset];
					DontDestroyOnLoad(levelloader);
					SceneManager.LoadScene("CustomLevels");
				}
				//edit the level
				if (GUI.Button(new Rect(buttonpos.x-30, buttonpos.y+65, buttonsize.x+65, buttonsize.y*0.6f), "Edit level", buttonstyle)){
					Transform levelloader = GameObject.Find("LevelContinue").transform;
					levelloader.name = levelname[i+visibleleveloffset];
					DontDestroyOnLoad(levelloader);
					SceneManager.LoadScene("LevelEditor");
				}
				if(i % 3 == 2){
					buttonpos.y += buttonsize.y + padding + 15;
					buttonpos.x = (Screen.width/2) - offset2;
				} else {
					buttonpos.x += buttonsize.x*3.25f + padding;
				}
				
			}
		}
		buttonpos.y = (Screen.height/2) - Screen.height*0.06f;
		buttonpos.x = (Screen.width/2) - offset;
	}
}

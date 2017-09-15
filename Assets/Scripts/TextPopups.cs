﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopups : MonoBehaviour {
	public int[] level;
	public Texture2D[] picture;
	public string[] name;
	public string[] text;
	
	public GUIStyle bgstyle;
	public GUIStyle labelstyle;
	
	private bool popupOpen = false;
	private int openID = -1;
	
	public void OpenPopup(int triggerID){
		int levelnum = GameObject.Find("LevelLoader").GetComponent<levelcheck>().GetLevelNum();
		int j = 0;
		for(int i = 0; i < level.Length; i++){
			if(level[i] == levelnum){
				if(triggerID == j){
					popupOpen = true;
					openID = i;
					break;
				}
				j++;
			}
		}
	}
	
	//display popup
	void OnGUI() {
		if(popupOpen){
			//show popup bg
			Vector2 BGsize = new Vector2(Screen.width*0.6f, Screen.width*0.6f*0.335f);
			Vector2 BGpos = new Vector2(Screen.width*0.2f, 15);
			GUI.Label(new Rect(BGpos.x, BGpos.y, BGsize.x, BGsize.y), "", bgstyle);
			//show text
			Vector2 Contentsize = new Vector2(BGsize.x*0.7f, BGsize.y*0.86f);
			Vector2 Contentpos = new Vector2(BGpos.x+(BGsize.x*0.25f), BGpos.y+(BGsize.y*0.08f));
			GUI.Label(new Rect(Contentpos.x, Contentpos.y, Contentsize.x, Contentsize.y), text[openID], labelstyle);
			//show name
			Vector2 Namesize = new Vector2(BGsize.x*0.2f, BGsize.y*0.2f);
			Vector2 Namepos = new Vector2(BGpos.x+(BGsize.x*0.01f), BGpos.y+(BGsize.y*0.75f));
			GUI.Label(new Rect(Namepos.x, Namepos.y, Namesize.x, Namesize.y), name[openID], labelstyle);
			//show pic
			Vector2 Picsize = new Vector2(BGsize.x*0.21f+10, BGsize.y*0.63f+10);
			Vector2 Picpos = new Vector2(BGpos.x+(BGsize.x*0.01f)-5, BGpos.y+(BGsize.y*0.04f)-5);
			GUI.Label(new Rect(Picpos.x, Picpos.y, Picsize.x, Picsize.y), picture[openID], labelstyle);
			
			//hide popup
			if (Input.GetKeyDown(KeyCode.E)){
				popupOpen = false;
			}
		}
	}
}

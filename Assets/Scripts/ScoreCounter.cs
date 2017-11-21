using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

	private float StartTime = 0f;
	private float Currtime = 0f;
	private int AbilityCount = 0;
	public bool customlevel = false;
	public string levelname = "";

	// Use this for initialization
	void Start () {
		ResetTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)
			|| Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)
			|| Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)){
			AbilityCount++;
		}
		Currtime = Time.timeSinceLevelLoad-StartTime;
		//Debug.Log(AbilityCount + " : " + Currtime);
	}
	
	public void SaveScore(int chapter, int level){
		//check and set level time and ability use
		Debug.Log("checking time");
		float checkTime = PlayerPrefs.GetFloat(chapter+"-"+level+"-time");
		if(customlevel){checkTime = PlayerPrefs.GetFloat(levelname+"-time");}
		if (Currtime < checkTime || checkTime == 0){
			if(customlevel){
				PlayerPrefs.SetFloat(levelname+"-time", Currtime);
			} else {
				PlayerPrefs.SetFloat(chapter+"-"+level+"-time", Currtime);
			}
		}
		Debug.Log("checking uses");
		int checkUses = PlayerPrefs.GetInt(chapter+"-"+level+"-abilities");
		if(customlevel){checkUses = PlayerPrefs.GetInt(levelname+"-abilities");}
		if (AbilityCount < checkUses || checkUses == 0){
			if(customlevel){
				PlayerPrefs.SetInt(levelname+"-abilities", AbilityCount);
			} else {
				PlayerPrefs.SetInt(chapter+"-"+level+"-abilities", AbilityCount);
			}
		}
	}
	
	public void ResetTimer(){
		StartTime = Time.timeSinceLevelLoad;
		AbilityCount = 0;
	}
}

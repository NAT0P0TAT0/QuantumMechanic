using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUpdate : MonoBehaviour {


    //
    public Toggle Toggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
        Screen.fullScreen = Toggle.isOn;
    }
}

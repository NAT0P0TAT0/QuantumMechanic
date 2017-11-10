using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour {
	
	public int PopupID = 0;
	private bool opened = false;

	//detecting if player hit trigger
	void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !opened) {
			GameObject.Find("TextPopups").GetComponent<TextPopups>().OpenPopup(PopupID);
			opened = true;
		}
        if (other.name == "Player-Light-Form" && !opened) {
			GameObject.Find("TextPopups").GetComponent<TextPopups>().OpenPopup(PopupID);
			opened = true;
		}
    }
}

using UnityEngine;
using System.Collections;

public class exit : MonoBehaviour {
	private GameObject go;
	
	void Start(){
		go = GameObject.Find("Map");
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Player-char"){
			go.GetComponent<levelcheck>().finished = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.gameObject.name == "Player-char"){
			go.GetComponent<levelcheck>().finished = false;
		}
	}
}

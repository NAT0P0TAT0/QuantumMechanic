using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorbelt : MonoBehaviour {

	public bool left = false;
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Entangled")){//move player
			GameObject.Find("Player-char").GetComponent<playercontroller>().onbelt = true;
			GameObject.Find("Player-char").GetComponent<playercontroller>().beltleft = left;
		} else if(other.gameObject.name.Contains("Copy") && Time.timeScale != 0){//move copies
			Vector3 speed = other.gameObject.GetComponent<Rigidbody>().velocity;
			if(left && speed.x > -3){
				speed.x -= 0.5f;
			} else if(speed.x < 3) {
				speed.x += 0.5f;
			}
			other.gameObject.GetComponent<Rigidbody>().velocity = speed;
		}
	}
}

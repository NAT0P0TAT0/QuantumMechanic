using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCancel : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-Light-Form"){
			GameObject.Find("Player-char").GetComponent<QuantumAbilities>().CancelLightMode(true);
		}
	}
}

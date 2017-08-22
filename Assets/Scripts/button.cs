using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool activated = false;
	private bool pressed = false;
	public bool Toggle = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 scale = this.gameObject.transform.GetChild(0).transform.localScale;
		if(activated){
			scale.x = 0.05f;
			this.gameObject.transform.GetChild(0).transform.localScale = scale;
		} else {
			scale.x = 0.15f;
			this.gameObject.transform.GetChild(0).transform.localScale = scale;
		}
		if(activated != pressed){
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("ground")){
				if (fooObj.name.Contains("Toggle")){
					fooObj.transform.position = new Vector3(fooObj.transform.position.x,fooObj.transform.position.y,1+fooObj.transform.position.z);
					GameObject fooChild = fooObj.transform.Find("Block-Model").gameObject;
					if(fooObj.transform.position.z == 2){
						fooObj.transform.position = new Vector3(fooObj.transform.position.x,fooObj.transform.position.y,0);
						fooChild.GetComponent<Renderer>().enabled = true;
					} else {
						fooChild.GetComponent<Renderer>().enabled = false;
					}
				}
			}
		}
		activated = pressed;
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			pressed = true;
		}
	}
	void OnTriggerExit(Collider other) {
		if(!Toggle){
			if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
				pressed = false;
			}
		}
	}
}

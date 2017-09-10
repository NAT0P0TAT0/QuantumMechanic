using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool activated = false;
	private bool pressed = false;
	public bool Toggle = false;
	public Material usedButton;
    private Collider objectTouching;

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
			//send (or end) power through connected wires
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("object")){
				if (fooObj.name.Contains("Wire")){
					if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
				}
			}
			
			if(Toggle){
				this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = usedButton;
			}
		}
		activated = pressed;
        if (!Toggle && pressed && !objectTouching){
            pressed = false;
        }
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			pressed = true;
            objectTouching = other;
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

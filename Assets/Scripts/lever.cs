using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour {
	private bool tiltedright;
	private bool playerleft = false;
	private bool playerright = false;
	private bool flip = false;
	void Start(){
		if(this.transform.rotation.y != 0){
			tiltedright = true;
		} else {
			tiltedright = false;
		}
	}
	
	void Update(){
		if(flip){
			tiltedright = !tiltedright;
			playerleft = false;
			playerright = false;
			flip = false;
            GameObject.Find("AudioController").GetComponent<AudioController>().PlaySound(1);
			//flip lever render
			if(tiltedright){
				this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -180, this.transform.eulerAngles.z);
			} else {
				this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 0, this.transform.eulerAngles.z);
			}
			//send (or end) power through connected wires
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("object")){
				if (fooObj.name.Contains("Wire")){
					if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
					if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = !fooObj.GetComponent<Wire>().powered;}
				}
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			if(other.gameObject.transform.position.x < this.transform.position.x){
				playerleft = true;
			} else {
				playerright = true;
			}
			if(playerleft && playerright){
				if(other.gameObject.transform.position.x < this.transform.position.x){
					if(tiltedright){
						flip = true;
					}
				} else {
					if(!tiltedright){
						flip = true;
					}
				}
			}
		}
	}
	void OnTriggerExit(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			playerleft = false;
			playerright = false;
		}
	}
}

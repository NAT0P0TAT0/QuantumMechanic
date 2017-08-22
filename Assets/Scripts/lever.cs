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
			if(tiltedright){
				this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -180, this.transform.eulerAngles.z);
			} else {
				this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 0, this.transform.eulerAngles.z);
			}
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

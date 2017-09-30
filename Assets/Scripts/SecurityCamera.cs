using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour {

	public bool right = false;
	private bool inAngle = false;
	private bool activated = false;
	private float radius = 5.5f;
	private float accuracy = 0.75f; // need this to account for player size and speed
	private List<GameObject> objects = new List<GameObject>();
	private List<Vector3> objectPos = new List<Vector3>();
	
	void Start(){
		if(right){
			Vector3 newscale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			this.transform.localScale = newscale;
		}
	}

	// Update is called once per frame
	void Update () {
		if(activated != inAngle){
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
		activated = inAngle;
		
		//check if theres nothing in radius (despawning clones dont trigger OnTriggerExit)
		if(inAngle){
			if(objects.Count > 0){
				bool nothingInside = false;
				for (int i = 0; i < objects.Count; i++){
					if (objects[i] != null){
						objectPos[i] = objects[i].transform.position;
					} else {//if a tracked object does despawn check that it despawned within visible area
						float distance = Vector3.Distance(objectPos[i], this.transform.position);
						if(distance <= radius){
							if(objectPos[i].y < this.transform.position.y){
								if(right && objectPos[i].x > this.transform.position.x){nothingInside = true;}
								if(!right && objectPos[i].x < this.transform.position.x){nothingInside = true;}
							}
						}
					}
				}
				if (nothingInside){inAngle = false;objects.Clear();objectPos.Clear();}
			}
		}
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			if(other.gameObject.transform.position.y < this.transform.position.y){
				float distance = Vector3.Distance(other.gameObject.transform.position, this.transform.position);
				if(distance <= radius){
					if(right && other.gameObject.transform.position.x > this.transform.position.x){
						inAngle = true;
						if(!objects.Contains(other.gameObject)){
							objects.Add(other.gameObject);
							objectPos.Add(other.gameObject.transform.position);
						}
					} else if(!right && other.gameObject.transform.position.x < this.transform.position.x){
						inAngle = true;
						if(!objects.Contains(other.gameObject)){
							objects.Add(other.gameObject);
							objectPos.Add(other.gameObject.transform.position);
						}
					}
				}
			}
		}
	}
	void OnTriggerExit(Collider other) {
		if(other.gameObject.name == "Player-char" || other.gameObject.name.Contains("Copy")){
			if(other.gameObject.transform.position.y < this.transform.position.y){
				float distance = Vector3.Distance(other.gameObject.transform.position, this.transform.position);
				if(distance > radius-accuracy && distance < radius+accuracy){
					if(right && other.gameObject.transform.position.x > this.transform.position.x){
						inAngle = false;
					} else if(!right && other.gameObject.transform.position.x < this.transform.position.x){
						inAngle = false;
					}
				}
			}
		}
	}
}

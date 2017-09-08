using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

	public bool powered = false;
	private bool up = false;
	private bool right = false;
	private bool down = false;
	private bool left = false;
	public Material unpoweredMat;
	public Material poweredMat;
	private bool previousState = false;
	private Renderer upRender;
	private Renderer rightRender;
	private Renderer downRender;
	private Renderer leftRender;

	// Use this for initialization
	void Start () {
		//figure out what parts are connected
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("object")){
			if (fooObj.name.Contains("Wire") || fooObj.name.Contains("Button") || fooObj.name.Contains("Lever")){
				if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){up = true;}
				if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){right = true;}
				if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){down = true;}
				if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){left = true;}
			}
		}
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("ground")){
			if (fooObj.name.Contains("Toggle")){
				if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){up = true;}
				if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){right = true;}
				if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){down = true;}
				if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){left = true;}
			}
		}
		//remember which directions have something linked
		if(!up){this.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
		} else {upRender = this.gameObject.transform.GetChild(1).GetComponent<Renderer>();}
		if(!right){this.gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
		} else {rightRender = this.gameObject.transform.GetChild(2).GetComponent<Renderer>();}
		if(!down){this.gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;
		} else {downRender = this.gameObject.transform.GetChild(3).GetComponent<Renderer>();}
		if(!left){this.gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;
		} else {leftRender = this.gameObject.transform.GetChild(4).GetComponent<Renderer>();}
	}
	
	// Update is called once per frame
	void Update () {
		//update attached objects
		if (powered != previousState){
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("object")){
				if (fooObj.name.Contains("Wire")){
					if(up && fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = powered;}
					if(right && fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = powered;}
					if(down && fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = powered;}
					if(left && fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = powered;}
				}
			}
		}
		//update render
		if (powered){
			this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = poweredMat;
			if(up){upRender.material = poweredMat;}
			if(right){rightRender.material = poweredMat;}
			if(down){downRender.material = poweredMat;}
			if(left){leftRender.material = poweredMat;}
		} else {
			this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = unpoweredMat;
			if(up){upRender.material = unpoweredMat;}
			if(right){rightRender.material = unpoweredMat;}
			if(down){downRender.material = unpoweredMat;}
			if(left){leftRender.material = unpoweredMat;}
		}
		previousState = powered;
	}
}

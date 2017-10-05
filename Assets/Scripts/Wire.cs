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
			if (fooObj.name.Contains("Wire") || fooObj.name.Contains("Lever") || fooObj.name.Contains("Camera")){
				if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){up = true;}
				if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){right = true;}
				if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){down = true;}
				if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){left = true;}
			}
			if (fooObj.name.Contains("Button")){
				if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){up = true;}
				if(fooObj.GetComponent<button>().Toggle){
					if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){right = true;}
					if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){left = true;}
					if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){down = true;}
				} else if(fooObj.transform.position.x == this.transform.position.x && fooObj.transform.position.y == this.transform.position.y){
					if(down && !up && !right && !left){
						Destroy(this.gameObject);
					} else {
						down = true;
					}
				}
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
		//get wire directions
		upRender = this.gameObject.transform.GetChild(1).GetComponent<Renderer>();
		rightRender = this.gameObject.transform.GetChild(2).GetComponent<Renderer>();
		downRender = this.gameObject.transform.GetChild(3).GetComponent<Renderer>();
		leftRender = this.gameObject.transform.GetChild(4).GetComponent<Renderer>();
		//hide directions that have nothing linked
		if(!up){this.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;}
		if(!right){this.gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;}
		if(!down){this.gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;}
		if(!left){this.gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;}
		
		updateRender();
	}
	
	// Update is called once per frame
	void Update () {
		//update attached objects
		if (powered != previousState){
			updateAttached();
			updateRender();
		}
		previousState = powered;
	}
	
	void updateAttached(){
		//update attached wires
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("object")){
			if (fooObj.name.Contains("Wire")){
				if(up && fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = powered;}
				if(right && fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = powered;}
				if(down && fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<Wire>().powered = powered;}
				if(left && fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<Wire>().powered = powered;}
			}
		}
		//update attached toggleable blocks
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("ground")){
			if (fooObj.name.Contains("Toggle")){
				if(up && fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<ToggleBlock>().swapState();}
				if(right && fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<ToggleBlock>().swapState();}
				if(down && fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){fooObj.GetComponent<ToggleBlock>().swapState();}
				if(left && fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){fooObj.GetComponent<ToggleBlock>().swapState();}
			}
		}
	}
	
	void updateRender(){
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
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBlock : MonoBehaviour {

	private List<GameObject> otherBlocks = new List<GameObject>();
	public bool active = false;

	// Use this for initialization
	void Start () {
		if (this.transform.position.z == 0){
			active = true;
		}
		//figure out what parts are connected
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("ground")){
			if (fooObj.name.Contains("Toggle")){
				if(fooObj.transform.position.y == this.transform.position.y + 1 && fooObj.transform.position.x == this.transform.position.x){otherBlocks.Add(fooObj);}
				if(fooObj.transform.position.x == this.transform.position.x + 1 && fooObj.transform.position.y == this.transform.position.y){otherBlocks.Add(fooObj);}
				if(fooObj.transform.position.y == this.transform.position.y - 1 && fooObj.transform.position.x == this.transform.position.x){otherBlocks.Add(fooObj);}
				if(fooObj.transform.position.x == this.transform.position.x - 1 && fooObj.transform.position.y == this.transform.position.y){otherBlocks.Add(fooObj);}
			}
		}
	}
	
	public void swapState(){
		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,1+this.transform.position.z);
		Renderer render = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
		if(this.transform.position.z == 2){
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,0);
			render.enabled = true;
		} else {
			render.enabled = false;
		}
		active = !active;
		//update connected parts
		foreach(GameObject fooObj in otherBlocks){
			if(fooObj){
				if (fooObj.GetComponent<ToggleBlock>().active != this.active){
					fooObj.GetComponent<ToggleBlock>().swapState();
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorbelt : MonoBehaviour {

	public bool left = false;
    public Texture2D[] sprites;
	
    private int frameRate = 45;
    private float currFrame = 0;
    private int spriteID = 0;
    private Renderer render;
	
	void Start(){
		if(left){
			Vector3 newscale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			this.transform.localScale = newscale;
		}
		render = this.GetComponent<Renderer>();
	}
	
	void Update(){
		if (sprites.Length > 1){
			float FPS = (float)1 / (float)frameRate;
			if (currFrame >= FPS){
				spriteID++;
				currFrame = 0;
			}
			if (spriteID >= sprites.Length){
				spriteID = 0;
			}
			render.material.mainTexture = sprites[spriteID];
		}
		currFrame += Time.deltaTime;
	}
	
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

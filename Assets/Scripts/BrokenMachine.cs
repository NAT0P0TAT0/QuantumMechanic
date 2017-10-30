using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMachine : MonoBehaviour {
	public Transform pointerPrefab;
	private Transform pointer;
	private Vector3 playerPos;
	private float repairTime = 10;
	private Vector3 pointerPos;
	private bool repaired = false;
	private bool showrepairing = false;
	private float rotation = 0;
	private bool clockwise = true;
	
    public Texture2D[] brokenSprites;
    public Texture2D[] fixedSprites;
	private Texture2D[] sprites;
	private int frameRate = 2;
    private float currFrame = 0;
    private int spriteID = 0;
    private Renderer render;

	// Use this for initialization
	void Start () {
		GameObject.Find("Exit").GetComponent<exit>().machineCount++;
		pointer = Instantiate(pointerPrefab, new Vector3(0, 0, 0), transform.rotation);
		render = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(repairTime > 0){
			sprites = brokenSprites;
			//show pointer
			playerPos = GameObject.Find("Player-char").transform.position;
			float dist = Vector3.Distance(playerPos, this.transform.position);
			if(dist > 2){
				pointerPos = playerPos - ((playerPos - this.transform.position).normalized);
				pointerPos.z = -1;
				pointer.position = pointerPos;
				pointer.LookAt(this.transform);
			} else {
				pointer.position = new Vector3(-999, -999, 0);
			}
			//show repairing indicator
			if(showrepairing){
				this.gameObject.transform.GetChild(1).transform.localPosition = new Vector3(0, 1.25f, -0.25f);
				if(clockwise){
					rotation += 150*Time.deltaTime;
				} else {
					rotation -= 400*Time.deltaTime;
				}
				if(clockwise && rotation >= 55){
					clockwise = false;
				}
				if(!clockwise && rotation <= -55){
					clockwise = true;
				}
				this.gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0,0,rotation);
			} else {
				this.gameObject.transform.GetChild(1).transform.position = new Vector3(-999, -999, 0);
			}
		} else if(!repaired){
			sprites = fixedSprites;
			//machine has been repaired
			pointer.position = new Vector3(-999, -999, 0);
			showrepairing = false;
			this.gameObject.transform.GetChild(1).transform.position = new Vector3(-999, -999, 0);
			Destroy(pointer.gameObject);
			GameObject.Find("Exit").GetComponent<exit>().machineCount--;
			repaired = true;
		}
		//show animation
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
		if(other.gameObject.name == "Player-char"){
			repairTime -= Time.deltaTime;
			showrepairing = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.gameObject.name == "Player-char"){
			showrepairing = false;
		}
	}
}

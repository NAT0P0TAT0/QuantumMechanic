using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public int frameRate = 5;
    public Texture2D[] walkSprites;
    public Texture2D[] jumpSprites;
    public Texture2D[] fallSprites;
    public Texture2D[] idleSprites;
    private Renderer playerRender;
    private int spriteID = 0;
    private float currFrame = 0;
	private int state = 0;

	// Use this for initialization
	void Start () {
        playerRender = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
	}
	
	// Update is called once per frame
    void Update(){
		//flip renderer when player facing left
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
			this.gameObject.transform.GetChild(0).transform.localScale = new Vector3(-1, 2.2f, 1);
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
			this.gameObject.transform.GetChild(0).transform.localScale = new Vector3(1, 2.2f, 1);
        }
		//check if player is jumping or falling
		if (!this.GetComponent<playercontroller>().onground){
			float vertSpeed = this.GetComponent<Rigidbody>().velocity.y;
			if (vertSpeed > 0.1f){
				PlayAnimation(jumpSprites);
				if(state != 3){
					currFrame = 0;
					spriteID = 0;
				}
				state = 3;
			} else if (vertSpeed < -0.1f){
				PlayAnimation(fallSprites);
				if(state != 2){
					currFrame = 0;
					spriteID = 0;
				}
				state = 2;
			}
		//check if player is running
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            PlayAnimation(walkSprites);
			if(state != 1){
				currFrame = 0;
				spriteID = 0;
			}
			state = 1;
        } else {
		//check if player is standing still
            PlayAnimation(idleSprites);
			if(state != 0){
				currFrame = 0;
				spriteID = 0;
			}
			state = 0;
        }
        currFrame += Time.deltaTime;
	}

	//play sprite loop
    public void PlayAnimation(Texture2D[] sprites){
		if (sprites.Length > 1){
			float FPS = (float)1 / (float)frameRate;
			if (currFrame >= FPS){
				spriteID++;
				currFrame = 0;
			}
			if (spriteID >= sprites.Length){
				spriteID = 0;
			}
		} else {
			currFrame = 0;
			spriteID = 0;
		}
        playerRender.material.mainTexture = sprites[spriteID];
    }
}

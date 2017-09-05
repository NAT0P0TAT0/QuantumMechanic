using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Texture2D[] walkSprites;
    public Texture2D[] jumpSprites;
    public Texture2D[] idleSprites;
    private Renderer playerRender;
    private int spriteID = 0;
    public int frameRate = 5;
    private float currFrame = 0;

	// Use this for initialization
	void Start () {
        playerRender = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
	}
	
	// Update is called once per frame
    void Update(){
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            PlayAnimation(walkSprites);
        } else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            PlayAnimation(jumpSprites);
        } else {
            PlayAnimation(idleSprites);
        }
        currFrame += Time.deltaTime;
	}

    public void PlayAnimation(Texture2D[] sprites){
        float FPS = (float)1 / (float)frameRate;
        if (currFrame >= FPS){
            spriteID++;
            currFrame = 0;
        }
        if (spriteID == sprites.Length){
            spriteID = 0;
        }
        playerRender.material.mainTexture = sprites[spriteID];
    }
}

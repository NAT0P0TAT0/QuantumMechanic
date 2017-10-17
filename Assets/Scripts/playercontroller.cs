using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playercontroller : MonoBehaviour {

	private Rigidbody rb;
	public bool onground = false;
	public float acceleration = 0.5f;
	public float deceleration = 0.25f;
	public float maxspeed = 4;
	public float jumpheight = 7.5f;
	public bool onbelt = false;
	public bool beltleft = false;
	private bool pressingLeft = false;
	private bool pressingRight = false;
	private bool pressingJump = false;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		//restart level if player somehow falls out of level
		if (this.transform.position.y < -2) {
			GameObject.Find("LevelLoader").GetComponent<levelcheck>().RestartLevel();
		}
		
		//Detect keypresses
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
		    pressingLeft = true;} else {pressingLeft = false;}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
		    pressingRight = true;} else {pressingRight = false;}
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
		    pressingJump = true;} else {pressingJump = false;}
    }

    void FixedUpdate() {
		//manage movement
		if(Time.timeScale != 0){
            float tempMax = maxspeed;
            if (pressingLeft){
				if(onbelt && beltleft){tempMax = maxspeed*2.5f;}
				if (rb.velocity.x > -tempMax){
                    if(onbelt && !beltleft){//stop player from running across conveyor belts the wrong way
                        if (rb.velocity.x < 0.25f){
                            rb.velocity = new Vector3(rb.velocity.x+acceleration, rb.velocity.y, 0);
                        }
                    } else {
					    rb.velocity = new Vector3(rb.velocity.x-acceleration, rb.velocity.y, 0);
                    }
                }
            } else if (pressingRight){
				if(onbelt && !beltleft){tempMax = maxspeed*2.5f;}
				if (rb.velocity.x < tempMax){
                    if(onbelt && beltleft){//stop player from running across conveyor belts the wrong way
                        if (rb.velocity.x > -0.25f){
                            rb.velocity = new Vector3(rb.velocity.x-acceleration, rb.velocity.y, 0);
                        }
                    } else {
					    rb.velocity = new Vector3(rb.velocity.x+acceleration, rb.velocity.y, 0);
                    }
				}
			} else {
				if (!onground){
					if (rb.velocity.x > -deceleration && rb.velocity.x < deceleration){
						rb.velocity = new Vector3(0, rb.velocity.y, 0);
					}
					if (rb.velocity.x > 0){
						rb.velocity = new Vector3(rb.velocity.x-deceleration, rb.velocity.y, 0);
					}
					if (rb.velocity.x < 0){
						rb.velocity = new Vector3(rb.velocity.x+deceleration, rb.velocity.y, 0);
					}
				}
			}
            if (pressingJump){
				if(onground){
					rb.velocity = new Vector3(rb.velocity.x, jumpheight, 0);
					onground = false;
                    GameObject.Find("AudioController").GetComponent<AudioController>().PlaySound(0);
				}
			}
		}
		
		//adjust speed if on conveyor belt
		if(Time.timeScale != 0 && onbelt){
			Vector3 speed = rb.velocity;
            float beltAccell = 0.5f;
			if(beltleft){
				if(speed.x > -3){
					if(pressingRight){
                        if(speed.x > 0f) {
                            speed.x = 0;
                        }
					} else {
                        speed.x -= beltAccell;
					}
				}
			} else {
				if(speed.x < 3) {
					if(pressingLeft){
                        if(speed.x < 0f) {
                            speed.x = 0;
                        }
					} else {
                        speed.x += beltAccell;
					}
				}
			}
			rb.velocity = speed;
		}
		onbelt = false;
		
		//disable friction when in air (prevents multiple bugs)
		if (onground){
			this.transform.GetComponent<Collider>().material.dynamicFriction = 0.75f;
			this.transform.GetComponent<Collider>().material.staticFriction = 0.75f;
			this.transform.GetComponent<SphereCollider>().material.dynamicFriction = 0.99f;
			this.transform.GetComponent<SphereCollider>().material.staticFriction = 0.99f;
		} else {
			this.transform.GetComponent<Collider>().material.dynamicFriction = 0;
			this.transform.GetComponent<Collider>().material.staticFriction = 0;
			this.transform.GetComponent<SphereCollider>().material.dynamicFriction = 0.33f;
			this.transform.GetComponent<SphereCollider>().material.staticFriction = 0.33f;
		}
	}
	
	
	//detecting if player can jump
	void OnTriggerEnter(Collider other) {
        if (other.tag == "ground" || other.tag == "clone") {
			if (!onground){
				//prevent friction stopping player moving sideways when they land
				if (pressingLeft){
					rb.velocity = new Vector3(-acceleration*9, rb.velocity.y, 0);
				}
				if (pressingRight){
					rb.velocity = new Vector3(acceleration*9, rb.velocity.y, 0);
				}
			}
            onground = true;
		}
    }
	void OnTriggerStay(Collider other) {
        if (other.tag == "ground" || other.tag == "clone") {
            onground = true;
		}
    }
	void OnTriggerExit(Collider other){
        if (other.tag == "ground" || other.tag == "clone") {
            onground = false;
		}
    }
}
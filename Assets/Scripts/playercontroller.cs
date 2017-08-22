using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playercontroller : MonoBehaviour {

	private Rigidbody rb;
	private bool onground = false;
	public float acceleration = 0.5f;
	public float deceleration = 0.25f;
	public int maxspeed = 4;
	public float jumpheight = 7.5f;
	private float invincibleTimeout = 0;
	private bool invincible = false;
	public int MaxHealth = 5;
	private int currHealth;
	private Renderer bodyrender;
    public bool clone = false;
	public int CloneID = 0;
    public float despawnTime = 9999999999999999999;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
		currHealth = MaxHealth;
		bodyrender = this.transform.GetChild(0).GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!clone) {
            //health and damage frames
            if (Time.time > invincibleTimeout) {
                invincible = false;
            }
            if (this.transform.position.y < -2) {
                currHealth = 0;
            }
            if (currHealth > MaxHealth) {
                currHealth = MaxHealth;
            }
            if (beingdamaged) {
                TakeDamage();
            }
            if (currHealth < 1) {
                //die and restart level
                //GameObject.Find("Map").GetComponent<levelcheck>().RestartLevel();
                currHealth = MaxHealth;
                beingdamaged = false;
                invincibleTimeout = 0;
            }
            if (invincible) {
                if (Mathf.Floor(Time.time * 10) % 2 == 0) {
                    bodyrender.enabled = true;
                } else {
                    bodyrender.enabled = false;
                }
            } else {
                bodyrender.enabled = true;
            }
        } else if(Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
            Destroy(this.gameObject);
        }
		//manage movement
		if(Time.timeScale != 0){
			if (Input.GetKey(KeyCode.A)){
				if (rb.velocity.x > -maxspeed){
					rb.velocity = new Vector3(rb.velocity.x-acceleration, rb.velocity.y, 0);
				}
			} else if (Input.GetKey(KeyCode.D)){
				if (rb.velocity.x < maxspeed){
					rb.velocity = new Vector3(rb.velocity.x+acceleration, rb.velocity.y, 0);
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
			if (Input.GetKey(KeyCode.Space)){
				if(onground){
					rb.velocity = new Vector3(rb.velocity.x, jumpheight, 0);
					onground = false;
				}
			}
		}
		//disable friction when in air (prevents multiple bugs)
		if (onground){
			this.transform.GetComponent<Collider>().material.dynamicFriction = 0.75f;
			this.transform.GetComponent<Collider>().material.staticFriction = 0.75f;
		} else {
			this.transform.GetComponent<Collider>().material.dynamicFriction = 0;
			this.transform.GetComponent<Collider>().material.staticFriction = 0;
		}
	}
	
	public void TakeDamage(){
		if (!invincible){
			currHealth--;
			invincible = true;
			invincibleTimeout = Time.time + 2f;
		}
	}
	
	private bool beingdamaged = false;
	void OnCollisionStay(Collision other){
		//detect if hit damaging surface
		if (other.transform.tag == "danger") {
			beingdamaged = true;
			if(clone){
				Destroy(this.gameObject);
			}
		}
    }
	void OnCollisionExit(Collision other){
		//detect if hit damaging surface
		if (other.transform.tag == "danger") {
			beingdamaged = false;
		}
    }
	
	//detecting if player can jump
	void OnTriggerEnter(Collider other) {
        if (other.tag == "ground" || other.tag == "clone" || other.tag == "danger") {
			if (!onground){
				//prevent landing friction stopping player moving sideways
				if (Input.GetKey(KeyCode.A)){
					rb.velocity = new Vector3(-acceleration*9, rb.velocity.y, 0);
				}
				if (Input.GetKey(KeyCode.D)){
					rb.velocity = new Vector3(acceleration*9, rb.velocity.y, 0);
				}
			}
            onground = true;
		}
    }
	void OnTriggerStay(Collider other) {
        if (other.tag == "ground" || other.tag == "clone" || other.tag == "danger") {
            onground = true;
		}
    }
	void OnTriggerExit(Collider other){
        if (other.tag == "ground" || other.tag == "clone" || other.tag == "danger") {
            onground = false;
		}
    }
}
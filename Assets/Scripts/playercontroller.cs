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
    public bool clone = false;
	public int CloneID = 0;
    public float despawnTime = 9999999999999999999;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!clone) {
            //restart level if player somehow falls out of level
            if (this.transform.position.y < -2) {
                GameObject.Find("LevelLoader").GetComponent<levelcheck>().RestartLevel();
            }
        } else if(Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
            Destroy(this.gameObject);
        }
		//manage movement
		if(Time.timeScale != 0){
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
				if (rb.velocity.x > -maxspeed){
					rb.velocity = new Vector3(rb.velocity.x-acceleration, rb.velocity.y, 0);
				}
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
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
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
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
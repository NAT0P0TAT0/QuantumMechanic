using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntangledClone : MonoBehaviour {

	private GameObject Player;
	public int CloneID = 0;
    public float despawnTime = 9999999999999999999;
    public float spawnTime = 0;
    private float maxWidth = 0.65f;

	// Use this for initialization
	void Start () {
		Renderer rend = this.transform.GetChild(0).GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
		rend.material.color = new Color(0.1f, 1, 1, 1);
		Player = GameObject.Find("Player-char");
		this.gameObject.AddComponent<FixedJoint> ();  
		this.gameObject.GetComponent<FixedJoint>().connectedBody = Player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
            Destroy(this.gameObject);
			//if the player is in the air and not moving when this despawns sometimes
			//the player doesnt fall, giving them a little vertical speed fixes that
			Vector3 speed = Player.GetComponent<Rigidbody>().velocity;
			Player.GetComponent<Rigidbody>().velocity = new Vector3(speed.x, speed.y + 0.03f, speed.z);
        }
        //calculate remaining time (relative to remaining time when first spawned)
        float maxRemainingTime = despawnTime - spawnTime;
        float remainingTime = despawnTime - Time.timeSinceLevelLoad;
        float timePercentage = remainingTime / maxRemainingTime;
        float indicatorWidth = maxWidth * timePercentage;
        this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(indicatorWidth, 0.25f, 1);
	}
	
	//detecting if player can jump
	void OnTriggerStay(Collider other) {
        if (other.tag == "ground" || other.tag == "clone") {
			Player.GetComponent<playercontroller>().onground = true;
		}
    }
	void OnTriggerExit(Collider other){
        if (other.tag == "ground" || other.tag == "clone") {
            Player.GetComponent<playercontroller>().onground = false;
		}
    }
}

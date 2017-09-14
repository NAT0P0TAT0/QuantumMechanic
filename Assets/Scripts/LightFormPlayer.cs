using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFormPlayer : MonoBehaviour {

	private Rigidbody rb;
	public int speed = 10;
    public float despawnTime = 5;
	private Vector3 currspeed;
	private GameObject player;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
		player = GameObject.Find("Player-char");
	}
	
	// Update is called once per frame
	void Update () {
		if(player.GetComponent<QuantumAbilities>().InLightForm){
			//manage movement speed
			currspeed = rb.velocity;
			if(currspeed.x == 0 && currspeed.y == 0){
				if(player.transform.GetChild(0).transform.localScale.x < 0){
					currspeed.x = -0.5f;
				} else {
					currspeed.x = 0.5f;
				}
			}
			currspeed.z = 0;
			rb.velocity = currspeed.normalized*speed;
			
			//render player facing right way
			if(currspeed.x < 0){
				this.transform.GetChild(0).transform.localScale = new Vector3(-0.9f, 2, 0.9f);
			} else {
				this.transform.GetChild(0).transform.localScale = new Vector3(0.9f, 2, 0.9f);
			}
			
			//turn back
			if (Time.timeSinceLevelLoad > despawnTime){
				GameObject.Find("Player-char").GetComponent<QuantumAbilities>().CancelLightMode(false);
			}
		} else {
			despawnTime = Time.timeSinceLevelLoad + 5f;
		}
	}
}

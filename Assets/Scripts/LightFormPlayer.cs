using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFormPlayer : MonoBehaviour {

	private Rigidbody rb;
	public int speed = 10;
    public float despawnTime = 5;
	private Vector3 playerpos;
	private Vector3 currspeed;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Player-char").GetComponent<QuantumAbilities>().InLightForm){
			playerpos = this.transform.position;
			//manage movement speed
			currspeed = rb.velocity;
			currspeed.z = 0;
			rb.velocity = currspeed.normalized*speed;
			
			//turn back
			if (Time.timeSinceLevelLoad > despawnTime){
				GameObject.Find("Player-char").transform.position = playerpos;
				GameObject.Find("Player-char").GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
				this.transform.position = new Vector3(2,999,0);
				GameObject.Find("Player-char").GetComponent<QuantumAbilities>().InLightForm = false;
			}
		} else {
			despawnTime = Time.timeSinceLevelLoad + 5f;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFormPlayer : MonoBehaviour {

	private Rigidbody rb;
	public int speed = 10;
    public float despawnTime = 5;
	private Vector3 currspeed;
	private GameObject player;
	private float angle = 0;

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
			
			//turn render to face direction of travel
			Vector2 dummy = new Vector2(0, 1);
			Vector2 Direction = new Vector2(currspeed.normalized.x, currspeed.normalized.y);
			float ang = Vector2.Angle(dummy, Direction);
			if(currspeed.normalized.x > 0){ang = -ang;}
			
			angle += Time.deltaTime*10;
			//0 is up, 90 is left, 180 is down, 270 is right
			Debug.Log(angle +" "+ ang);
			
			this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,ang);
			
			//turn back
			if (Time.timeSinceLevelLoad > despawnTime){
				GameObject.Find("Player-char").GetComponent<QuantumAbilities>().CancelLightMode(false);
			}
		} else {
			despawnTime = Time.timeSinceLevelLoad + 5f;
		}
	}
}

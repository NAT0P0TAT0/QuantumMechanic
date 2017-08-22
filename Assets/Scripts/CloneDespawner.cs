using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDespawner : MonoBehaviour {
    
    public float despawnTime = 9999999999999999999;
	public bool tunneler = false;
	public int CloneID = 0;
	private Transform Player;
	
	void Start (){
        Player = GameObject.Find("Player-char").transform;
	}

	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
			if(tunneler){
				Player.position = this.transform.position;
				Player.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
				GameObject.Find("Player-char").GetComponent<QuantumAbilities>().tunnelerout = false;
			}
            Destroy(this.gameObject);
        }
	}
	
	void OnCollisionStay(Collision other){
		//detect if hit damaging surface
		if (other.transform.tag == "danger") {
			Destroy(this.gameObject);
		}
    }
}

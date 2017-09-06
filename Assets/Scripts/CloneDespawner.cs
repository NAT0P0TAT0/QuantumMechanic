using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDespawner : MonoBehaviour {
    
    public float despawnTime = 9999999999999999999;
	public int CloneID = 0;
	private Transform Player;
	
	void Start (){
        Player = GameObject.Find("Player-char").transform;
	}

	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
            Destroy(this.gameObject);
        }
	}
}

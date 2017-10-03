using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDespawner : MonoBehaviour {
    
    public float despawnTime = 9999999999999999999;
	public int CloneID = 0;
    public float spawnTime = 0;
    private float maxWidth = 0.65f;
	
	void Start (){
	}

	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > despawnTime) {//despawn clones after timeout
            Destroy(this.gameObject);
        }
        //calculate remaining time (relative to remaining time when first spawned)
        float maxRemainingTime = despawnTime-spawnTime;
        float remainingTime = despawnTime - Time.timeSinceLevelLoad;
        float timePercentage = remainingTime / maxRemainingTime;
        float indicatorWidth = maxWidth * timePercentage;
        this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(indicatorWidth, 0.25f, 1);
	}
}

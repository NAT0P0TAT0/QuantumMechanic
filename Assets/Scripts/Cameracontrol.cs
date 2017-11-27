using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontrol : MonoBehaviour {

	public List<GameObject> targets;
	public int MinZoom = 10;
	private float currZoom = 10f;
	private int minSeparation = 8;
	public int maxZoom = 20;
	public int Xlimit = 9999999;
	public int levelheight = 20;
	public float camSpeed = 3.5f;
	private float targetZoom = 10f;
	private Vector3 targetPos;

	// Use this for initialization
	void Start () {
		currZoom = MinZoom;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.Find("Player-char").GetComponent<QuantumAbilities>().InLightForm){
			targets[0] = GameObject.Find("Player-char");
		} else {
			targets[0] = GameObject.Find("Player-Light-Form");
		}
		Vector3 center = new Vector3(0,0,0);
		//find center of camera targets and manage min zoom
		if (targets.Count > 1){
			Bounds targetbounds = new Bounds(targets[0].transform.position, Vector3.zero);
			for (int i = 1; i < targets.Count; i++){
				targetbounds.Encapsulate(targets[i].transform.position); 
			}
			center = targetbounds.center;
			float dist = targetbounds.size.x;
			if(targetbounds.size.y > dist){dist = targetbounds.size.y;}
			currZoom = (dist - minSeparation) + MinZoom;
		} else {
			center = targets[0].transform.position;
			currZoom = MinZoom;
		}
		
		//manage max zoom
		if (currZoom > maxZoom) {
			float zoomdifference = maxZoom/currZoom;
			currZoom = ((currZoom - MinZoom)*(zoomdifference*zoomdifference*zoomdifference*zoomdifference)) + MinZoom;
		}
        int tempMinZoom = MinZoom;
		//zoomout if player holding Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) {
				float vertSpeed = targets[0].GetComponent<Rigidbody>().velocity.y;
				if (vertSpeed < 0.2f && vertSpeed > -0.2f){
					tempMinZoom = (int)(MinZoom*3);
				}
			}
		}
        if (currZoom < tempMinZoom){
            currZoom = tempMinZoom;
		}
		
		//move cam position and adjust zoom smoothly
		if(targetZoom > currZoom){
			targetZoom -= camSpeed*Time.deltaTime*camSpeed/2;
			if(targetZoom < currZoom){targetZoom = currZoom;}
		} else if(targetZoom < currZoom){
			targetZoom += camSpeed*Time.deltaTime*camSpeed/2;
			if(targetZoom > currZoom){targetZoom = currZoom;}
		}
		targetPos = new Vector3(center.x, center.y, -10);
		this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 0.015f*camSpeed);
        
		//make sure camera doesnt zoom so far out that you can see outside the level
        if (targetZoom > levelheight) { targetZoom = levelheight; }
		Camera.main.orthographicSize = targetZoom/2;
		
		float aspectratio = (float)Screen.width/(float)Screen.height;
		//make sure camera does not leave level bounds
		if (this.transform.position.y < Camera.main.orthographicSize-0.5f) {
			this.transform.position = new Vector3(this.transform.position.x,Camera.main.orthographicSize-0.5f,this.transform.position.z);
		}
		if (this.transform.position.x < Camera.main.orthographicSize*aspectratio) {
			this.transform.position = new Vector3(Camera.main.orthographicSize*aspectratio,this.transform.position.y,this.transform.position.z);
		}
		if (this.transform.position.x > Xlimit-Camera.main.orthographicSize*aspectratio-1) {
			this.transform.position = new Vector3(Xlimit-Camera.main.orthographicSize*aspectratio-1,this.transform.position.y,this.transform.position.z);
        }
        if (this.transform.position.y > levelheight - Camera.main.orthographicSize-0.5f) {
            this.transform.position = new Vector3(this.transform.position.x, levelheight - Camera.main.orthographicSize - 0.5f, this.transform.position.z);
        }
	}
}

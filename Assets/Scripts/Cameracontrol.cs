using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontrol : MonoBehaviour {

	public List<GameObject> targets;
	public int MinZoom = 10;
	private float currZoom = 10f;
	private int minSeparation = 8;
	private float playerfocus = 0.5f;
	public int maxZoom = 20;
	private Vector3 PlayerPos;
	private Vector3 OtherPos;
	public int Xlimit = 9999999;
	public int levelheight = 20;
	public float camSpeed = 3.5f;
	private float targetZoom = 10f;
	private Vector3 targetPos;
    private int levelSideBorder = 6;

	// Use this for initialization
	void Start () {
		currZoom = MinZoom;
	}
	
	// Update is called once per frame
	void Update () {
		playerfocus = 0.5f;
		if(!GameObject.Find("Player-char").GetComponent<QuantumAbilities>().InLightForm){
			targets[0] = GameObject.Find("Player-char");
		} else {
			targets[0] = GameObject.Find("Player-Light-Form");
		}
		PlayerPos = targets[0].transform.position;
		OtherPos = targets[0].transform.position;
		if (targets.Count > 2){
			Vector3 average = Vector3.zero;
			for (int i = 1; i < targets.Count; i++){
				average += targets[i].transform.position;
			}
			OtherPos = average/(targets.Count-1);
		} else if (targets.Count == 2){
			OtherPos = targets[1].transform.position;
		}
		float dist = Vector3.Distance(PlayerPos, OtherPos);
		
		//manage min and max
		if (dist > minSeparation) {
			currZoom = (dist - minSeparation) + MinZoom;
		} else {
			currZoom = MinZoom;
		}
		if (currZoom > maxZoom) {
			float zoomdifference = maxZoom/currZoom;
			playerfocus = playerfocus*zoomdifference*zoomdifference*zoomdifference;
			currZoom = ((currZoom - MinZoom)*(zoomdifference*zoomdifference*zoomdifference*zoomdifference)) + MinZoom;
		}
        int tempMinZoom = MinZoom;
		//zoomout if player holding Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow)) {
				float vertSpeed = targets[0].GetComponent<Rigidbody>().velocity.y;
				if (vertSpeed < 0.2f && vertSpeed > -0.2f){
					tempMinZoom = (int)(MinZoom*2);
				}
			}
		}
        if (currZoom < tempMinZoom){
            currZoom = tempMinZoom;
		}
		
		//move cam position and adjust zoom smoothly
		if(targetZoom > currZoom){
			targetZoom -= camSpeed*Time.deltaTime;
			if(targetZoom < currZoom){targetZoom = currZoom;}
		} else if(targetZoom < currZoom){
			targetZoom += camSpeed*Time.deltaTime;
			if(targetZoom > currZoom){targetZoom = currZoom;}
		}
		Vector3 center = Vector3.Lerp(PlayerPos, OtherPos, playerfocus);
		targetPos = new Vector3(center.x, center.y, -10);
		this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 0.015f*camSpeed);
        //make camera doesnt zoom so far out that you can see outside the level
        if (targetZoom > levelheight) { targetZoom = levelheight; }
		Camera.main.orthographicSize = targetZoom/2;
		
		
		//make sure camera does not leave level bounds
		if (this.transform.position.y < Camera.main.orthographicSize-0.5f) {
			this.transform.position = new Vector3(this.transform.position.x,Camera.main.orthographicSize-0.5f,this.transform.position.z);
		}
		if (this.transform.position.x < Camera.main.orthographicSize+levelSideBorder) {
			this.transform.position = new Vector3(Camera.main.orthographicSize+levelSideBorder,this.transform.position.y,this.transform.position.z);
		}
		if (this.transform.position.x > Xlimit-Camera.main.orthographicSize-levelSideBorder) {
			this.transform.position = new Vector3(Xlimit-Camera.main.orthographicSize-levelSideBorder,this.transform.position.y,this.transform.position.z);
        }
        if (this.transform.position.y > levelheight - Camera.main.orthographicSize - 0.5f) {
            this.transform.position = new Vector3(this.transform.position.x, levelheight - Camera.main.orthographicSize - 0.5f, this.transform.position.z);
        }
	}
}

using UnityEngine;
using System.Collections;

public class exit : MonoBehaviour {
	private GameObject go;
	public int machineCount = 0;
	private bool active = false;
	private bool machines = false;
	private Vector3 playerPos;
	private Vector3 pointerPos;
	public Transform pointerPrefab;
	private Transform pointer;
	
	void Start(){
		go = GameObject.Find("LevelLoader");
	}
	
	void Update(){
		if(machineCount > 0 || Time.timeSinceLevelLoad > 2){
			machines = true;
		}
		if(machines && !active && machineCount < 1){
			active = true;
			Vector3 ExitPos = GameObject.Find("ExitClosed").transform.position;
			ExitPos.y -= 0.5f;
			GameObject.Find("ExitClosed").transform.position = new Vector3(-999, -999, 0);
			this.transform.position = ExitPos;
			pointer = Instantiate(pointerPrefab, new Vector3(0, 0, 0), transform.rotation);
		}
		if(active){
			//show pointer
			playerPos = GameObject.Find("Player-char").transform.position;
			float dist = Vector3.Distance(playerPos, this.transform.position);
			if(dist > 2){
				pointerPos = playerPos - ((playerPos - this.transform.position).normalized);
				pointerPos.z = -1;
				pointer.position = pointerPos;
				pointer.LookAt(this.transform);
			} else {
				pointer.position = new Vector3(-999, -999, 0);
			}
		}
	}
	
	public void reset(){
		machineCount = 0;
		active = false;
		machines = false;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Player-char"){
			go.GetComponent<levelcheck>().finished = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.gameObject.name == "Player-char"){
			go.GetComponent<levelcheck>().finished = false;
		}
	}
}

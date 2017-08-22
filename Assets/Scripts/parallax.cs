using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {
	private Vector3 camerapos;
	public float backgroundscale = 0.25f;
	
	void Start(){
		GameObject.Find("Background").transform.position = new Vector3(-999,-999,10f);
		GameObject.Find("Background-moving").transform.position = new Vector3(-999,-999,5f);
	}
	
	void Update() {
		camerapos = GameObject.Find("Main Camera").transform.position;
		GameObject.Find("Background").transform.position = new Vector3(camerapos.x,camerapos.y,10f);
		GameObject.Find("Background-moving").transform.position = new Vector3(camerapos.x*backgroundscale,camerapos.y*backgroundscale,5f);
	}
}

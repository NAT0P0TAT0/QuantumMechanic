using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour {
	void Update() {
		if (Input.GetKey(KeyCode.R)){
			SceneManager.LoadScene("game");
		}
	}
}

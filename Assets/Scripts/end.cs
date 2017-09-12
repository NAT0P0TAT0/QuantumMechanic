using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour {
	void Update() {
        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Q)){
			SceneManager.LoadScene("MainMenu");
		}
	}
}

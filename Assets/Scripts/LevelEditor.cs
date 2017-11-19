using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelEditor : MonoBehaviour {

	private byte[] imageBuffer;
	private int picWidth = 200;
	private int picHeight = 100;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//detect keypresses
		if (Input.GetKey(KeyCode.G)){
			SaveLevel();
		}
		
		//move camera
		Vector3 campos = GameObject.Find("Main Camera").transform.position;
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
			campos.x -= Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
			campos.x += Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
			campos.y += Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
			campos.y -= Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if(campos.y > 100){campos.y = 100;}
		if(campos.x > 100){campos.x = 100;}
		if(campos.y < -100){campos.y = -100;}
		if(campos.x < -100){campos.x = -100;}
		GameObject.Find("Main Camera").transform.position = campos;
		Camera.main.orthographicSize -= 2*Input.GetAxis("Mouse ScrollWheel");
		if(Camera.main.orthographicSize < 3){Camera.main.orthographicSize = 3;}
		if(Camera.main.orthographicSize > 45){Camera.main.orthographicSize = 45;}
	}
	
	void SaveLevel(){
		//get level dimensions
		picWidth = 200;
		picHeight = 200;
		//create texture
		Texture2D texture = new Texture2D(picWidth, picHeight);
		for (int y = 0; y < picHeight; y++){
            for (int x = 0; x < picWidth; x++){
				Color color = GetColor(x,y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
		string filename = "CustomLevels/test.png";
		//save texture to image
		SaveTextureToFile(texture, filename);
	}
	
	Color GetColor(int x, int y){
		//default colour is black
		float r = 0f;
		float g = 0f;
		float b = 0f;
		//if an object is detected set a colour thats not black
		r = y/(float)picHeight;
		g = x/(float)picWidth;
		b = ((1/(y+0.00000001f))+(1/(x+0.00000001f)))*(picWidth/10f);
		//return the colour
		return new Color(r, g, b, 1);
	}
	
	void SaveTextureToFile (Texture2D texture, string filename) {
		//create folder and file if they dont already exist
		//if they do exist overwrite the file
		System.IO.FileInfo file = new System.IO.FileInfo(filename);
		file.Directory.Create();
		System.IO.File.WriteAllBytes (file.FullName, texture.EncodeToPNG());
	}
}

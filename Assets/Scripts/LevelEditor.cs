using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelEditor : MonoBehaviour {

	private byte[] imageBuffer;
	private int picWidth = 200;
	private int picHeight = 100;
	
	private int angledcap = 23;
	private float menuoffset = 5;
	private int selectedItem = -1;
	public Transform[] levelObjects;
	private Transform objectPreview;

	// Use this for initialization
	void Start () {
	}
	
	Vector3 GetMenuObjectPos(int id){
		float x = 0;
		float y = 0;
		int itemsperrow = 10;
		float scale = (Camera.main.orthographicSize/5f)*1.3f;
		x = (((id%itemsperrow)-(itemsperrow/2))+0.5f)*scale;
		y = (Mathf.Floor((float)id/itemsperrow) + 1)*-scale;
		y -= menuoffset*Camera.main.orthographicSize;
		return new Vector3(x,y,0);
	}
	
	// Update is called once per frame
	void Update () {
		//show available objects as a 'menu'
		Vector3 objPos = new Vector3(0,0,-10);
		for (int i = 0; i < levelObjects.Length; i++){
			//get pos and size relative to camera
			objPos = Camera.main.transform.position;
			objPos.z = -10;
			float scale = Camera.main.orthographicSize/5f;
			levelObjects[i].transform.localScale = new Vector3(scale,scale,1);
			//get positions for indiviaul objects
			objPos += GetMenuObjectPos(i);
			levelObjects[i].transform.position = objPos;
		}
		//change selected item
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonUp(1)){
			if(objectPreview){
				if(selectedItem > angledcap){
					objectPreview.position = new Vector3(-9999,-9999,0);
					objectPreview = null;
				} else {
					Destroy(objectPreview.gameObject);
				}
			}
			selectedItem = -1;
		}
		//open and close objects menu
		if (Input.GetKey(KeyCode.LeftShift)){
			if(menuoffset > 0){
				menuoffset -= Time.deltaTime*2;
			} else {
				menuoffset = 0;
			}
			showlevelname = false;
			//get mouseover text
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				int id = GetObjectId(hit.transform.gameObject.name);
				mouseovertext = "";
				if(id == 0){mouseovertext = "Block";}
				if(id == 1){mouseovertext = "Wall";}
				if(id == 2){mouseovertext = "Entance";}
				if(id == 3){mouseovertext = "Exit";}
				if(id == 4){mouseovertext = "Wire on wall";}
				if(id == 5){mouseovertext = "Wire on block";}
				if(id == 6){mouseovertext = "Broken machine";}
				if(id == 7){mouseovertext = "Button";}
				if(id == 8){mouseovertext = "Pressureplate";}
				if(id == 9){mouseovertext = "Pressureplate with wall";}
				if(id == 10){mouseovertext = "Lever (left)";}
				if(id == 11){mouseovertext = "Lever (right)";}
				if(id == 12){mouseovertext = "Camera (left)";}
				if(id == 13){mouseovertext = "Camera (right)";}
				if(id == 14){mouseovertext = "Retractable block (on)";}
				if(id == 15){mouseovertext = "Retractable block (off)";}
				if(id == 16){mouseovertext = "Conveyor belt (left)";}
				if(id == 17){mouseovertext = "Conveyor belt (right)";}
				if(id == 18){mouseovertext = "Glass block";}
				if(id == 19){mouseovertext = "Light absorbing block";}
				if(id == 20){mouseovertext = "Thin block (horizontal)";}
				if(id == 21){mouseovertext = "Thin block (vertical)";}
				if(id == 22){mouseovertext = "Thin block with wall (horizontal)";}
				if(id == 23){mouseovertext = "Thin block with wall (vertical)";}
				if(id == 24){mouseovertext = "Angled block (bottom left)";}
				if(id == 25){mouseovertext = "Angled block (bottom right)";}
				if(id == 26){mouseovertext = "Angled block (top left)";}
				if(id == 27){mouseovertext = "Angled block (top right)";}
			}
		} else {
			showlevelname = true;
			if(menuoffset < 1.3f){
				menuoffset += Time.deltaTime*2;
			} else {
				menuoffset = 1.3f;
			}
			mouseovertext = "";
		}
		
		//move camera
		Vector3 campos = GameObject.Find("Main Camera").transform.position;
		if (Input.GetKey(KeyCode.A)){
			campos.x -= Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.D)){
			campos.x += Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.W)){
			campos.y += Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if (Input.GetKey(KeyCode.S)){
			campos.y -= Time.deltaTime*100*Camera.main.orthographicSize/75;
		}
		if(campos.y > 100){campos.y = 100;}
		if(campos.x > 100){campos.x = 100;}
		if(campos.y < -100){campos.y = -100;}
		if(campos.x < -100){campos.x = -100;}
		campos.z = -20;
		GameObject.Find("Main Camera").transform.position = campos;
		if (Input.GetKey(KeyCode.Q)){
			Camera.main.orthographicSize -= 9*Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.E)){
			Camera.main.orthographicSize += 9*Time.deltaTime;
		}
		Camera.main.orthographicSize -= 4*Input.GetAxis("Mouse ScrollWheel");
		if(Camera.main.orthographicSize < 3){Camera.main.orthographicSize = 3;}
		if(Camera.main.orthographicSize > 45){Camera.main.orthographicSize = 45;}
		//recenter camera
		if (Input.GetKeyDown(KeyCode.C)){
			campos.x = 0;
			campos.y = 0;
			Camera.main.orthographicSize = 8;
			GameObject.Find("Main Camera").transform.position = campos;
		}
		
		
		//pick and choose objects
		if (Input.GetMouseButtonUp(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				Vector3 AimPos = hit.point;
				AimPos.x = Mathf.FloorToInt(AimPos.x+0.5f);
				AimPos.y = Mathf.FloorToInt(AimPos.y+0.5f);
				AimPos.z = 0;
				if(selectedItem == -1 || Input.GetKey(KeyCode.LeftShift)){
					//pick an object and create preview that follows mouse
					if(objectPreview){
						if(selectedItem > angledcap){
							objectPreview.position = new Vector3(-9999,-9999,0);
							objectPreview = null;
						} else {
							Destroy(objectPreview.gameObject);
						}
					}
					selectedItem = GetObjectId(hit.transform.gameObject.name);
					if(selectedItem > -1){
						objectPreview = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
					}
				} else {
					//check if object already exists at position, delete it and place object
					DeleteObjectAtPos(AimPos);
					Transform newobject = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
					//check if player placed a door, remove old doors so theres only 1
					if(newobject.gameObject.name.Contains("Entry")){
						foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
							if (fooObj.name.Contains("Entry")){
								Destroy(fooObj);
							}
						}
						newobject = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
						objectPreview = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
					}
					if(newobject.gameObject.name.Contains("Exit")){
						foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
							if (fooObj.name.Contains("Exit")){
								Destroy(fooObj);
							}
						}
						newobject = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
						objectPreview = Instantiate(levelObjects[selectedItem], AimPos, transform.rotation);
					}
					newobject.localScale = new Vector3(1,1,1);
				}
			}
		}
		//delete objects
		if (Input.GetMouseButtonUp(1)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				if(hit.transform.gameObject.name.Contains("Clone")){
					int id = GetObjectId(hit.transform.gameObject.name);
					if(id > angledcap){
						hit.transform.position = new Vector3(-9999,-9999,0);
					} else if(id > -1){
						Destroy(hit.transform.gameObject);
					}
				}
			}
		}
		//show placement preview
		if(objectPreview){
			objectPreview.localScale = new Vector3(1,1,1);
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)){
				Vector3 AimPos = hit.point;
				AimPos.x = Mathf.FloorToInt(AimPos.x+0.5f);
				AimPos.y = Mathf.FloorToInt(AimPos.y+0.5f);
				AimPos.z = -2;
				objectPreview.position = AimPos;
			}
		}
	}
	
	
	
	void DeleteObjectAtPos(Vector3 pos){
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
            if (fooObj.name.Contains("(Clone)") && fooObj.transform.position == pos){
				Destroy(fooObj);
            }
		}
	}
	
	int GetObjectId(string name){
		int id = -1;
		for (int i = 0; i < levelObjects.Length; i++){
			if(name.Contains(levelObjects[i].gameObject.name)){
				id = i;
			}
		}
		return id;
	}
	
	void SaveLevel(){
		//check that level meets requirements
		bool valid = false;
		bool entry = false;
		bool exit = false;
		bool floorunderentry = false;
		bool floorunderexit = false;
		errormessage = "";
		Vector3 entrypos = new Vector3(-9999,-9999,0);
		Vector3 exitpos = new Vector3(-9999,-9999,0);
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
            if (fooObj.name.Contains("(Clone)")){
				if (fooObj.name.Contains("Entry")){
					entry = true;
					entrypos = fooObj.transform.position;
				}
				if (fooObj.name.Contains("Exit")){
					exit = true;
					exitpos = fooObj.transform.position;
				}
            }
		}
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
            if (fooObj.name == "Block(Clone)"){
				if(fooObj.transform.position.x == entrypos.x){
					if(fooObj.transform.position.y == entrypos.y-1){
						floorunderentry = true;
					}
				}
				if(fooObj.transform.position.x == exitpos.x){
					if(fooObj.transform.position.y == exitpos.y-1){
						floorunderexit = true;
					}
				}
            }
		}
		if(!entry){
			errormessage = "The level needs an entrance!";
		} else if(!exit){
			errormessage = "The level needs an exit!";
		} else if(!floorunderentry || !floorunderexit){
			errormessage = "The level needs floors under the entrance and exit!";
		}
		if(entry && exit && floorunderentry && floorunderexit){
			valid = true;
			errormessage = "Level saved!";
		}
		if(valid){
			//get level dimensions
			int xmin = 999999;
			int ymin = 999999;
			int xmax = -99999;
			int ymax = -99999;
			foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
				if (fooObj.name.Contains("(Clone)")){
					if(fooObj.transform.position.x < xmin && fooObj.transform.position.x > -999){
						xmin = Mathf.FloorToInt(fooObj.transform.position.x);
					}
					if(fooObj.transform.position.y < ymin && fooObj.transform.position.y > -999){
						ymin = Mathf.FloorToInt(fooObj.transform.position.y);
					}
					if(fooObj.transform.position.x > xmax){
						xmax = Mathf.FloorToInt(fooObj.transform.position.x);
					}
					if(fooObj.transform.position.y > ymax){
						ymax = Mathf.FloorToInt(fooObj.transform.position.y);
					}
				}
			}
			//create texture
			picWidth = xmax-xmin+1;
			picHeight = ymax-ymin+1;
			Texture2D texture = new Texture2D(picWidth, picHeight);
			for (int y = ymin; y <= ymax; y++){
				for (int x = xmin; x <= xmax; x++){
					Color color = GetColor(x, y);
					texture.SetPixel(x-xmin, y-ymin, color);
				}
			}
			texture.Apply();
			string filename = "CustomLevels/"+levelname+".png";
			//save texture to image
			SaveTextureToFile(texture, filename);
		}
	}
	
	Color GetColor(int x, int y){
		//default colour is black
		float r = 0f;
		float g = 0f;
		float b = 0f;
		//if an object is detected set a colour thats not black
		Vector3 pos = new Vector3(x,y,0);
		Collider[] hitColliders = Physics.OverlapSphere(pos, 0.0001f);
		foreach(Collider collider in hitColliders){
			if(collider.gameObject.name.Contains("(Clone)")){
				Vector3 checkpos = collider.gameObject.transform.position;
				if(pos.x == checkpos.x && pos.y == checkpos.y){
					int objectID = GetObjectId(collider.gameObject.name);
					if(objectID == 0){r = 1f; g = 1f; b = 1f;}			//block
					if(objectID == 1){r = 0.33f; g = 0.33f; b = 0.33f;}	//wall
					if(objectID == 2){r = 0f; g = 1f; b = 0f;}			//entance
					if(objectID == 3){r = 0f; g = 0.66f; b = 0f;}		//exit
					if(objectID == 4){r = 0.66f; g = 0.66f; b = 0.33f;}	//wall wire
					if(objectID == 5){r = 1f; g = 1f; b = 0f;}			//block wire
					if(objectID == 6){r = 0.33f; g = 0.66f; b = 0.33f;}	//broken machine
					if(objectID == 7){r = 0f; g = 0f; b = 0.33f;}		//button
					if(objectID == 8){r = 0f; g = 0f; b = 0.66f;}		//pressureplate
					if(objectID == 9){r = 0.66f; g = 0f; b = 0.66f;}	//pressureplate - wall
					if(objectID == 10){r = 0.33f; g = 0.33f; b = 1f;}	//lever - left
					if(objectID == 11){r = 0.66f; g = 0.66f; b = 1f;}	//lever - right
					if(objectID == 12){r = 1f; g = 1f; b = 0.33f;}		//camera - left
					if(objectID == 13){r = 1f; g = 1f; b = 0.66f;}		//camera - right
					if(objectID == 14){r = 0f; g = 0f; b = 1f;}			//toggle block - on
					if(objectID == 15){r = 0.33f; g = 0.33f; b = 0.66f;}//toggle block - off
					if(objectID == 16){r = 0.33f; g = 0.33f; b = 0f;}	//conveyor belt - left
					if(objectID == 17){r = 0.66f; g = 0.66f; b = 0f;}	//conveyor belt - right
					if(objectID == 18){r = 1f; g = 0f; b = 0f;}			//glass block
					if(objectID == 19){r = 0.33f; g = 0f; b = 0f;}		//light cancelling block
					if(objectID == 20){r = 1f; g = 0f; b = 1f;}			//thin block - horiz
					if(objectID == 21){r = 0.66f; g = 0.33f; b = 0.66f;}//thin block - vert
					if(objectID == 22){r = 1f; g = 0.66f; b = 1f;}		//thin block - horiz wall
					if(objectID == 23){r = 1f; g = 0.33f; b = 1f;}		//thin block - vert wall
					if(objectID == 24){r = 1f; g = 0.66f; b = 0.66f;}	//wedge tile - bottom left
					if(objectID == 25){r = 1f; g = 0.33f; b = 0.33f;}	//wedge tile - bottom right
					if(objectID == 26){r = 0.66f; g = 0.33f; b = 0.33f;}//wedge tile - top left
					if(objectID == 27){r = 0.66f; g = 0f; b = 0f;}		//wedge tile - top right
				}
			}
		}
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
	
	Vector2 labelsize = new Vector2(0, 0);
	Vector2 labelpos = new Vector2(0, 0);
	public GUIStyle textStyle;
	private string errormessage = "";
	private string mouseovertext = "";
	private bool showlevelname = false;
	private string levelname = "customlevelname";
	//display text
	void OnGUI() {
		int fontsize = 25;
		labelsize.y = 40;
		if(Screen.width < 725){
			fontsize = Screen.width/29;
			labelsize.y = fontsize + 10;
		}
		textStyle.fontSize = fontsize;
		labelpos.x = 0;
		labelpos.y = 0;
		labelsize.x = Screen.width;
		GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "W/A/S/D to move camera, Q/E/Mousewheel to zoom", textStyle);
		labelpos.y += labelsize.y;
		GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "Hold SHIFT to show list and LEFT CLICK to pick an object", textStyle);
		labelpos.y += labelsize.y;
		GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "LEFT CLICK to place objects, RIGHT CLICK to delete objects", textStyle);
		if(errormessage != ""){
			textStyle.normal.textColor = Color.red;
			labelpos.y += labelsize.y;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), errormessage, textStyle);
			textStyle.normal.textColor = Color.white;
		}
		if(showlevelname){
			labelsize.y = 30;
			textStyle.fontSize = Mathf.RoundToInt(fontsize*0.75f);
			labelpos.y = Screen.height-labelsize.y;
			levelname = GUI.TextField(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), levelname, textStyle);
			labelpos.y -= labelsize.y;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "Edit the levels name below", textStyle);
			//save the level to a file
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Save the level")){
				if(objectPreview){
					if(objectPreview.name.Contains("AngledTile")){
						objectPreview.position = new Vector3(-9999,-9999,0);
						objectPreview = null;
					} else {
						DestroyImmediate(objectPreview.gameObject);
					}
				}
				selectedItem = -1;
				SaveLevel();
			}
		}
		if(mouseovertext != ""){
			textStyle.fontSize = fontsize/2;
			labelsize.x = 250;
			labelsize.y = 30;
			labelpos.x = Input.mousePosition.x-125;
			labelpos.y = Screen.height - Input.mousePosition.y - 30;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), mouseovertext, textStyle);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelEditor : MonoBehaviour {

	private int picWidth = 200;
	private int picHeight = 100;
	
	private int angledcap = 23;
	private float menuoffset = 5;
	private int selectedItem = -1;
	public Transform[] levelObjects;
	private Transform objectPreview;

	// Use this for initialization
	void Start () {
		//see if player chose to edit a pre-existing level or make a new one
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("LevelContinue")){
			levelname = fooObj.name;
			Destroy(fooObj);
			LoadLevel(levelname);
		}
	}
	
	void LoadLevel(string levelname){
		string path = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer) {
			path += "/../../";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path += "/../";
		}
		int pathindex = path.IndexOf("/Quantum Mechanic_Data");
		if(pathindex  < 0) {
			pathindex = path.IndexOf("/Assets");
		}
		if(pathindex >= 0) {
			path = path.Substring(0, pathindex);
			path += "/CustomLevels";
			DirectoryInfo dir = new DirectoryInfo(@path);
			string[] extensions = new[] { ".png", ".PNG" };
			FileInfo[] info = dir.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
			for (int i = 0; i < info.Length; i++){
				string folderpath = path.ToString();
				string filepath = info[i].ToString();
				folderpath = folderpath.Replace("/", "\\");
				filepath = filepath.Replace(folderpath, "");
				filepath = filepath.Replace("\\", "");
				filepath = filepath.Replace(".png", "");
				filepath = filepath.Replace(".PNG", "");
				if(filepath == levelname){
					//level found
					GenerateLevel(info[i].ToString());
					errormessage = "Level loaded";
				}
			}
		}
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
			removePreview();
			errormessage = "";
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
					removePreview();
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
			errormessage = "";
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
	
	bool valid = false;
	void SaveLevel(){
		//check that level meets requirements
		valid = false;
		bool named = false;
		bool entry = false;
		bool exit = false;
		bool floorunderentry = false;
		bool floorunderexit = false;
		bool doorsunblocked = false;
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
		doorsunblocked = true;
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
            if (fooObj.name == "Block(Clone)"){
				if(fooObj.transform.position.x == entrypos.x){
					if(fooObj.transform.position.y == entrypos.y-1){
						floorunderentry = true;
					}
					if(fooObj.transform.position.y == entrypos.y+1){
						doorsunblocked = false;
					}
				}
				if(fooObj.transform.position.x == exitpos.x){
					if(fooObj.transform.position.y == exitpos.y-1){
						floorunderexit = true;
					}
					if(fooObj.transform.position.y == exitpos.y+1){
						doorsunblocked = false;
					}
				}
            }
		}
		if(levelname != "ENTER LEVEL NAME HERE" && levelname != "" && levelname != " " && levelname != null){
			named = !string.IsNullOrEmpty(levelname) && levelname.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
		}
		if(!named){
			errormessage = "The level needs a valid name!";
		} else if(!entry){
			errormessage = "The level needs an entrance!";
		} else if(!exit){
			errormessage = "The level needs an exit!";
		} else if(!floorunderentry || !floorunderexit){
			errormessage = "The level needs floors under the entrance and exit!";
		} else if(!doorsunblocked){
			errormessage = "Cannot block the tops of doors!";
		}
		if(named && entry && exit && floorunderentry && floorunderexit && doorsunblocked){
			valid = true;
			errormessage = "Level saved";
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
					if(y == ymin && x == xmin && Lightmode){color = Color.red;}
					if(y == ymin && x == xmin+1 && Tunnel){color = Color.green;}
					if(y == ymin && x == xmin+2 && Superpos){
						if(Entangle){
							color = Color.cyan;
						} else {
							color = Color.blue;
						}
					}
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
	
	private Texture2D levelfile;
	private byte[] fileData;
	private string levelfilepath = "";
	void GenerateLevel(string filepath){
		levelfilepath = filepath;
		//load level file as a texture
		levelfile = null;
		if (File.Exists(filepath)) {
			fileData = File.ReadAllBytes(filepath);
			levelfile = new Texture2D(2, 2);
			levelfile.LoadImage(fileData);
		}
		//check every single pixel in the image
		picWidth = levelfile.width;
		picHeight = levelfile.height;
		for(int y = 0; y < picHeight; y++){
			for(int x = 0; x < picWidth; x++){
				if (x < 3 && y == 0){
					Transform newobject = Instantiate(levelObjects[0], new Vector3(x, y, 0), transform.rotation);
					newobject.localScale = new Vector3(1,1,1);
					setAbilities(x, y, levelfile);//spawn basic tile and set available abilities
				} else {
					spawntile(x, y, levelfile);//spawn tile based on pixel colour
				}
			}
		}
	}
	private bool Superpos = false;
	private bool Tunnel = false;
	private bool Entangle = false;
	private bool Lightmode = false;
	private float red = 0;
	private float green = 0;
	private float blue = 0;
	void getColourValues(int x, int y, Texture2D levelfile){
		//get the pixels colour values
		Color pixelcol = levelfile.GetPixel(x, y);
		red = pixelcol.r;
		green = pixelcol.g;
		blue = pixelcol.b;
		//adjust for slight variation of decimals when reading pixels
		if(red > 0.8){red = 3;} else if(red > 0.45){red = 2;} else if(red > 0.15){red = 1;} else {red = 0;}
		if(green > 0.8){green = 3;} else if(green > 0.45){green = 2;} else if(green > 0.15){green = 1;} else {green = 0;}
		if(blue > 0.8){blue = 3;} else if(blue > 0.45){blue = 2;} else if(blue > 0.15){blue = 1;} else {blue = 0;}
	}
	void spawntile(int x, int y, Texture2D levelfile){
		getColourValues(x, y, levelfile);
		//place tiles based on colours
		int blockID = -1;
		if(red == 3 && green  == 3 && blue  == 3){
			blockID = 0;//white - standard tile
		} else if(red == 1 && green  == 1 && blue  == 1){
			blockID = 1;//dark grey - back wall tile
		} else if(red == 3 && green == 2 && blue == 2) {
			blockID = 24;//lighter red - angled surface (bottom left)
		} else if(red == 3 && green == 1 && blue == 1) { 
			blockID = 25;//light red - angled surface (bottom right)
		} else if(red == 3 && green == 0 && blue == 0) { 
			blockID = 18;//red - transparent tile
		} else if(red == 2 && green == 1 && blue == 1) { 
			blockID = 26;//grey red - angled surface (top left)
		} else if(red == 2 && green == 0 && blue == 0) { 
			blockID = 27;//dark red - angled surface (top right)
		} else if(red == 1 && green == 0 && blue == 0) { 
			blockID = 19;//darker red - black surface that cancels lightform
		} else if(red == 0 && green == 3 && blue == 0) { //green - player spawn
			blockID = 2;
		} else if(red == 1 && green == 2 && blue == 1) { //grey green - broken machine
			blockID = 6;
		} else if(red == 0 && green == 2 && blue == 0) { //dark green - level exit
			blockID = 3;
		} else if(red == 2 && green == 2 && blue == 3) { //lighter blue - lever (facing right)
			blockID = 11;
		} else if(red == 1 && green == 1 && blue == 3) { //light blue - lever (facing left)
			blockID = 10;
		} else if(red == 0 && green == 0 && blue == 3) { //blue - toggleable block (on)
			blockID = 14;
		} else if(red == 1 && green == 1 && blue == 2) { //grey blue - toggleable block (off)
			blockID = 15;
		} else if(red == 0 && green == 0 && blue == 2) { //dark blue - pressureplate
			blockID = 8;
		} else if(red == 0 && green == 0 && blue == 1) { //darker blue - button
			blockID = 7;
		} else if(red == 3 && green == 3 && blue == 2) { //lighter yellow - security camera (right)
			blockID = 13;
		} else if(red == 3 && green == 3 && blue == 1) { //light yellow - security camera (left)
			blockID = 12;
		} else if(red == 3 && green == 3 && blue == 0) { //yellow - wire on solid tile
			blockID = 5;
		} else if(red == 2 && green == 2 && blue == 1) { //grey yellow - wire on background tile
			blockID = 4;
		} else if(red == 2 && green == 2 && blue == 0) { //dark yellow - conveyror belt (right)
			blockID = 17;
		} else if(red == 1 && green == 1 && blue == 0) { //darker yellow - conveyror belt (left)
			blockID = 16;
		} else if(red == 3 && green == 2 && blue == 3) { //lighter magenta - thin floor/ceiling (with backwall)
			blockID = 22;
		} else if(red == 3 && green == 1 && blue == 3) { //light magenta - thin wall (with backwall)
			blockID = 23;
		} else if(red == 3 && green == 0 && blue == 3) { //magenta - thin floor/ceiling
			blockID = 20;
		} else if(red == 2 && green == 1 && blue == 2) { //grey magenta - thin wall
			blockID = 21;
		} else if(red == 2 && green == 0 && blue == 2) { //dark magenta - pressureplate (with backwall)
			blockID = 9;
		}
		if(blockID > -1){
			Transform newobject = Instantiate(levelObjects[blockID], new Vector3(x, y, 0), transform.rotation);
			newobject.localScale = new Vector3(1,1,1);
		}
	}
	void setAbilities(int x, int y, Texture2D levelfile){
		getColourValues(x, y, levelfile);
		if (x == 0 && y == 0){
			Superpos = false;
			Tunnel = false;
			Entangle = false;
			Lightmode = false;
		}
		if(red == 3 && green == 0 && blue == 0) { //red - wave particle duality enabled
			Lightmode = true;
		} else if(red == 0 && green == 3 && blue == 0) { //green - tunnleing enabled
			Tunnel = true;
		} else if(red == 0 && green == 0 && blue == 3) { //blue - superposition enabled
			Superpos = true;
		} else if(red == 0 && green == 3 && blue == 3) { //cyan - superpos and entanglement enabled
			Superpos = true;
			Entangle = true;
		}
	}
	
	void removePreview(){
		if(objectPreview){
			if(objectPreview.name.Contains("AngledTile")){
				objectPreview.position = new Vector3(-9999,-9999,0);
				objectPreview = null;
			} else {
				DestroyImmediate(objectPreview.gameObject);
			}
		}
		selectedItem = -1;
	}
	
	
	Vector2 labelsize = new Vector2(0, 0);
	Vector2 labelpos = new Vector2(0, 0);
	public GUIStyle textStyle;
	private string errormessage = "";
	private string mouseovertext = "";
	private bool showlevelname = false;
	private string levelname = "ENTER LEVEL NAME HERE";
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
		GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "SHIFT to show list, LEFT CLICK on object to choose", textStyle);
		labelpos.y += labelsize.y;
		GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "LEFT CLICK to place chosen object, RIGHT CLICK to delete", textStyle);
		//give user feedback
		if(errormessage != ""){
			textStyle.normal.textColor = Color.red;
			labelpos.y += labelsize.y;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), errormessage, textStyle);
			textStyle.normal.textColor = Color.white;
		}
		//let user see and change the level name
		if(showlevelname){
			labelsize.y = 30;
			textStyle.fontSize = Mathf.RoundToInt(fontsize*0.75f);
			labelpos.y = Screen.height-labelsize.y;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "", textStyle);
			levelname = GUI.TextField(new Rect(Screen.width*0.32f, labelpos.y, Screen.width*0.42f, labelsize.y), levelname, textStyle);
			labelpos.y -= labelsize.y;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), "Edit the levels name below", textStyle);
			//save the level to a file and then play
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+15, 150, labelsize.y), "Save and Play level")){
				removePreview();
				SaveLevel();
				if(valid){
					GameObject levelloader = new GameObject(levelname);
					levelloader.tag = "LevelContinue";
					DontDestroyOnLoad(levelloader);
					SceneManager.LoadScene("CustomLevels");
				}
			}
			//save the level to a file
			labelpos.y -= labelsize.y+25;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+15, 150, labelsize.y), "Save the level")){
				removePreview();
				SaveLevel();
				valid = false;
			}
			//reload level from file
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Reload level")){
				removePreview();
				foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
					if (fooObj.name.Contains("(Clone)")){
						Destroy(fooObj);
					}
				}
				LoadLevel(levelname);
			}
			//add border
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Add Border")){
				removePreview();
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
				xmin-= 6;
				xmax+= 6;
				ymin-= 2;
				ymax+= 2;
				//add blocks around previous outer level bounds
				for (int x = xmin; x <= xmax; x++){
					Transform newobject = Instantiate(levelObjects[0], new Vector3(x, ymin, 0), transform.rotation);
					newobject.localScale = new Vector3(1,1,1);
					newobject = Instantiate(levelObjects[0], new Vector3(x, ymin+1, 0), transform.rotation);
					newobject.localScale = new Vector3(1,1,1);
					newobject = Instantiate(levelObjects[0], new Vector3(x, ymax, 0), transform.rotation);
					newobject.localScale = new Vector3(1,1,1);
					newobject = Instantiate(levelObjects[0], new Vector3(x, ymax-1, 0), transform.rotation);
					newobject.localScale = new Vector3(1,1,1);
				}
				for (int y = ymin; y <= ymax; y++){
					for (int i = 0; i < 6; i++){
						Transform newobject = Instantiate(levelObjects[0], new Vector3(xmin+i, y, 0), transform.rotation);
						newobject.localScale = new Vector3(1,1,1);
						newobject = Instantiate(levelObjects[0], new Vector3(xmax-i, y, 0), transform.rotation);
						newobject.localScale = new Vector3(1,1,1);
					}
				}
			}
			//go back to menu
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Back to menu")){
				removePreview();
				SceneManager.LoadScene("MainMenu");
			}
			//center cam
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Recenter camera")){
				removePreview();
				Vector3 campos = GameObject.Find("Main Camera").transform.position;
				campos.x = 0;
				campos.y = 0;
				Camera.main.orthographicSize = 8;
				GameObject.Find("Main Camera").transform.position = campos;
			}
			//wipe level objects
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Destroy all objects")){
				removePreview();
				foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
					if (fooObj.name.Contains("(Clone)")){
						Destroy(fooObj);
					}
				}
			}
			//delete level
			labelpos.y -= labelsize.y+10;
			if (GUI.Button(new Rect(Screen.width-160, labelpos.y+10, 150, labelsize.y), "Delete level")){
				removePreview();
				if(errormessage != "Are you sure you want to delete the level?"){
					errormessage = "Are you sure you want to delete the level?";
				} else {
					foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
						if (fooObj.name.Contains("(Clone)")){
							Destroy(fooObj);
						}
					}
					SaveLevel();
					if (File.Exists(levelfilepath)) {
						File.Delete(levelfilepath);
					}
					LoadLevel(levelname);
					foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
						if (fooObj.name.Contains("(Clone)")){
							Destroy(fooObj);
						}
					}
					valid = false;
					errormessage = "Level deleted";
				}
			}
		}
		//tell user what objects they are choosing
		if(mouseovertext != ""){
			textStyle.fontSize = fontsize/2;
			labelsize.x = 250;
			labelsize.y = 30;
			labelpos.x = Input.mousePosition.x-125;
			labelpos.y = Screen.height - Input.mousePosition.y - 30;
			GUI.Label(new Rect(labelpos.x, labelpos.y, labelsize.x, labelsize.y), mouseovertext, textStyle);
		}
		//let user pick available abilities
		if(showlevelname){
			Superpos = GUI.Toggle(new Rect(10, Screen.height-(2*labelsize.y)+5, 100, 20), Superpos, "Superposition");
			Entangle = GUI.Toggle(new Rect(10, Screen.height-(2*labelsize.y)+30, 100, 20), Entangle, "Entanglement");
			Tunnel = GUI.Toggle(new Rect(115, Screen.height-(2*labelsize.y)+5, 100, 20), Tunnel, "Tunneling");
			Lightmode = GUI.Toggle(new Rect(115, Screen.height-(2*labelsize.y)+30, 100, 20), Lightmode, "Wave/Particle Duality");
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class levelcheck : MonoBehaviour {
	private int levelnum = 0;
	public bool finished = false;
	private int lastlevel;
	
	private string _imagePath = "Levels";
	private string levelgroup = "";
    public string nextLevel = "end";
    private Texture2D[] levelcodes;
	void Start(){
		//load levels from image files
		levelgroup = SceneManager.GetActiveScene().name;
		Object[] textures = Resources.LoadAll(_imagePath+"/"+levelgroup, typeof(Texture2D));
		if(textures.Length == 0){
			Debug.Log("Incorrect levelgroup filepath");
		}
        levelcodes = new Texture2D[textures.Length];
        for (int i = 0; i < textures.Length; i++){
            levelcodes[i] = (Texture2D)textures[i];
        }
		lastlevel = levelcodes.Length;
		loadlevel(levelnum);
	}
	
	public void RestartLevel(){
		loadlevel(levelnum);
	}
	public void RestartGame(){
		levelnum = 0;
		loadlevel(levelnum);
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.R)){
			RestartLevel();
		}
		if((finished && ending == false) || Input.GetKeyDown(KeyCode.P)){
			levelnum++;
			ending = true;
			StartCoroutine(Levelfinished());
		}
	}
	
	
	private bool ending = false;
	
	IEnumerator Levelfinished() {
        yield return new WaitForSeconds(1f);
		if(levelnum < lastlevel){
			loadlevel(levelnum);
		} else {
            SceneManager.LoadScene(nextLevel);
		}
    }
	
	public Transform blockprefab;
	public Transform backwallprefab;
	public Transform thinblockvprefab;
	public Transform thinblockhprefab;
	public Transform glassblockprefab;
	public Transform angledtileprefab;
	public Transform toggleblockprefab;
	public Transform leverprefab;
	public Transform buttonprefab;
	public Transform buttonwallprefab;
	public Transform wireprefab;
	private Color pixelcol;
	void loadlevel(int levelid){
		finished = false;
		ending = false;
		
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
			if (fooObj.name.Contains("(Clone)") && !fooObj.name.Contains("Music")){
				if(fooObj.name.Contains("AngledTile")){
					fooObj.transform.position = new Vector3(-999,-999,-999);
				} else {
					Destroy(fooObj);
				}
			}
		}
		
		//check every single pixel in the image
		for(int y = 0; y < levelcodes[levelid].height; y++){
			for(int x = 0; x < levelcodes[levelid].width; x++){
				spawntile(x, y, levelid);//spawn tile based on pixel colour
			}
		}
		
		//move camera to start of level and let it know the level bounds
		Vector3 playerpos = GameObject.Find("Player-char").transform.position;
		GameObject.Find("Main Camera").transform.position = new Vector3(playerpos.x, playerpos.y, -10);
		GameObject.Find("Main Camera").GetComponent<Cameracontrol>().levelheight = levelcodes[levelid].height;
		GameObject.Find("Main Camera").GetComponent<Cameracontrol>().Xlimit = levelcodes[levelid].width;
	}
	private string templatepath = "";
	public float backgroundscale = 0.25f;
	public float middlegroundscale = 0.5f;
	void getleveltheme(int levelid){
		//set up parallax
		GameObject.Find("Background").transform.localScale = new Vector3(levelcodes[levelid].width,levelcodes[levelid].height,0.1f);
		GameObject.Find("Background-moving").transform.localScale = new Vector3(levelcodes[levelid].width/backgroundscale,levelcodes[levelid].height/backgroundscale,0.1f);
	}
	void spawntile(int x, int y, int levelid){
		//get the pixels colour values
		pixelcol = levelcodes[levelid].GetPixel(x, y);
		float red = pixelcol.r;
		float green = pixelcol.g;
		float blue = pixelcol.b;
		//adjust for slight variation of decimals when reading pixels
		if(red > 0.8){red = 3;} else if(red > 0.45){red = 2;} else if(red > 0.15){red = 1;} else {red = 0;}
		if(green > 0.8){green = 3;} else if(green > 0.45){green = 2;} else if(green > 0.15){green = 1;} else {green = 0;}
		if(blue > 0.8){blue = 3;} else if(blue > 0.45){blue = 2;} else if(blue > 0.15){blue = 1;} else {blue = 0;}
		
		//define which of the 64 different shades the pixel is, shades can be seen in the "Level colour key" image
		if(red == 3 && green  == 3 && blue  == 3){ //white - standard tile
			Instantiate(blockprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 2 && green  == 2 && blue  == 2){ //light grey
		} else if(red == 1 && green  == 1 && blue  == 1){ //dark grey - back wall tile
			Instantiate(backwallprefab, new Vector3(x, y, 0.5f), transform.rotation);
		}
		//no black condition needed, black means nothing
		
		
		//primary colours
		//reds for wave duality stuff
		if(red == 3 && green == 2 && blue == 2) { //lighter red - angled surface (bottom left)
			Instantiate(angledtileprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 3 && green == 1 && blue == 1) { //light red - angled surface (bottom right)
			Instantiate(angledtileprefab, new Vector3(x, y, 0), Quaternion.Euler(0,180,0));
		} else if(red == 3 && green == 0 && blue == 0) { //red - transparent tile
			Instantiate(glassblockprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 2 && green == 1 && blue == 1) { //grey red - angled surface (top left)
			Instantiate(angledtileprefab, new Vector3(x, y, 0), Quaternion.Euler(180,0,0));
		} else if(red == 2 && green == 0 && blue == 0) { //dark red - angled surface (top right)
			Instantiate(angledtileprefab, new Vector3(x, y, 0), Quaternion.Euler(180,180,0));
		} else if(red == 1 && green == 0 && blue == 0) { //darker red
		}
		
		//greens for player stuff (spawn, checkpoints, etc)
		if(red == 2 && green == 3 && blue == 2) { //lighter green
		} else if(red == 1 && green == 3 && blue == 1) { //light green
		} else if(red == 0 && green == 3 && blue == 0) { //green - player spawn
			GameObject.Find("Player-char").transform.position = new Vector3(x,y,0);
			GameObject.Find("Player-char").GetComponent<Rigidbody>().velocity = Vector3.zero;
			GameObject.Find("Player-char").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		} else if(red == 1 && green == 2 && blue == 1) { //grey green
		} else if(red == 0 && green == 2 && blue == 0) { //dark green - level end (will replace with repair spots)
			GameObject.Find("Exit").transform.position = new Vector3(x,y,0.5f);
		} else if(red == 0 && green == 1 && blue == 0) { //darker green
		}
		
		//blues for (non ability specific) interactive elements
		if(red == 2 && green == 2 && blue == 3) { //lighter blue - lever (facing right)
			Instantiate(leverprefab, new Vector3(x, y, 0), Quaternion.Euler(0,-180,0));
		} else if(red == 1 && green == 1 && blue == 3) { //light blue - lever (facing left)
			Instantiate(leverprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 0 && green == 0 && blue == 3) { //blue - toggleable block (will replace with something else)
			Transform fooObj = Instantiate(toggleblockprefab, new Vector3(x, y, 0), transform.rotation);
			GameObject fooChild = fooObj.Find("Block-Model").gameObject;
			fooChild.GetComponent<Renderer>().enabled = true;
		} else if(red == 1 && green == 1 && blue == 2) { //grey blue - toggleable block "off" (will replace with something else)
			Transform fooObj = Instantiate(toggleblockprefab, new Vector3(x, y, 1), transform.rotation);
			GameObject fooChild = fooObj.Find("Block-Model").gameObject;
			fooChild.GetComponent<Renderer>().enabled = false;
		} else if(red == 0 && green == 0 && blue == 2) { //dark blue - pressureplate
			Instantiate(buttonprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 0 && green == 0 && blue == 1) { //darker blue - button
			Instantiate(backwallprefab, new Vector3(x, y, 0.5f), transform.rotation);
			Transform newbutton = Instantiate(buttonwallprefab, new Vector3(x, y, 0), transform.rotation);
		}
		
		
		//secondary colours
		if(red == 3 && green == 3 && blue == 2) { //lighter yellow - thin floor/ceiling
			Instantiate(thinblockhprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 3 && green == 3 && blue == 1) { //light yellow - thin wall
			Instantiate(thinblockvprefab, new Vector3(x, y, 0), transform.rotation);
		} else if(red == 3 && green == 3 && blue == 0) { //yellow - wire on solid tile
			Instantiate(blockprefab, new Vector3(x, y, 0), transform.rotation);
			Instantiate(wireprefab, new Vector3(x, y, -0.5f), transform.rotation);
		} else if(red == 2 && green == 2 && blue == 1) { //grey yellow - wire on background tile
			Instantiate(backwallprefab, new Vector3(x, y, 0.5f), transform.rotation);
			Instantiate(wireprefab, new Vector3(x, y, 0.49f), transform.rotation);
		} else if(red == 2 && green == 2 && blue == 0) { //dark yellow
		} else if(red == 1 && green == 1 && blue == 0) { //darker yellow
		}
		
		if(red == 3 && green == 2 && blue == 3) { //lighter magenta
		} else if(red == 3 && green == 1 && blue == 3) { //light magenta
		} else if(red == 3 && green == 0 && blue == 3) { //magenta
		} else if(red == 2 && green == 1 && blue == 2) { //grey magenta
		} else if(red == 2 && green == 2 && blue == 0) { //dark magenta
		} else if(red == 1 && green == 1 && blue == 0) { //darker magenta
		}
		
		if(red == 2 && green == 3 && blue == 3) { //lighter cyan
		} else if(red == 1 && green == 3 && blue == 3) { //light cyan
		} else if(red == 0 && green == 3 && blue == 3) { //cyan
		} else if(red == 1 && green == 2 && blue == 2) { //grey cyan
		} else if(red == 0 && green == 2 && blue == 2) { //dark cyan
		} else if(red == 0 && green == 1 && blue == 1) { //darker cyan
		}
		
		
		//tertiary colours
		if(red == 3 && green == 1 && blue == 2) { //light pink
		} else if(red == 3 && green == 0 && blue == 2) { //pink
		} else if(red == 3 && green == 0 && blue == 1) { //strong pink
		} else if(red == 2 && green == 0 && blue == 1) { //dark pink
		}
		
		if(red == 3 && green == 2 && blue == 1) { //light orange
		} else if(red == 3 && green == 2 && blue == 0) { //orange
		} else if(red == 3 && green == 1 && blue == 0) { //strong orange
		} else if(red == 2 && green == 1 && blue == 0) { //dark orange
		}
		
		if(red == 2 && green == 3 && blue == 1) { //light lime
		} else if(red == 2 && green == 3 && blue == 0) { //lime
		} else if(red == 1 && green == 3 && blue == 0) { //strong lime
		} else if(red == 1 && green == 2 && blue == 0) { //dark lime
		}
		
		if(red == 1 && green == 3 && blue == 2) { //light teal
		} else if(red == 0 && green == 3 && blue == 2) { //teal
		} else if(red == 0 && green == 3 && blue == 1) { //strong teal
		} else if(red == 0 && green == 2 && blue == 1) { //dark teal
		}
		
		if(red == 1 && green == 2 && blue == 3) { //light cobalt
		} else if(red == 0 && green == 2 && blue == 3) { //cobalt
		} else if(red == 0 && green == 1 && blue == 3) { //strong cobalt
		} else if(red == 0 && green == 1 && blue == 2) { //dark cobalt
		}
		
		if(red == 2 && green == 1 && blue == 3) { //light purple
		} else if(red == 2 && green == 0 && blue == 3) { //purple
		} else if(red == 1 && green == 0 && blue == 3) { //strong purple
		} else if(red == 1 && green == 0 && blue == 2) { //dark purple
		}
	}
}
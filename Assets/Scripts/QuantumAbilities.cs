using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuantumAbilities : MonoBehaviour {

	public bool SuperPosition = false;
	public bool Entanglement = false;
	public bool Tunneling = false;
	public bool WaveParticleDuality = false;
    private Transform Player;
    private Vector3 playerpos;
	public Transform Clone;
    public Transform EntangledClone;
	public Transform Lightform;
	public int maxClones = 3;
	private int cloneCount = 0;
	public bool InLightForm = false;
	private bool usingTunnel = false;
    private float WavedualityTimeout = 0;
    public float SuperPositionLifeTime = 15;
    public float EntanglementLifeTime = 10;
    private float buttonHold = 0;
	private bool inGlass = false;
	private List<Transform> SuperposClones = new List<Transform>();
	private List<Transform> EntangledClones = new List<Transform>();

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player-char").transform;
	}
	
	// Update is called once per frame
	void Update () {
		//Dev cheats
		if (Input.GetKeyDown(KeyCode.U)){
			SuperPosition = true;
			Entanglement = true;
			Tunneling = true;
			WaveParticleDuality = true;
		}
		
        playerpos = Player.position;
		
		//check if player has left glass area after light mode shouldve been disabled
		if(InLightForm && inGlass){
			CancelLightMode(false);
		}
		
		//disable other abilities while in light mode
		if(!InLightForm){
			//SuperPosition Ability is usable
			if(SuperPosition){
                if (buttonHold < 0){
                    buttonHold += Time.deltaTime;
                    if (buttonHold > 0){
                        buttonHold = 0;
                    }
                }
                if ((Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1))){
                    buttonHold += Time.deltaTime;
                } else if (buttonHold > 0){
                    //Entanglement Ability is usable
                    if (Entanglement && buttonHold > 0.6f){
                        //find all superposition clones spawned so far
                        for (int i = 0; i < SuperposClones.Count; i++){
                            //does it still exist? if so entangle it
                            if (SuperposClones[i] != null){
                                SpawnClone(1, SuperposClones[i].position.x, SuperposClones[i].position.y);
                                KillClone(i);
                            }
                        }
                    } else {//create superposition clone
						if(cloneCount < maxClones){
							SpawnClone(0, playerpos.x, playerpos.y);
						}
                    }
                    buttonHold = -0.5f;
                }
			
			}
			
			//Tunnelling Ability is usable
			if(Tunneling){
                if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2)){
					if(!usingTunnel){
						this.gameObject.layer = LayerMask.NameToLayer("Tunneling");
						Renderer rend = this.transform.GetChild(0).GetComponent<Renderer>();
						rend.material.shader = Shader.Find("Standard");
						rend.material.color = new Color(0.2f, 1, 0.1f, 1);
					}
					usingTunnel = true;
                } else {
					if(usingTunnel){
						this.gameObject.layer = LayerMask.NameToLayer("Player");
						Renderer rend = this.transform.GetChild(0).GetComponent<Renderer>();
						rend.material.shader = Shader.Find("Unlit/Transparent Cutout");
					}
					usingTunnel = false;
				}
			}
		}
		
		//Wave/Particle Duality Ability is usable
		if(WaveParticleDuality){
            if ((Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3)) && Time.timeSinceLevelLoad > WavedualityTimeout){
				WavedualityTimeout = Time.timeSinceLevelLoad + 0.25f;
				if(InLightForm){
					CancelLightMode(false);
				} else {
                    KillAllEntangledClones();
					//turn player into light form
					Lightform.gameObject.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
					Lightform.position = new Vector3(playerpos.x, playerpos.y, 0);
					InLightForm = true;
					this.transform.position = new Vector3(2,999,0);
				}
			}
		}
		
		//player 'observes' clones to despawn them
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
			KillAllClones();
		}
		
		//check how many clones there are
		cloneCount = 0;
		Cameracontrol camera = GameObject.Find("Main Camera").GetComponent<Cameracontrol>();
		int remove = Mathf.Max(0, camera.targets.Count - 1);
		camera.targets.RemoveRange(0, remove);
		foreach(GameObject fooObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()){
			if (fooObj.name.Contains("Copy")){
				cloneCount++;
				camera.targets.Add(fooObj);
			}
		}
	}
	

	public void SpawnClone(int clonetype, float X, float Y){
		if(clonetype == 0){//basic copy
			Transform newClone = Instantiate(Clone, new Vector3(X, Y, 0), transform.rotation);
            newClone.gameObject.GetComponent<CloneDespawner>().despawnTime = Time.timeSinceLevelLoad + SuperPositionLifeTime;
            newClone.gameObject.GetComponent<CloneDespawner>().spawnTime = Time.timeSinceLevelLoad;
			SuperposClones.Add(newClone);
			newClone.gameObject.GetComponent<CloneDespawner>().CloneID = SuperposClones.Count;
		} else if(clonetype == 1){//entangled copy
			Transform newClone = Instantiate(EntangledClone, new Vector3(X, Y, 0), transform.rotation);
            newClone.gameObject.GetComponent<EntangledClone>().despawnTime = Time.timeSinceLevelLoad + EntanglementLifeTime;
            newClone.gameObject.GetComponent<EntangledClone>().spawnTime = Time.timeSinceLevelLoad;
			EntangledClones.Add(newClone);
			newClone.gameObject.GetComponent<EntangledClone>().CloneID = EntangledClones.Count;
		}
	}
	

	public void KillClone(int cloneID){
		if(SuperposClones[cloneID] != null){
			Destroy(SuperposClones[cloneID].gameObject);
		}
	}

	public void KillEntangledClone(int cloneID){
		if(EntangledClones[cloneID] != null){
			Destroy(EntangledClones[cloneID].gameObject);
		}
	}

    //cancel light mode if player is not in glass, if they are wait until they leave
    public void CancelLightMode(bool levelloading){
		inGlass = false;
		if(!levelloading){
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("ground")){
				if (fooObj.name.Contains("GlassBlock")){
					float dist = Vector3.Distance(Lightform.position, fooObj.transform.position);
					if(dist < 0.75f){
						inGlass = true;
					}
				}
			}
		}
		if(!inGlass){
			this.transform.position = Lightform.position;
			this.GetComponent<Rigidbody>().velocity = Lightform.gameObject.GetComponent<Rigidbody>().velocity;
			Lightform.position = new Vector3(2, 999, 0);
			InLightForm = false;
		}
    }

	//cancel superpositions and entanglements
    public void KillAllClones(){
        for (int i = 0; i < SuperposClones.Count; i++){
            KillClone(i);
        }
        for (int i = 0; i < EntangledClones.Count; i++){
            KillEntangledClone(i);
        }

    }
    //cancel entanglements
    public void KillAllEntangledClones()
    {
        for (int i = 0; i < EntangledClones.Count; i++){
            KillEntangledClone(i);
        }

    }
}

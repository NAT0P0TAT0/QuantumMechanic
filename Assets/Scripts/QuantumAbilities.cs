﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public int maxclones = 3;
	public bool InLightForm = false;
    private float SuperpositionTimeout = 0;
    private float EntanglementTimeout = 0;
    private float WavedualityTimeout = 0;
    public float SuperPositionLifeTime = 15;
    public float EntanglementLifeTime = 10;
    private float buttonHold = 0;
	private List<Transform> SuperposClones = new List<Transform>();
	private List<Transform> EntangledClones = new List<Transform>();

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player-char").transform;
	}
	
	// Update is called once per frame
	void Update () {
        playerpos = Player.position;
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
                    if (Entanglement && buttonHold > 1f){
                        //find all superposition clones spawned so far
                        for (int i = 0; i < SuperposClones.Count; i++){
                            //does it still exist?
                            if (SuperposClones[i] != null){
                                SpawnClone(1, SuperposClones[i].position.x, SuperposClones[i].position.y);
                                KillClone(i);
                            }
                        }
                    } else {
                        SuperpositionTimeout = Time.timeSinceLevelLoad + 0.5f;
                        SpawnClone(0, playerpos.x, playerpos.y);
                    }
                    buttonHold = -0.5f;
                }
			
			}
			
			//Tunnelling Ability is usable
			if(Tunneling){
                if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2)){
                    this.gameObject.layer = LayerMask.NameToLayer("Tunneling");
                } else {
                    this.gameObject.layer = LayerMask.NameToLayer("Player");
				}
			}
		}
		
		//Wave/Particle Duality Ability is usable
		if(WaveParticleDuality){
            if ((Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3)) && Time.timeSinceLevelLoad > WavedualityTimeout){
				WavedualityTimeout = Time.timeSinceLevelLoad + 0.25f;
				if(InLightForm){
					this.transform.position = Lightform.position;
					this.GetComponent<Rigidbody>().velocity = Lightform.gameObject.GetComponent<Rigidbody>().velocity;
					Lightform.position = new Vector3(2,999,0);
					InLightForm = false;
				} else {
					//turn player into light form
					Lightform.gameObject.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
					Lightform.position = new Vector3(playerpos.x, playerpos.y, 0);
					InLightForm = true;
					this.transform.position = new Vector3(2,999,0);
					//cancel superpositions and entanglements
                    KillAllClones();
				}
			}
		}
	}
	

	public void SpawnClone(int clonetype, float X, float Y){
		if(clonetype == 0){//basic copy
			Transform newClone = Instantiate(Clone, new Vector3(X, Y, 0), transform.rotation);
			newClone.gameObject.GetComponent<CloneDespawner>().despawnTime = Time.timeSinceLevelLoad + SuperPositionLifeTime;
			SuperposClones.Add(newClone);
			newClone.gameObject.GetComponent<CloneDespawner>().CloneID = SuperposClones.Count;
		} else if(clonetype == 1){//entangled copy
			Transform newClone = Instantiate(EntangledClone, new Vector3(X, Y, 0), transform.rotation);
			newClone.gameObject.GetComponent<playercontroller>().clone = true;
			newClone.gameObject.GetComponent<playercontroller>().despawnTime = Time.timeSinceLevelLoad + EntanglementLifeTime;
			EntangledClones.Add(newClone);
			newClone.gameObject.GetComponent<playercontroller>().CloneID = EntangledClones.Count;
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

    public void KillAllClones(){
        for (int i = 0; i < SuperposClones.Count; i++){
            KillClone(i);
        }
        for (int i = 0; i < EntangledClones.Count; i++){
            KillEntangledClone(i);
        }

    }
}

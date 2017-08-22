using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumAbilities : MonoBehaviour {

	public bool SuperPosition = false;
	public bool Interference = false;
	public bool Entanglement = false;
	public bool Tunneling = false;
	public bool WaveParticleDuality = false;
    private Transform Player;
    private Vector3 playerpos;
	public Transform Clone;
    public Transform InterferingClone;
    public Transform EntangledClone;
    public Transform InterferingEntangledClone;
    public Transform TunnelingClone;
	public Transform Lightform;
	public int maxclones = 3;
	public bool InLightForm = false;
    private float SuperpositionTimeout = 0;
    private float EntanglementTimeout = 0;
    private float TunnelingTimeout = 0;
    private float WavedualityTimeout = 0;
    public float SuperPositionLifeTime = 20;
    public float EntanglementLifeTime = 10;
    public float TunnelingLifeTime = 5;
	public bool tunnelerout = false;
	private Transform CurrTunneler;
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
				if (Input.GetKey(KeyCode.S) && Time.timeSinceLevelLoad > SuperpositionTimeout) {
					SuperpositionTimeout = Time.timeSinceLevelLoad + 0.5f;
					//can copies collide with each other?
					if (Interference) {
						SpawnClone(1, playerpos.x-1, playerpos.y);
					} else {
						SpawnClone(0, playerpos.x, playerpos.y);
					}
				}
			
				//Entanglement Ability is usable
				if(Entanglement){
					if (Input.GetKey(KeyCode.W) && Time.timeSinceLevelLoad > EntanglementTimeout) {
						EntanglementTimeout = Time.timeSinceLevelLoad + 0.5f;
						//find all superposition clones spawned so far
						for(int i = 0; i < SuperposClones.Count; i++){
							//does it still exist?
							if(SuperposClones[i] != null){
								//can copies collide with each other?
								if (Interference) {
									SpawnClone(3, SuperposClones[i].position.x, SuperposClones[i].position.y);
									KillClone(i);
								} else {
									SpawnClone(2, SuperposClones[i].position.x, SuperposClones[i].position.y);
									KillClone(i);
								}
							}
						}
					}
				}
			}
			
			
			//Tunnelling Ability is usable
			if(Tunneling){
				if (Input.GetKey(KeyCode.Q) && Time.timeSinceLevelLoad > TunnelingTimeout) {
					TunnelingTimeout = Time.timeSinceLevelLoad + 0.5f;
					if(tunnelerout){
						Player.position = CurrTunneler.position;
						Player.GetComponent<Rigidbody>().velocity = CurrTunneler.GetComponent<Rigidbody>().velocity;
						Destroy(CurrTunneler.gameObject);
						tunnelerout = false;
					} else {
						SpawnClone(4, playerpos.x, playerpos.y);
						tunnelerout = true;
					}
				}
			}
		}
		
		
		//Wave/Particle Duality Ability is usable
		if(WaveParticleDuality){
            if (Input.GetKey(KeyCode.E) && Time.timeSinceLevelLoad > WavedualityTimeout) {
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
					for(int i = 0; i < SuperposClones.Count; i++){
						KillClone(i);
					}
					for(int i = 0; i < EntangledClones.Count; i++){
						KillEntangledClone(i);
					}
					if(tunnelerout){
						Destroy(CurrTunneler.gameObject);
						tunnelerout = false;
					}
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
		} else if(clonetype == 1){//collidable copy
			Transform newClone = Instantiate(InterferingClone, new Vector3(X, Y, 0), transform.rotation);
			newClone.gameObject.GetComponent<CloneDespawner>().despawnTime = Time.timeSinceLevelLoad + SuperPositionLifeTime;
			SuperposClones.Add(newClone);
			newClone.gameObject.GetComponent<CloneDespawner>().CloneID = SuperposClones.Count;
		} else if(clonetype == 2){//entangled copy
			Transform newClone = Instantiate(EntangledClone, new Vector3(X, Y, 0), transform.rotation);
			newClone.gameObject.GetComponent<playercontroller>().clone = true;
			newClone.gameObject.GetComponent<playercontroller>().despawnTime = Time.timeSinceLevelLoad + EntanglementLifeTime;
			EntangledClones.Add(newClone);
			newClone.gameObject.GetComponent<playercontroller>().CloneID = EntangledClones.Count;
		} else if(clonetype == 3){//entangled collidable copy
			Transform newClone = Instantiate(InterferingEntangledClone, new Vector3(X, Y, 0), transform.rotation);
			newClone.gameObject.GetComponent<playercontroller>().clone = true;
			newClone.gameObject.GetComponent<playercontroller>().despawnTime = Time.timeSinceLevelLoad + EntanglementLifeTime;
			EntangledClones.Add(newClone);
			newClone.gameObject.GetComponent<playercontroller>().CloneID = EntangledClones.Count;
		} else if(clonetype == 4){//tunneling preview
			CurrTunneler = Instantiate(TunnelingClone, new Vector3(X, Y, 0), transform.rotation);
			CurrTunneler.gameObject.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
			CurrTunneler.gameObject.GetComponent<CloneDespawner>().despawnTime = Time.timeSinceLevelLoad + TunnelingLifeTime;
			CurrTunneler.gameObject.GetComponent<CloneDespawner>().tunneler = true;
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
}

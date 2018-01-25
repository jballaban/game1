//Copyright (c) 2017 , Milkid - Kristin Stock 

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[Header("General Settings:")]
	public string poolType;
	public int maxInScene;

	[Header("Mode:")]
	public SPAWNMODE spawnMode;
	
	[Header("Region:")]
	public float spawnRadius;
	public float objectRadius;
	public LayerMask unwalkableLayers;

	[HideInInspector] public enum SPAWNMODE{WAVE,PERMANENT};
	[HideInInspector] public enum SPAWNSTATE{SPAWNING,WAITING};

	private float delayTimer;
	private Pooling pooling;
	private Vector3 currentSpawnpoint;
	private SPAWNSTATE spawnState;
	[HideInInspector] public List<GameObject> activeObjects = new List<GameObject>();

	private void Start(){
		activeObjects.Clear();

		pooling = gameObject.GetComponentInParent<Pooling>();

		currentSpawnpoint.y = transform.position.y;
		spawnState = SPAWNSTATE.WAITING;
	}

	private void Update(){
		//Spawns an enemy as soon as there are less then the set amount
		//of enemies then there should be in the scene.
		if(spawnMode == SPAWNMODE.PERMANENT){
			if(activeObjects.Count < maxInScene){
				if(spawnState == SPAWNSTATE.WAITING){
					GenerateSpawnpoint();
				}
			}
		//Respawns enemies in a wave as soon as all enemies are dead/gone
		} else if (spawnMode == SPAWNMODE.WAVE){
			if(activeObjects.Count == 0){
				if(spawnState == SPAWNSTATE.WAITING){
					GenerateSpawnpoint();
				}
			}
		}
	}

	//Generates random spawnpoint within a given radius;
	private void GenerateSpawnpoint(){
		currentSpawnpoint.x = Random.Range(transform.position.x - spawnRadius,transform.position.x + spawnRadius);
		currentSpawnpoint.z = Random.Range(transform.position.z - spawnRadius,transform.position.z + spawnRadius);
		CheckSpawnpoint(currentSpawnpoint);
	}

	//Checks if anything would be blocking the enemy on that specific waypoint, if
	//nothing is blocking, we'll proceed to spwan. If there is something in the way
	//a new spawnpoint will be genereated.
	private void CheckSpawnpoint(Vector3 _spawnpoint){
		if(!Physics.CheckSphere(_spawnpoint,objectRadius,unwalkableLayers)){
			SpawnEnemy(_spawnpoint);
		} else {
			GenerateSpawnpoint();
		}
	}
	//Spawns an enemy on the given spawnpoint, removes spawned enemie from the pool
	//adds enemy to a list of active objects.
	private void SpawnEnemy(Vector3 _spawnPoint){
		spawnState = SPAWNSTATE.SPAWNING;
		GameObject spawnedObject = pooling.poolsDictionairy[poolType][0];
		spawnedObject.transform.position = _spawnPoint;
		spawnedObject.SetActive(true);
		activeObjects.Add(spawnedObject);
		pooling.poolsDictionairy[poolType].Remove(spawnedObject);
		spawnState = SPAWNSTATE.WAITING;
	}

}

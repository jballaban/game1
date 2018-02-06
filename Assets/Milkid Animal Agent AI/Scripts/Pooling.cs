//Copyright (c) 2017 , itsMilkid

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool  {

	public string name;
	public GameObject pooledObject;
	public int poolSize;
}

public class Pooling : MonoBehaviour {

	public Pool[] enemyPools;
	public Dictionary<string,List<GameObject>> poolsDictionairy = new Dictionary<string,List<GameObject>>();

	private void Awake(){
		InitiateAndPopulatePools();
	}

	private void InitiateAndPopulatePools(){
		for(int i = 0; i < enemyPools.Length; i++){
			List<GameObject> newPool = new List<GameObject>();
			for(int j = 0; j < enemyPools[i].poolSize; j++){
				GameObject obj = (GameObject) Instantiate(enemyPools[i].pooledObject,new Vector3(0,0,0),Quaternion.identity);
				obj.transform.parent = transform;
				obj.SetActive(false);
				newPool.Add(obj);
			}
			poolsDictionairy.Add(enemyPools[i].name,newPool);
		}
	}	
}

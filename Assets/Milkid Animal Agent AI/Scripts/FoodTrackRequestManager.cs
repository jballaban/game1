//Copyright(c) 2017, itsMilkid

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public struct FoodTrackRequest{
	public TrackableObject requestingObject;
	public float requestRadius;
	public Action<TrackableObject,bool> callback;

	public FoodTrackRequest(TrackableObject _requestingObject,float _requestRadius,Action<TrackableObject,bool> _callback){
		requestingObject = _requestingObject;
		requestRadius = _requestRadius;
		callback = _callback;
	}
}

public struct FoodTrackResult{
	public TrackableObject target;
	public bool success;
	public Action<TrackableObject,bool> callback;

	public FoodTrackResult(TrackableObject _target,bool _success,Action<TrackableObject,bool> _callback){
		target = _target;
		success = _success;
		callback = _callback;
	}
}
public class FoodTrackRequestManager : MonoBehaviour {

	private Queue<FoodTrackResult> results = new Queue<FoodTrackResult>();

	private static FoodTrackRequestManager instance;
	private Tracking tracking;

	private void Awake(){
		instance = this;
		tracking = GetComponentInParent<Tracking>();
	}

	private void Update(){
		if(results.Count > 0){
			int itemsInQueue = results.Count;
			lock(results){
				for(int i = 0; i <itemsInQueue; i++){
					FoodTrackResult result = results.Dequeue();
					result.callback(result.target, result.success);
				}
			}
		}
	}

	public static void RequestTrackingFood(FoodTrackRequest _request){
		ThreadStart threadStart = delegate
        {
            GetTracking().FindFood(_request, instance.FinishedProcessing);
        };
		threadStart.Invoke();
	}

    private static Tracking GetTracking()
    {
        return instance.tracking;
    }

    public void FinishedProcessing(FoodTrackResult _result){
		lock(results){
			results.Enqueue(_result);
		}
	}
}

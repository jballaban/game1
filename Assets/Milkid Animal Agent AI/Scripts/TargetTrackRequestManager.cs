//Copyright(c) 2017, itsMilkid

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public struct TargetTrackRequest{
	public string requestedType;
	public TrackableObject requestingObject;
	public float requestRadius;
	public Action<TrackableObject,bool> callback;

	public TargetTrackRequest(string _requestedType,TrackableObject _requestingObject,float _requestRadius,Action<TrackableObject,bool> _callback){
		requestedType = _requestedType;
		requestingObject = _requestingObject;
		requestRadius = _requestRadius;
		callback = _callback;
	}
}

public struct TargetTrackResult{
	public TrackableObject target;
	public bool success;
	public Action<TrackableObject,bool> callback;

	public TargetTrackResult(TrackableObject _target,bool _success,Action<TrackableObject,bool> _callback){
		target = _target;
		success = _success;
		callback = _callback;
	}
}
public class TargetTrackRequestManager : MonoBehaviour {

	private Queue<TargetTrackResult> results = new Queue<TargetTrackResult>();

	private static TargetTrackRequestManager instance;
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
					TargetTrackResult result = results.Dequeue();
					result.callback(result.target, result.success);
				}
			}
		}
	}

	public static void RequestTrackingTarget(TargetTrackRequest _request){
		ThreadStart threadStart = delegate
        {
            GetTracking().FindTarget(_request, instance.FinishedProcessing);
        };
		threadStart.Invoke();
	}

    private static Tracking GetTracking()
    {
        return instance.tracking;
    }

    public void FinishedProcessing(TargetTrackResult _result){
		lock(results){
			results.Enqueue(_result);
		}
	}
}

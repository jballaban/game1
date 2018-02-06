using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObject {

    public string objectType;
    public GameObject trackedObject;
    public Vector3 objectPosition;

    public TrackableObject(string _objectType, GameObject _trackedObject, Vector3 _objectPosition)
    {
        objectType      = _objectType;
        trackedObject   = _trackedObject;
        objectPosition  = _objectPosition;
    }
}

public class Tracking : MonoBehaviour {

	public static List<TrackableObject> activeObjects = new List<TrackableObject>();
    public static List<TrackableObject> deadObjects = new List<TrackableObject>();
    
    private void Start()
    {
        activeObjects.Clear();
        deadObjects.Clear();
    }
    public void FindTarget(TargetTrackRequest _request,Action<TargetTrackResult> callback)
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            TrackableObject possibleTarget = activeObjects[i];
            if (possibleTarget.objectType == _request.requestedType)
            {
                if (possibleTarget != _request.requestingObject)
                {
                    float targetDistance = Vector3.Distance(possibleTarget.objectPosition, _request.requestingObject.objectPosition);
                    if (targetDistance <= _request.requestRadius)
                    {
                        callback(new TargetTrackResult(possibleTarget,true,_request.callback));
                        }
                        else
                        {
                        callback(new TargetTrackResult(null,false,_request.callback));
                    }
                }
            }
        }
    }

    public void FindFood(FoodTrackRequest _request,Action<FoodTrackResult>callback)
    {
        for (int i = 0; i < deadObjects.Count; i++)
        {
            TrackableObject possibleFood = deadObjects[i];
            float foodDistance = Vector3.Distance(possibleFood.objectPosition, _request.requestingObject.objectPosition);
            if (foodDistance <= _request.requestRadius)
            {
                callback(new FoodTrackResult(possibleFood,true,_request.callback));
            }
            else
            {
                callback(new FoodTrackResult(null,false,_request.callback));
            }
        }
    }
}

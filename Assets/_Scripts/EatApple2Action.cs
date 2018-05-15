using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.AI;

public class EatApple2Action : GoapAction
{
	private bool eaten = false;
	private Apple2Component targetAppleTree; // where we get the logs from

	private float startTime = 0;
	public float workDuration = 2; // seconds
	public EatApple2Action()
	{
		addEffect(Enum.GetName(typeof(Goals2), Goals2.FillHunger), true);
	}

	public override void reset()
	{
		eaten = false;
		targetAppleTree = null;
		startTime = 0;
	}

	public override bool isDone()
	{
		return eaten;
	}

	public override bool checkProceduralPrecondition(GameObject agent, BlackBoard bb)
	{
		Debug.Log("Recheck appletrees");
		// find the nearest tree that we can chop
		var trees = (List<Apple2Component>)bb.GetData("appleTree");
		var targettransform = Utility.GetNearest(agent.GetComponent<NavMeshAgent>(), trees.Where(t => t.current > 0).Select(t => t.transform).ToList());
		if (targettransform == null)
		{
			target = null;
			targetAppleTree = null;
			return false;
		}
		targetAppleTree = targettransform.GetComponentInParent<Apple2Component>();
		target = targetAppleTree.gameObject;
		return true;
	}

	public override bool perform(GameObject agent, BlackBoard bb)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > workDuration)
		{
			Debug.Log("Bite " + targetAppleTree.current);
			// finished chopping
			Brain2 brain = (Brain2)agent.GetComponent(typeof(Brain2));
			brain.Hunger += targetAppleTree.consume(50);
			eaten = true;
		}
		return true;
	}

	public override bool requiresInRange()
	{
		return true; // yes we need to be near a tree
	}
}

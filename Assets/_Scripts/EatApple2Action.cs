using UnityEngine;
using System.Collections;
using System.Linq;
using System;

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
		// find the nearest tree that we can chop
		Apple2Component[] trees = (Apple2Component[])bb.GetData("appleTree");
		if (trees.Count() == 0)
			return false;
		targetAppleTree = trees.OrderByDescending(a => (a.gameObject.transform.position - agent.transform.position).magnitude).First();
		target = targetAppleTree.gameObject;
		return true;
	}

	public override bool perform(GameObject agent, BlackBoard bb)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > workDuration)
		{
			// finished chopping
			Brain2 brain = (Brain2)agent.GetComponent(typeof(Brain2));
			brain.Hunger = Math.Min(100, brain.Hunger + 50);
			eaten = true;
		}
		return true;
	}

	public override bool requiresInRange()
	{
		return true; // yes we need to be near a tree
	}
}

using UnityEngine;

public class GetFoodAction : ActionSequence
{
	GotoAction gotoAction;

	public GetFoodAction(Brain brain) : base(brain)
	{
		gotoAction = new GotoAction(brain);
		actions.Add(gotoAction);
		actions.Add(new EatAction(brain));
	}

	public override void Activate()
	{
		gotoAction.location = getClosest("Food").transform.position;
		base.Activate();
	}

	public override float getGoalChange(IGoal goal)
	{
		return getClosest("Food") == null ? 0f : base.getGoalChange(goal);
	}

	GameObject getClosest(string tag)
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		float distance = Mathf.Infinity;
		Vector3 position = brain.transform.position;
		GameObject closest = null;
		foreach (GameObject go in gos)
		{
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
}
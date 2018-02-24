using UnityEngine;
using UnityEngine.AI;

public class GotoAction : Action
{
	NavMeshAgent _agent;
	public Vector3 location;

	public GotoAction(Brain brain) : base(brain)
	{
		_agent = brain.GetComponent<NavMeshAgent>();
	}

	public override void Activate()
	{
		_agent.SetDestination(location);
	}

	public override float getGoalChange(IGoal goal)
	{
		return 0f;
	}

	public override bool Complete()
	{
		return _agent.remainingDistance <= 1f;
	}
}
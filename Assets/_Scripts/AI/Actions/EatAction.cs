using UnityEngine;

public class EatAction : Action
{
	public EatAction(Brain brain) : base(brain) { }

	public override void Activate()
	{
		brain.hunger.current += 1f;
	}

	public override float getGoalChange(IGoal goal)
	{
		return goal is IHungerGoal ? 1f : 0f;
	}

	public override bool Complete()
	{
		return true;
	}
}
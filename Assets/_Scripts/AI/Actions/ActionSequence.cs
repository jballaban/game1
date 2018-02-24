using System.Collections.Generic;
using System.Linq;

public class ActionSequence : Action
{
	public ActionSequence(Brain brain) : base(brain) { }
	public List<Action> actions = new List<Action>();
	public override void Activate()
	{
		if (actions.Count > 0)
			actions[0].Activate();
	}

	public override float getGoalChange(IGoal goal)
	{
		return actions.Sum(a => a.getGoalChange(goal));
	}

	public override void Update()
	{
		if (actions.Count == 0) return;
		if (actions[0].Complete())
		{
			actions.RemoveAt(0);
			if (actions.Count > 0)
				actions[0].Activate();
		}
		else
			actions[0].Update();
	}

	public override bool Complete()
	{
		return actions.Count == 0;
	}
}
using System.Collections.Generic;

public class LumberjackAgent : Labourer2
{
	public override Dictionary<string, bool> createGoalState()
	{
		return Brain.NextGoal();
	}
}
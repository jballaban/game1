using System.Collections.Generic;
using UnityEngine;

public class SimpleGOB : IAIEngine
{
	public Action chooseAction(List<Action> actions, List<IGoal> goals)
	{
		if (goals.Count == 0 || actions.Count == 0) return null;
		// determine the most pressing goal to work on
		IGoal topGoal = null;
		foreach (var goal in goals)
		{
			if (topGoal == null || goal.insistence > topGoal.insistence)
				topGoal = goal;
		}
		// determine what action to take to achieve the goal
		Action bestAction = null;
		float bestUtility = 0;
		foreach (var action in actions)
		{
			var utility = action.getGoalChange(topGoal);
			if (utility > bestUtility)
			{
				bestUtility = utility;
				bestAction = action;
			}
		}
		return bestAction;
	}
}
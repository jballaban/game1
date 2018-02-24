using System.Collections.Generic;

public interface IAIEngine
{
	Action chooseAction(List<Action> actions, List<IGoal> goals);
}
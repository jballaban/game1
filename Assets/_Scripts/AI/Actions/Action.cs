public abstract class Action
{
	protected Brain brain;
	public Action(Brain brain)
	{
		this.brain = brain;
	}
	public abstract float getGoalChange(IGoal goal); // the amount that it should reduce the goal (good) or increase (bad)
	public virtual void Update() { }
	public virtual void Activate() { }
	public abstract bool Complete();
}
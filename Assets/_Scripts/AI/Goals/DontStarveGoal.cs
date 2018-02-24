using Game.Scripts.AI.Attribute;

public class DontStarveGoal : IGoal, IHungerGoal
{
	private HungerAttributeAI _hunger;
	public DontStarveGoal(HungerAttributeAI hunger)
	{
		_hunger = hunger;
	}
	public float insistence { get { return _hunger.currentPercentInv * 10; } }
	public HungerAttributeAI hunger { get { return _hunger; } }
}
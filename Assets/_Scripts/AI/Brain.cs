using System.Collections.Generic;
using System.Linq;
using Game.Scripts.AI.Attribute;
using UnityEngine;

public class Brain : MonoBehaviour
{
	public IAIEngine ai = new SimpleGOB();
	public List<IGoal> goals = new List<IGoal>();
	public List<Action> actions = new List<Action>();
	[HideInInspector]
	public HungerAttributeAI hunger;
	Action activeAction;

	void Awake()
	{
		goals.Add(new DontStarveGoal(GetComponent<HungerAttributeAI>()));
		actions.Add(new GetFoodAction(this));
		hunger = GetComponent<HungerAttributeAI>();
	}

	void Update()
	{
		var action = ai.chooseAction(actions, goals);
		if (action != null && action != activeAction)
		{
			activeAction = action;
			activeAction.Activate();
			Debug.Log(action.GetType().Name);
		}
		if (activeAction != null)
		{
			activeAction.Update();
			if (activeAction.Complete())
			{
				activeAction = null;
				Debug.Log("complete");
			}
		}
	}
}
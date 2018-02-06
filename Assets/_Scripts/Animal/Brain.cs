using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{
	public interface IAnimalAIBehaviour
	{
		void think(Brain brain);
	}

	public enum DecisionBlending
	{
		Exclusive,
		Fallback,
		Additive
	}

	public abstract class DecisionAction
	{ }

	public class DoNothingDecisionAction : DecisionAction
	{
		public static WeightedDecision instance = new WeightedDecision()
		{
			weight = 0,
			action = new DoNothingDecisionAction()
		}
	}

	public class WalkToDecisionAction : DecisionAction
	{
		public Vector3 destination;
	}

	public struct WeightedDecision
	{
		public float weight;
		public DecisionAction action;
	}

	public class Brain
	{
		public WeightedDecision active;
		public WeightedDecision next;
		public Dictionary<DecisionBlending, WeightedDecision> decisions = new Dictionary<DecisionBlending, WeightedDecision>();

		public void Reset()
		{
			decisions.Clear();
			next = DoNothingDecisionAction.instance;
		}
	}
}
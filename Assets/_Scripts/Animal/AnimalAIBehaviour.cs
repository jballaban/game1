using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{
	public abstract class AnimalAIBehaviour : MonoBehaviour
	{
		protected WeightedDecision currentWeightedDecision;
		protected Decision currentDecision;
		abstract public WeightedDecision Think(Brain brain);
		public virtual void StopDecision() { }
		public virtual void StartDecision() { }
		protected virtual void Awake()
		{
			currentWeightedDecision = new WeightedDecision()
			{
				weight = 0f,
				decision = currentDecision
			};
		}
	}
}
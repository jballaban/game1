using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{

	public class AnimalIdleBehaviour : AnimalAIBehaviour
	{
		[Tooltip("The maximum number of seconds the animal will be content being idle for")]
		public float maxContentSeconds = 10f;
		float contentment;
		const float MAX_BOREDOM_RATE = 1f;
		float boredomRate;
		protected override void Awake()
		{
			currentDecision = new IdleDecision()
			{
				behavour = this
			};
			base.Awake();
			DecisionComplete();
		}

		public override void DecisionComplete()
		{
			currentWeightedDecision.weight = maxContentSeconds;
			boredomRate = Random.Range(0f, MAX_BOREDOM_RATE);
		}

		public override WeightedDecision Think(Brain brain)
		{
			return currentWeightedDecision;
		}

		void Update()
		{
			currentWeightedDecision.weight -= boredomRate * Time.deltaTime;
		}

	}
}
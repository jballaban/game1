using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{

	public class AnimalWanderBehaviour : AnimalAIBehaviour
	{
		[Tooltip("Distance the animal will wander in a single action")]
		public float wanderDistance = 10f;
		[Tooltip("How close to get to destination before considering yourself there")]
		public float closeness = 1f;
		[Tooltip("Wandering speed")]
		public float speed = 1f;

		protected override void Awake()
		{
			currentDecision = new NavigateToDecision()
			{
				destination = Vector3.zero,
				nav = GetComponent<NavMeshAgent>(),
				closeness = 0f,
				speed = 0f,
				behavour = this
			};
			base.Awake();
		}

		public override void StopDecision()
		{
			currentDecision.Stop();
			//currentWeightedDecision.weight = 0;
		}

		public override void StartDecision()
		{
			currentDecision.Start();
		}

		public override WeightedDecision Think(Brain brain)
		{
			if (IsValid(brain))
			{
				if (currentWeightedDecision.weight == 0)
				{
					currentNavigateDecision.destination = RandomNavSphere(transform.position, wanderDistance, -1);
					currentNavigateDecision.closeness = closeness;
					currentNavigateDecision.speed = speed;
					currentWeightedDecision.weight = 2f;
				}
				return currentWeightedDecision;
			}
			else if (currentWeightedDecision.weight != 0)
				currentWeightedDecision.weight = 0;
			return null;
		}

		NavigateToDecision currentNavigateDecision { get { return currentDecision as NavigateToDecision; } }

		bool IsValid(Brain brain)
		{
			return brain.active == null
				|| brain.active.decision == currentDecision
				|| (brain.active.decision is IdleDecision);
		}

		Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
		{
			Vector3 randDirection = Random.insideUnitSphere * dist;
			randDirection += origin;
			NavMeshHit navHit;
			NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
			return navHit.position;
		}
	}
}
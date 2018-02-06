using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{
	public class AnimalWanderAI : MonoBehaviour, IAnimalAIBehaviour
	{
		[Tooltip("Distance the animal will wander in a single action")]
		public float wanderDistance;
		public void think(Brain brain)
		{
			brain.decisions.Add(DecisionBlending.Fallback, new WeightedDecision()
			{
				weight = 1f,
				action = new WalkToDecisionAction()
				{
					destination = RandomNavSphere(transform.position, wanderDistance, -1)
				}
			});
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

	[RequireComponent(typeof(HealthAttributeAI))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class AnimalAI : MonoBehaviour
	{
		HealthAttributeAI health;
		HungerAttributeAI hunger;
		NavMeshAgent nav;
		List<IAnimalAIBehaviour> behaviours;
		Brain brain = new Brain();

		public void Awake()
		{
			health = GetComponent<HealthAttributeAI>();
			hunger = GetComponent<HungerAttributeAI>();
			nav = GetComponent<NavMeshAgent>();
			behaviours = GetComponents<IAnimalAIBehaviour>().ToList();
			StartCoroutine("think");
		}

		IEnumerator think()
		{
			while (true)
			{
				brain.Reset();
				behaviours.ForEach(b => b.think(brain));
				yield return new WaitForSeconds(1);
			}

		}
	}

}
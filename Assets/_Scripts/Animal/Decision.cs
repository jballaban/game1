using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{
	public class IdleDecision : Decision
	{
		public float duration;
		public static WeightedDecision Instantiate()
		{
			return new WeightedDecision() { weight = 0f, decision = new IdleDecision() };
		}

		public override void Start()
		{
			duration = 0;
		}
		public override void Update()
		{
			duration += Time.deltaTime;
		}
	}

	public class NavigateToDecision : Decision
	{
		public Vector3 destination;
		public NavMeshAgent nav;
		public float closeness;
		public float speed;

		public override void Update()
		{
			if (nav.remainingDistance <= closeness || speed == 0)
			{
				completed = true;
			}
		}

		public override void Start()
		{
			nav.speed = speed;
			nav.isStopped = false;
			nav.SetDestination(destination);
		}

		public override void Stop()
		{
			nav.isStopped = true;
		}

		public override string ToString()
		{
			return base.ToString() + ": dest:" + destination + ", speed:" + speed;
		}
	}

	public abstract class Decision
	{
		public bool completed = false;
		public AnimalAIBehaviour behavour;
		public virtual void Update() { }
		public virtual void Stop() { }
		public virtual void Start() { }
		public override string ToString()
		{
			return this.GetType().Name;
		}
	}

	public class WeightedDecision
	{
		public float weight;
		public Decision decision;
		public override string ToString()
		{
			return "weight:" + weight + ", decision:" + decision.ToString();
		}
	}

}
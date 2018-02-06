using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Animal
{/* 
	public class AnimalHuntState : AnimalState
	{
		List<AnimalAI2> prey = new List<AnimalAI2>();
		AnimalAI2 target;
		HungerAttributeAI hunger;
		// Use this for initialization
		void Start()
		{
			hunger = GetComponent<HungerAttributeAI>();
		}

		// Update is called once per frame
		void Update()
		{
			if (target == null)
			{
				if (prey.Count == 0)
				{
					AI.changeState(typeof(AnimalIdleState));
					return;
				}
				target = prey[0];
				AI.nav.SetDestination(target.transform.position);
			}
			else if (AI.nav.remainingDistance < 1)
			{
				target = null;
				AI.changeState(typeof(AnimalIdleState));
			}
		}

		public void lookForFood()
		{
			if (prey.Count > 0)
			{
				AI.changeState(typeof(AnimalHuntState));
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Animal")
			{
				prey.Add(other.GetComponent<AnimalAI2>());
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.tag == "Animal")
			{
				var ai = other.GetComponent<AnimalAI2>();
				prey.Remove(ai);
			}
		}
	} */
}
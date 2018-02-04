using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

namespace Game.Scripts.Animal
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(AnimalIdleState))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class AnimalAI : MonoBehaviour
	{
		/* 	enum Mood
			{
				Hungry,
				Scared,
				Bored,
				Aggressive,
				Dead
			} */
		//public float chaseRecalculationSeconds;

		//public string[] foodTags;
		//	float chaseRecalculationTimer;
		[HideInInspector] public NavMeshAgent nav;
		[HideInInspector] public Animator animator;
		//	HealthAttributeAI health;
		//	HungerAttributeAI hunger;
		AnimalIdleState idle;
		AnimalState state;
		AnimalWanderState wander;
		//	Mood mood = Mood.Bored;
		//	GameObject chanseTarget;
		//	GameObject[] visibleFood = new GameObject[10]();

		void Awake()
		{
			animator = GetComponent<Animator>();
			nav = GetComponent<NavMeshAgent>();
			//	health = GetComponent<HealthAttributeAI>();
			//	hunger = GetComponent<HungerAttributeAI>();
			idle = GetComponent<AnimalIdleState>();
			wander = GetComponent<AnimalWanderState>();
		}

		void Start()
		{
			changeState(typeof(AnimalIdleState));
		}

		void Update()
		{
			animator.SetFloat("velocity", nav.velocity.magnitude);
		}

		public void changeState(Type statetype)
		{
			if (statetype.IsInstanceOfType(wander))
			{
				setState(wander);
			}
			else if (statetype.IsInstanceOfType(idle))
			{
				setState(idle);
			}
			else
			{
				Debug.Log("Unknown state " + statetype.ToString());
			}
		}

		void setState(AnimalState newstate)
		{
			if (state != null) state.enabled = false;
			state = newstate;
			Debug.Log(">" + state.enabled);
			state.enabled = true;
			Debug.Log("<" + state.enabled);
		}

		/* void Update()
		{
			if (health.getCurrent() == 0)
			{
				mood = Mood.Dead;
				health.enabled = false;
				hunger.enabled = false;
				this.enabled = false;
			}
			switch (mood)
			{
				case Mood.Bored:
					if (hunger.getCurrentPercent() <= 50)
						mood = Mood.Hungry;
					break;
				case Mood.Hungry:
					if (hunger.getCurrentPercent() >= 90)
						mood = Mood.Bored;
					else if (target == null)
						startChase(findFood(huntRadius)); 
					break;
				case Mood.Aggressive:
					//chase();
					break;
			}

} */

		/* void startChase(GameObject prey)
		{
			if (prey == null) return;
			target = prey;
			chaseRecalculationTimer = chaseRecalculationSeconds;
			mood = Mood.Aggressive;
		}

		void chase()
		{
			chaseRecalculationTimer += Time.deltaTime;
			if (chaseRecalculationTimer >= chaseRecalculationSeconds)
			{
				nav.SetDestination(target.transform.position);
				chaseRecalculationTimer -= chaseRecalculationSeconds;
			}
		} */

	}
}
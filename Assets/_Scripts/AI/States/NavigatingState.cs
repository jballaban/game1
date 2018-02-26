using System;
using UnityEngine;
using System.Collections;
using Game.Scripts.AI.Core.FSM;
using Game.Scripts.AI.Core;
using UnityEngine.AI;

namespace Game.Scripts.AI.States
{
	// generic goto state, can be used in most games, override Tick and Enter if you are using 
	//  a navmesh / pathfinding library 
	//  (ex. tell the library to search a path in Enter, when done move to the next waypoint in Tick)
	[RequireComponent(typeof(StateMachine))]
	[RequireComponent(typeof(IdleState))]
	public class NavigatingState : SmState
	{
		private Vector3? objective;
		private Transform objectiveTransform;
		private Action onDoneMovementCallback;
		private Action onFailureMovementCallback;

		private enum GoToState
		{
			Disabled, Pulsed, Active, Success, Failure
		}
		private GoToState currentState;
		private NavMeshAgent nav;
		public bool WorkInFixedUpdate;
		// when the magnitude of the difference between the objective and self is <= of this then we're done
		public float MinDistanceToObjective = 0.5f;

		protected override void Awake()
		{
			base.Awake();
			nav = GetComponent<NavMeshAgent>();
		}

		#region Work
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!WorkInFixedUpdate) return;
			Tick();
		}

		protected override void Update()
		{
			base.Update();
			if (WorkInFixedUpdate) return;
			Tick();
		}

		// if you're using an animation just override this, call base function (base.Tick()) and then 
		//  set the animator variables (if you want to use root motion then also override MoveTo)
		protected virtual void Tick()
		{
			var objectivePosition = objectiveTransform != null ? objectiveTransform.position : objective.GetValueOrDefault();
			if (nav.remainingDistance <= MinDistanceToObjective)
			{
				currentState = GoToState.Success;
			}
		}

		#endregion

		#region StateHandler
		public override void Init(StateMachine stateMachine)
		{
			base.Init(stateMachine);
			var transistion = new SmTransistion(GetPriority(), Transistion);
			var doneTransistion = new SmTransistion(GetPriority(), DoneTransistion);
			stateMachine.GetComponent<IdleState>().Transistions.Add(transistion);
			Transistions.Add(doneTransistion);
		}

		private Type DoneTransistion(ISmState state)
		{
			if (currentState != GoToState.Active)
				return typeof(IdleState);
			return null;
		}

		private Type Transistion(ISmState state)
		{
			if (currentState == GoToState.Pulsed)
				return typeof(NavigatingState);
			return null;
		}

		public void GoTo(Vector3? position, Action onDoneMovement, Action onFailureMovement)
		{
			objective = position;
			GoTo(onDoneMovement, onFailureMovement);
		}

		void GoTo(Action onDoneMovement, Action onFailureMovement)
		{
			currentState = GoToState.Pulsed;
			onDoneMovementCallback = onDoneMovement;
			onFailureMovementCallback = onFailureMovement;
		}

		public override void Enter()
		{
			base.Enter();
			if (nav.SetDestination(objective.Value))
			{
				nav.isStopped = false;
				currentState = GoToState.Active;
			}
			else
			{
				nav.isStopped = true;
				currentState = GoToState.Failure;
			}
		}

		public override void Exit()
		{
			base.Exit();
			nav.isStopped = true;
			if (currentState == GoToState.Success)
				onDoneMovementCallback();
			else
				onFailureMovementCallback();
		}
		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Scripts.AI.Core.FSM
{
	public class State : MonoBehaviour, IState
	{
		public List<IStateTransition> Transistions { get; set; }
		public int priority;

		#region UnityFunctions
		protected virtual void Awake()
		{
			Transistions = new List<IStateTransition>();
		}
		protected virtual void Start()
		{

		}
		protected virtual void FixedUpdate()
		{
		}
		protected virtual void Update()
		{
		}
		#endregion

		#region ISmState
		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
		}

		public virtual void Init(StateMachine stateMachine)
		{
		}

		public virtual bool IsActive()
		{
			return enabled;
		}

		public virtual int GetPriority()
		{
			return priority;
		}
		#endregion
	}
}
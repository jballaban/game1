using System;
using System.Collections.Generic;
namespace Game.Scripts.AI.Core.FSM
{
	public interface IState
	{
		List<IStateTransition> Transistions { get; set; }

		void Enter();
		void Exit();
		void Init(StateMachine stateMachine);
		bool IsActive();

		int GetPriority();
	}

	public interface IStateTransition
	{
		Type TransistionCheck(IState state);
		int GetPriority();
	}

	// you can inherit your FSM's transistion from this, but feel free to implement your own (note: must implement ISmTransistion and IComparable<ISmTransistion>)
	public class SmTransistion : IStateTransition, IComparable<IStateTransition>
	{
		private readonly int priority;
		private readonly Func<IState, Type> checkFunc;

		public SmTransistion(int priority, Func<IState, Type> checkFunc)
		{
			this.priority = priority;
			this.checkFunc = checkFunc;
		}

		public Type TransistionCheck(IState state)
		{
			return checkFunc(state);
		}

		public int GetPriority()
		{
			return priority;
		}

		public int CompareTo(IStateTransition other)
		{
			return -GetPriority().CompareTo(other.GetPriority());
		}
	}
}
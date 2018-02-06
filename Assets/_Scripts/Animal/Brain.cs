using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Animal
{
	public class Brain : MonoBehaviour
	{
		[Tooltip("The number of seconds to wait between reconsidering decisions")]
		public float secondsBetweenThoughts = 1f;
		WeightedDecision _active = null;
		public WeightedDecision active { get { return _active; } }
		WeightedDecision _next;
		public WeightedDecision next { get { return _next; } }
		List<WeightedDecision> decisions = new List<WeightedDecision>();
		public Memory memory = new Memory();
		List<AnimalAIBehaviour> behaviours;

		void Awake()
		{
			behaviours = GetComponents<AnimalAIBehaviour>().ToList();
		}

		void Start()
		{
			StartCoroutine("Think");
		}

		IEnumerator Think()
		{
			while (true)
			{
				Reset();
				foreach (var behavour in behaviours)
				{
					var decision = behavour.Think(this);
					if (decision != null && (_next == null || decision.weight >= _next.weight))
						_next = decision;
				}
				Decide();
				yield return new WaitForSeconds(secondsBetweenThoughts);
			}
		}

		void Update()
		{
			if (_active == null) return;
			if (_active.decision.completed)
			{
				_active.decision.behavour.StopDecision();
				_active = null;
			}
			else
				_active.decision.Update();
		}

		void Reset()
		{
			decisions.Clear();
			_next = null;
			memory.Process();
		}

		void Decide()
		{
			if (_next == _active) return; // keep current decision
			if (_active != null)
			{
				_active.decision.behavour.StopDecision();
			}
			_active = _next;
			if (_active != null)
			{
				_active.decision.completed = false;
				_active.decision.Start();
			}
			Debug.Log(_active.ToString());
		}

	}
}
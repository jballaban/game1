using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI.Attribute
{
	[RequireComponent(typeof(HealthAttributeAI))]
	public abstract class AttributeAI : MonoBehaviour
	{
		[Tooltip("The current maximum attribute value.  Can change at any time (but it will take a value change to clamp it to new max)")]
		public float currentMax;
		[Tooltip("Readonly current value")]
		public float _current = 0; // internal current value
		protected HealthAttributeAI health;
		public List<IAttributeAIBehaviour> behaviours = new List<IAttributeAIBehaviour>();
		public float current // current value
		{
			get { return _current; }
			set
			{
				if (Mathf.Clamp(value, 0, currentMax) == _current) return;
				_current = Mathf.Clamp(value, 0, currentMax);
			}
		}
		public float currentPercent { get { return _current / currentMax; } } // current value as a percent of max
		public float currentPercentInv { get { return 1 - _current / currentMax; } }
		public virtual void Awake()
		{
			health = (this is HealthAttributeAI) ? this as HealthAttributeAI : GetComponent<HealthAttributeAI>();
			behaviours.ForEach(b => b.Awake(this));
		}

		public virtual void Update()
		{
			if (!health.alive) return;
			behaviours.ForEach(b => b.Update(this));
		}
	}
}
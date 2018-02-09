using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI
{
	public interface IGrowthAttributeAI
	{
		void Grow();
	}

	public abstract class RegeneratingAttributeAI : AttributeAI
	{
		[Tooltip("max property will be overriden with a random value between 1 and maxPossibleValue.")]
		public float maxPossibleValue;
		[Tooltip("The maximum amount per second to regenerate/deplete")]
		public float regenRate;
		protected HealthAttributeAI health;

		public virtual void Awake()
		{
			if (maxPossibleValue == 0)
				Debug.LogError("Missing required maxPossibleValue");
			current = currentMax = Random.Range(1, maxPossibleValue);
			health = GetComponent<HealthAttributeAI>();
			if (health == null)
				Debug.LogError("Missing required HealthAttribute");
		}

		public virtual void Update()
		{
			current += getRegenRate();
		}

		public abstract float getRegenRate();
	}

	public abstract class AttributeAI : MonoBehaviour
	{
		[Tooltip("The current maximum attribute value.  Can change at any time (but it will take a value change to clamp it to new max)")]
		public float currentMax;
		[Tooltip("Readonly current value")]
		public float _current = 0; // internal current value
		public float current // current value
		{
			get { return _current; }
			protected set
			{
				if (Mathf.Clamp(value, 0, currentMax) == _current) return;
				_current = Mathf.Clamp(value, 0, currentMax);
			}
		}
		public float currentPercent { get { return _current / currentMax; } } // current value as a percent of max

	}
}
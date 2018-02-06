using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Animal
{
	public class AttributeAI : MonoBehaviour
	{
		[Tooltip("The maximum attribute value.  Can change at any time (but it will take a value change to clamp it to new value)")]
		public float max;
		[Tooltip("The amount of attribute to add (or subtract) per second.  Use 0 to disable regeneration.  Can change at any time.")]
		public float regenPerSecond;
		[Tooltip("Optional UI bar to update to show current value")]
		public Image bar;

		public float current // current value
		{
			get { return _current; }
			set
			{
				_current = Mathf.Clamp(value, 0, max);
				if (bar != null)
					bar.fillAmount = currentPercent;
			}
		}
		public float currentPercent { get { return _current / max; } } // current value as a percent of max
		bool regenerating; // true if object is currently regenerating
		float _current; // internal current value

		public virtual void Awake()
		{
			current = max;
		}

		public virtual void Update()
		{
			if (!regenerating && canRegen())
			{
				StartCoroutine("Regen");
			}
		}

		IEnumerator Regen()
		{
			regenerating = true;
			while (canRegen())
			{
				current += regenPerSecond;
				yield return new WaitForSeconds(1);
			}
			regenerating = false;
		}

		bool canRegen()
		{
			return regenPerSecond != 0;
		}

	}
}
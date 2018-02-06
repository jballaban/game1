using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Animal
{
	public class HungerAttributeAI : AttributeAI
	{
		[Tooltip("The amount of health to remove per second when hunger is at 0.  Set to 0 to disable starvation.  Can change at any time.")]
		public float starvationPerSecond;
		HealthAttributeAI health; // health attribute
		bool starving = false; // true if object is currently starving

		public override void Awake()
		{
			base.Awake();
			health = GetComponent<HealthAttributeAI>();
		}

		public override void Update()
		{
			base.Update();
			if (!starving && canStarve())
			{
				StartCoroutine("Starve");
			}
		}

		IEnumerator Starve()
		{
			starving = true;
			while (canStarve())
			{
				health.current -= starvationPerSecond;
				yield return new WaitForSeconds(1);
			}
			starving = false;
		}

		bool canStarve()
		{
			return starvationPerSecond != 0 && current == 0;
		}

	}
}
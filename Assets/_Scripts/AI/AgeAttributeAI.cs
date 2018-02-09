using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.AI
{
	public class AgeAttributeAI : AttributeAI
	{
		[Tooltip("How much the model will scale (additive) per day")]
		public float physicalGrowthPerDay;
		internal List<IGrowthAttributeAI> growthAttributes = new List<IGrowthAttributeAI>();
		public RandomMaxAttributeAIBehavour maxValue;
		Transform animal;

		public override void Awake()
		{
			behaviours.Add(maxValue);
			base.Awake();
			current = 0;
			animal = gameObject.transform.GetChild(0);
			StartCoroutine("Grow");
		}

		IEnumerator Grow()
		{
			while (health.alive)
			{
				current += 1 / (Environment.DAYS_PER_YEAR);
				growthAttributes.ForEach(g => g.Grow());
				if (physicalGrowthPerDay != 0)
					animal.localScale += new Vector3(physicalGrowthPerDay, physicalGrowthPerDay, physicalGrowthPerDay);
				if (current == currentMax)
					health.Die();
				yield return new WaitForSeconds(Environment.SECONDS_PER_DAY);
			}
		}

	}
}
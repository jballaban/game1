using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.AI
{
	public class AgeAttributeAI : AttributeAI
	{
		[Tooltip("max property will be overriden with a random value between 1 and maxLifeSpan (years).")]
		public float maxPossibleLifeSpan;
		[Tooltip("How much the model will scale (additive) per day")]
		public float physicalGrowthPerDay;
		public List<IGrowthAttributeAI> growthAttributes = new List<IGrowthAttributeAI>();
		HealthAttributeAI health;
		Transform animal;

		void Awake()
		{
			health = GetComponent<HealthAttributeAI>();
			if (health == null)
				Debug.LogError("Missing required HealthAttributeAI");
			if (maxPossibleLifeSpan == 0)
				Debug.LogError("Missing required maxPossibleLifeSpan value");
			animal = gameObject.transform.GetChild(0);
			currentMax = Random.Range(1, maxPossibleLifeSpan);
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
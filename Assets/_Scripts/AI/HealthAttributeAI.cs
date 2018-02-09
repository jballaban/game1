using System.Collections;
using UnityEngine;

namespace Game.Scripts.AI
{
	[RequireComponent(typeof(AgeAttributeAI))]
	public class HealthAttributeAI : AttributeAI, IGrowthAttributeAI, IDynamicRegenAttributeAI
	{
		[Tooltip("Dead or alive")]
		public bool alive = true;
		[Tooltip("The amount that the currentMax health will increase per day")]
		public float maxGrowthPerDay;
		AgeAttributeAI age;
		public RegenAttributeAIBehaviour regen;
		public RandomMaxAttributeAIBehavour maxValue;
		public UpdateUIAttributeAIBehaviour updateBar;
		public override void Awake()
		{
			behaviours.Add(regen);
			behaviours.Add(maxValue);
			if (updateBar != null)
				behaviours.Add(updateBar);
			base.Awake();
			age = GetComponent<AgeAttributeAI>();
			age.growthAttributes.Add(this);
		}

		public float getRegenRate(float rate)
		{
			return (rate / age.currentPercent);
		}

		public void Grow()
		{
			if (maxGrowthPerDay != 0)
				currentMax += maxGrowthPerDay;
		}

		public void takeDamage(float amount)
		{
			current -= amount;
			if (current == 0)
				Die();
		}

		public void Die()
		{
			current = 0;
			alive = false;
			Debug.Log("Dead");
		}

		public float getRegenRate(AttributeAI attribute)
		{
			throw new System.NotImplementedException();
		}
	}
}
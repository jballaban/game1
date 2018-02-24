using System.Collections;
using UnityEngine;

namespace Game.Scripts.AI.Attribute
{
	[RequireComponent(typeof(AgeAttributeAI))]
	public class HungerAttributeAI : AttributeAI, IGrowthAttributeAI
	{
		[Tooltip("The amount that the currentMax hunger will increase per day")]
		public float maxGrowthPerDay;
		[Tooltip("The amount of damage per second when starving.  0 to prevent starvation.")]
		public float starvationDamage;
		public RandomMaxAttributeAIBehavour initialValue;
		public RegenAttributeAIBehaviour regen;
		public UpdateUIAttributeAIBehaviour updateBar;
		AgeAttributeAI age;
		public override void Awake()
		{
			behaviours.Add(regen);
			behaviours.Add(initialValue);
			if (updateBar != null)
				behaviours.Add(updateBar);
			base.Awake();
			age = GetComponent<AgeAttributeAI>();
			age.growthAttributes.Add(this);
		}

		public override void Update()
		{
			if (!health.alive) return;
			if (current == 0 && starvationDamage != 0)
				health.takeDamage(starvationDamage * Time.deltaTime);
			base.Update();
		}

		public void Grow()
		{
			if (maxGrowthPerDay != 0)
				currentMax += maxGrowthPerDay;
		}
	}
}
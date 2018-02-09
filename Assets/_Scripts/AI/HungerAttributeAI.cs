using System.Collections;
using UnityEngine;

namespace Game.Scripts.AI
{
	public class HungerAttributeAI : RegeneratingAttributeAI, IGrowthAttributeAI
	{
		[Tooltip("The amount that the currentMax hunger will increase per day")]
		public float maxGrowthPerDay;
		AgeAttributeAI age;
		public override void Awake()
		{
			base.Awake();
			age = GetComponent<AgeAttributeAI>();
			if (age == null)
				Debug.LogError("Missing required AgeAttribute");
			age.growthAttributes.Add(this);
		}

		public override void Update()
		{
			if (!health.alive) return;
			base.Update();
		}

		public override float getRegenRate()
		{
			return (regenRate / age.currentPercent) * Time.deltaTime;
		}

		public void Grow()
		{
			if (maxGrowthPerDay != 0)
				currentMax += maxGrowthPerDay;
		}
	}
}
using System.Collections;
using UnityEngine;

namespace Game.Scripts.AI
{
	public class HealthAttributeAI : RegeneratingAttributeAI, IGrowthAttributeAI
	{

		[Tooltip("Dead or alive")]
		public bool alive = true;
		[Tooltip("The amount that the currentMax health will increase per day")]
		public float maxGrowthPerDay;
		AgeAttributeAI age;
		public override void Awake()
		{
			base.Awake();
			age = GetComponent<AgeAttributeAI>();
			if (age == null)
				Debug.LogError("Health attribute depends on AgeAttribute");
			age.growthAttributes.Add(this);
		}

		public override void Update()
		{
			if (!alive) return;
			base.Update();
			if (current == 0)
				Die();
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

		public void Die()
		{
			current = 0;
			alive = false;
			Debug.Log("Dead");
		}
	}
}
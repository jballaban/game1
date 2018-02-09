using System.Collections;
using UnityEngine;

namespace Game.Scripts.AI.Attribute
{
	[RequireComponent(typeof(AgeAttributeAI))]
	public class HealthAttributeAI : AttributeAI, IGrowthAttributeAI, IDynamicRegenAttributeAI
	{
		[Tooltip("Dead or alive")]
		public bool alive = true;
		[Tooltip("The amount that the currentMax health will increase per day")]
		public float maxGrowthPerDay;
		AgeAttributeAI age;
		Color deadColor = Color.black;
		public RegenAttributeAIBehaviour regen;
		public RandomMaxAttributeAIBehavour maxValue;
		public UpdateUIAttributeAIBehaviour updateBar;
		Transform model;
		Renderer rend;
		public override void Awake()
		{
			behaviours.Add(regen);
			behaviours.Add(maxValue);
			if (updateBar != null)
				behaviours.Add(updateBar);
			base.Awake();
			age = GetComponent<AgeAttributeAI>();
			age.growthAttributes.Add(this);
			model = transform.GetChild(0);
			rend = model.GetComponent<Renderer>();
		}

		public override void Update()
		{
			base.Update();
			if (!alive) return;
		}

		public float getRegenRate(float rate)
		{
			return rate + (rate / age.currentPercent);
		}

		public void Grow()
		{
			if (maxGrowthPerDay != 0)
				currentMax += maxGrowthPerDay;
		}

		public void takeDamage(float amount)
		{
			current -= amount;
			rend.material.color = Color.red;
			if (current == 0)
				Die();
		}

		public void Die()
		{
			current = 0;
			alive = false;
			StartCoroutine("Dieing");
			Debug.Log("Dead");
		}

		IEnumerator Dieing()
		{
			Color startColor = rend.material.color;
			float inc = 0.01f;
			Vector3 distance = Vector3.down * inc;
			for (float t = 0; t < 1; t += inc)
			{
				model.Translate(distance);
				rend.material.color = Color.Lerp(startColor, deadColor, t);
				yield return new WaitForSeconds(inc);
			}
		}
	}
}
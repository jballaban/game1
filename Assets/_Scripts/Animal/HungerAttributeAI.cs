using UnityEngine;

namespace Game.Scripts.Animal
{
	public class HungerAttributeAI : AttributeAI
	{
		public float starvationAmount;
		public float starvationDelay;
		HealthAttributeAI health;
		float starvationTimer;

		public override void Start()
		{
			base.Start();
			health = GetComponent<HealthAttributeAI>();
		}

		public override void Update()
		{
			base.Update();
			if (getCurrent() == 0)
			{
				starvationTimer += Time.deltaTime;
				if (starvationTimer >= starvationDelay)
				{
					if (health != null)
						health.getHit(starvationAmount);
					starvationTimer = 0;
				}
			}
		}
	}
}
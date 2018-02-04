using UnityEngine;

namespace Game.Scripts.Animal
{
	public class HealthAttributeAI : AttributeAI
	{

		public void getHit(float amount)
		{
			Inc(-amount);
			if (getCurrent() == 0)
			{
				Die();
			}
		}

		public void Die()
		{
			animator.SetTrigger("IsDead");
		}
	}
}
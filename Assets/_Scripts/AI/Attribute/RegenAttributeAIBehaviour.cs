using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI.Attribute
{
	[System.Serializable]
	public class RegenAttributeAIBehaviour : IAttributeAIBehaviour
	{
		[Tooltip("The maximum amount per second to regenerate/deplete")]
		public float regenRate;
		IDynamicRegenAttributeAI dynamicRate;

		public void Awake(AttributeAI attribute)
		{
			dynamicRate = attribute as IDynamicRegenAttributeAI;
		}

		public void Update(AttributeAI attribute)
		{
			attribute.current += (dynamicRate == null ? regenRate : dynamicRate.getRegenRate(regenRate)) * Time.deltaTime;
		}
	}
}
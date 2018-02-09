using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI
{

	[System.Serializable]
	public class UpdateUIAttributeAIBehaviour : IAttributeAIBehaviour
	{
		public Image bar;
		public void Awake(AttributeAI attribute)
		{
			if (bar == null)
				Debug.LogError("Missing bar");
		}

		public void Update(AttributeAI attribute)
		{
			bar.fillAmount = attribute.currentPercent;
		}
	}

	[System.Serializable]
	public class RandomMaxAttributeAIBehavour : IAttributeAIBehaviour
	{
		[Tooltip("max property will be overriden with a random value between 1 and maxPossibleValue.")]
		public float maxPossibleValue;
		public float minPossibleValue;
		public void Awake(AttributeAI attribute)
		{
			if (maxPossibleValue == 0)
				Debug.LogError("Missing required maxPossibleValue");
			attribute.current = attribute.currentMax = Random.Range(minPossibleValue, maxPossibleValue);
		}

		public void Update(AttributeAI attribute)
		{ }
	}

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

	public interface IAttributeAIBehaviour
	{
		void Awake(AttributeAI attribute);
		void Update(AttributeAI attribute);
	}
}
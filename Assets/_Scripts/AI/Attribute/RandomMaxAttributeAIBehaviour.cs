using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI.Attribute
{
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
}
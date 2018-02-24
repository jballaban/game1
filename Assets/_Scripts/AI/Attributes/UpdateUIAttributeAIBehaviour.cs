using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI.Attribute
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
}
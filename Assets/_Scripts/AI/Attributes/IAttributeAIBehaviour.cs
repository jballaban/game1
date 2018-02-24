using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.AI.Attribute
{
	public interface IAttributeAIBehaviour
	{
		void Awake(AttributeAI attribute);
		void Update(AttributeAI attribute);
	}
}
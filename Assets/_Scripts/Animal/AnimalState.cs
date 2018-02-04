using UnityEngine;

namespace Game.Scripts.Animal
{

	public abstract class AnimalState : MonoBehaviour
	{
		protected AnimalAI AI;
		void Awake()
		{
			AI = GetComponent<AnimalAI>();
			this.enabled = false;
		}


	}
}
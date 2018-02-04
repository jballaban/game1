using UnityEngine;

namespace Game.Scripts.Animal
{

	public class AnimalIdleState : AnimalState
	{
		public float maxIdle;
		float timer;
		float idleTimer;

		void OnEnable()
		{
			timer = 0;
			idleTimer = Random.Range(0f, maxIdle);
			if (maxIdle == 0)
			{
				this.enabled = false;
			}
			AI.animator.SetBool("moving", false);
			Debug.Log("Idle");
		}

		void Update()
		{
			timer += Time.deltaTime;
			if (timer >= idleTimer)
			{
				AI.changeState(typeof(AnimalWanderState));
			}
		}
	}

}
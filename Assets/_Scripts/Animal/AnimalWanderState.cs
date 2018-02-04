using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Animal
{

	public class AnimalWanderState : AnimalState
	{
		public float wanderRadius;
		void OnEnable()
		{
			Vector3 pos = RandomNavSphere(AI.nav.transform.position, wanderRadius, -1);
			AI.nav.SetDestination(pos);
			AI.animator.SetBool("moving", true);
			Debug.Log("Wander");
		}

		void Update()
		{
			if (AI.nav.remainingDistance <= AI.nav.stoppingDistance + .1)
			{
				AI.changeState(typeof(AnimalIdleState));
			}
		}

		Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
		{
			Vector3 randDirection = Random.insideUnitSphere * dist;
			randDirection += origin;
			NavMeshHit navHit;
			NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
			return navHit.position;
		}
	}

}
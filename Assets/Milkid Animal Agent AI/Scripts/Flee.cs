//Copyright(c) 2017, itsMilkid

using UnityEngine;
using UnityEngine.AI;

public class Flee : StateInterface {

	private readonly Agent agent;

    private float fleeTimer;
    private string enemyType;
    private Vector3 fleeWaypoint;

    public Flee(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        RunTimer();

        if (fleeWaypoint == Vector3.zero)
            RandomWaypoint();
        if (fleeWaypoint != Vector3.zero)
            Fleeing();

        if (agent.enableChase == true || agent.enableFlee == true)
        {
            if (agent.currentTarget == null)
                Look();
        }

        if (agent.mode == Agent.MODE.DEFENSIVE && agent.aggro == true)
            ToChase();

        if (agent.useAnimator == true)
			Animate();

        if (agent.currentHealth == 0)
            ToDead();
    }

	public void Animate()
	{
		agent.anim.SetBool("isIdle", false);
            agent.anim.SetBool("isWalking", false);
            if (agent.enableRest == true)
                agent.anim.SetBool("isResting", false);
            if (agent.enableEat == true)
                agent.anim.SetBool("isEating", false);
            if (agent.enableChase == true || agent.enableFlee == true)
                agent.anim.SetBool("isRunning", true);
            if (agent.enableAttack == true)
                agent.anim.SetBool("isAttacking", false);
            if (agent.enableDead == true)
                agent.anim.SetBool("isDead", false);
	}

    public void ToIdle()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO IDLE");

        fleeTimer = 0.0F;
        agent.currentState = agent.idle;
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO REST");

        fleeTimer = 0.0F;
        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO EAT");

        fleeTimer = 0.0F;
        agent.currentState = agent.eat;
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO WANDER");

        fleeTimer = 0.0F;
        agent.currentState = agent.wander;
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR::TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO CHASE");

        fleeTimer = 0.0F;
        agent.currentState = agent.chase;
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO ATTACK");

        fleeTimer = 0.0F;
        agent.currentState = agent.attack;
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: FLEE TO DEAD");

        fleeTimer = 0.0F;
        agent.currentState = agent.dead;
    }

   public void OnTargetFound(TrackableObject _target,bool _success)
	{
		if(_success == true){
			agent.currentTarget = _target.trackedObject;
		} else {
			agent.currentTarget = null;
		}
	}
    public void Look()
    {
        for (int i = 0; i < agent.enemies.Length; i++)
        {
            enemyType = agent.enemies[i];
             TargetTrackRequestManager.RequestTrackingTarget(new TargetTrackRequest(enemyType,agent.agent,agent.sightRange,OnTargetFound));
        }
    }

    public void RandomWaypoint()
    {
        if (agent.currentTarget != null)
        {
            fleeWaypoint.x = Random.Range(agent.currentTarget.transform.position.x - agent.fleerange, agent.currentTarget.transform.position.x + agent.fleerange);
            fleeWaypoint.y = agent.currentTarget.transform.position.y;
            fleeWaypoint.z = Random.Range(agent.currentTarget.transform.position.z - agent.fleerange, agent.currentTarget.transform.position.z + agent.fleerange);
            CheckWaypoint(fleeWaypoint);
        }
    }

    public void CheckWaypoint(Vector3 _waypoint)
    {
        fleeWaypoint = RandomNavSphere(_waypoint, 1.0F);
    }

    public Vector3 RandomNavSphere(Vector3 _waypoint, float _distance)
    {
        NavMeshHit navHit;

        Vector3 randomDirection = Random.insideUnitSphere * _distance;
        randomDirection += _waypoint;

        NavMesh.SamplePosition(_waypoint, out navHit, _distance, (1 << NavMesh.GetAreaFromName("Walkable")));
        return navHit.position;
    }

    public void Fleeing()
    {
        agent.navAgent.isStopped = false;
        agent.navAgent.speed = agent.runspeed;

        if (agent.currentTarget != null)
        {
            agent.navAgent.SetDestination(fleeWaypoint);
            agent.agent.objectPosition = agent.transform.position;

            if (agent.navAgent.remainingDistance > agent.fleerange)
            {
                agent.navAgent.isStopped = true;
                agent.currentTarget = null;
                ToIdle();
            }

        }
        else
        {
            agent.navAgent.isStopped = true;
            ToIdle();
        }
    }

    public void RunTimer()
    {
        fleeTimer += Time.deltaTime;

        if (fleeTimer < agent.fleetime)
            return;
        if (fleeTimer > agent.fleetime)
            fleeTimer = agent.fleetime;
        if (fleeTimer == agent.fleetime)
			agent.navAgent.isStopped = true;
        ToIdle();
    }
}

//Copyright(c) 2017, itsMilkid

using UnityEngine;
using UnityEngine.AI;

public class Chase : StateInterface {

	private readonly Agent agent;

    private float chaseTimer;
    private string enemyType;

    public Chase(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        Timer();
        Chasing();

        if (agent.enableChase == true || agent.enableFlee == true)
        {
            if (agent.currentTarget == null)
                Look();
        }

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
            Debug.Log("SWITCHING STATES :: CHASE TO IDLE");

        chaseTimer = 0.0F;
        agent.currentState = agent.idle;
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO REST");

        chaseTimer = 0.0F;
        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO EAT");

        chaseTimer = 0.0F;
        agent.currentState = agent.eat;
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO WANDER");

        chaseTimer = 0.0F;
        agent.currentState = agent.wander;
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO FLEE");

        chaseTimer = 0.0F;
        agent.currentState = agent.flee;
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR::TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO ATTACK");

        chaseTimer = 0.0F;
        agent.currentState = agent.attack;
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: CHASE TO DEAD");

        chaseTimer = 0.0F;
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

    public void Chasing()
    {
        agent.navAgent.isStopped = false;
        agent.navAgent.speed = agent.runspeed;

        if (agent.currentTarget != null)
        {
            agent.navAgent.SetDestination(agent.currentTarget.transform.position);
            agent.agent.objectPosition = agent.transform.position;
            if (agent.navAgent.remainingDistance > agent.attackrange)
            {
                agent.navAgent.SetDestination(agent.currentTarget.transform.position);
            }
            else
            {
                agent.navAgent.isStopped = true;
                ToAttack();
            }

            if (agent.navAgent.remainingDistance > agent.chaserange)
            {
                agent.currentTarget = null;
            }
        }
        else
        {
            agent.navAgent.isStopped = true;
            ReturnToPosition();
        }
    }

    public void ReturnToPosition()
    {
        agent.navAgent.isStopped = false;
        agent.navAgent.speed = agent.runspeed;
        agent.navAgent.SetDestination(agent.lastPositionBeforeChase);
        agent.agent.objectPosition = agent.transform.position;

        if (agent.navAgent.remainingDistance == 0)
            ToIdle();
    }

    public void Timer()
    {
        chaseTimer += Time.deltaTime;

        if (chaseTimer < agent.chasetime)
            return;
        if (chaseTimer > agent.chasetime)
            chaseTimer = agent.chasetime;
        if (chaseTimer == agent.chasetime)
            agent.navAgent.isStopped = true;
        ReturnToPosition();
    }
}

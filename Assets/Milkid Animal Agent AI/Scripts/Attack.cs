//Copyright(c) 2017, itsMilkid

using UnityEngine;

public class Attack : StateInterface {

	private readonly Agent agent;

    private float targetDistance;

    public Attack(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        LookAtTarget();
        CheckDistance();

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
                agent.anim.SetBool("isRunning", false);
            if (agent.enableAttack == true)
                agent.anim.SetBool("isAttacking", true);
            if (agent.enableDead == true)
                agent.anim.SetBool("isDead", false);
	}

    public void ToIdle()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO IDLE");

        agent.currentState = agent.idle;
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO REST");

        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO EAT");

        agent.currentState = agent.eat;
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO WANDER");

        agent.currentState = agent.wander;
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO FLEE");

        agent.currentState = agent.flee;
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO CHASE");

        agent.lastPositionBeforeChase = agent.transform.localPosition;
        agent.currentState = agent.chase;
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: ATTACK TO DEAD");

        agent.currentState = agent.dead;
    }

    public void LookAtTarget()
    {
        if (agent.currentTarget != null)
            agent.transform.LookAt(agent.currentTarget.transform.position);
    }

    public void CheckDistance()
    {
        if (agent.currentTarget != null)
        {
            targetDistance = Vector3.Distance(agent.currentTarget.transform.position, agent.agent.objectPosition);
            if (targetDistance > agent.attackrange && targetDistance < agent.chaserange)
            {
                ToChase();
            }
            else if (targetDistance > agent.chaserange)
            {
                ToIdle();
            }
        }
        else
        {
            ToIdle();
        }
    }
}

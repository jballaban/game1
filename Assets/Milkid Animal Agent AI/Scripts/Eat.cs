﻿//Copyright(c) 2017, itsMilkid

using UnityEngine;

public class Eat : StateInterface {

	private readonly Agent agent;

    private float eatTimer;
    private string enemyType;

    public Eat(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        Timer();

        if (agent.enableChase == true || agent.enableFlee == true)
        {
            if (agent.currentTarget == null)
                Look();
        }

        if (agent.useAnimator == true)
			Animate();

        if (agent.currentTarget != null && agent.mode == Agent.MODE.AGGRESSIVE)
            ToChase();

        if (agent.currentTarget != null && agent.mode == Agent.MODE.PASSIVE)
            ToFlee();

        if (agent.mode == Agent.MODE.DEFENSIVE && agent.aggro == true)
            ToChase();

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
                agent.anim.SetBool("isEating", true);
            if (agent.enableChase == true || agent.enableFlee == true)
                agent.anim.SetBool("isRunning", false);
            if (agent.enableAttack == true)
                agent.anim.SetBool("isAttacking", false);
            if (agent.enableDead == true)
                agent.anim.SetBool("isDead", false);
	}

    public void ToIdle()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO IDLE");

        eatTimer = 0.0F;
        agent.currentSatisfaction = agent.maxSatisfaction;
        agent.currentState = agent.idle;
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO REST");

        eatTimer = 0.0F;
        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO WANDER");

        eatTimer = 0.0F;
        agent.currentState = agent.wander;
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO FLEE");

        eatTimer = 0.0F;
        agent.currentState = agent.flee;
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO CHASE");

        eatTimer = 0.0F;
        agent.lastPositionBeforeChase = agent.transform.position;
        agent.currentState = agent.chase;
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: EAT TO ATTACK");

        eatTimer = 0.0F;
        agent.currentState = agent.attack;
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO DEAD");

        eatTimer = 0.0F;
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

    public void Timer()
    {
        eatTimer += Time.deltaTime;

        if (eatTimer < agent.eattime)
            return;
        if (eatTimer > agent.eattime)
            eatTimer = agent.eattime;
        if (eatTimer == agent.eattime)
            ToIdle();
    }
}
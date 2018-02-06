//Copyright(c) 2017, itsMilkid

using UnityEngine;

public class Idle : StateInterface {

	private readonly Agent agent;

    private float idleTimer;
    private string enemyType;

    public Idle(Agent _agent)
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

        if (agent.currentMood == Agent.MOOD.TIRED)
            ToRest();

        if (agent.currentHealth == 0)
            ToDead();


    }

	public void Animate()
	{
		agent.anim.SetBool("isIdle", true);
            agent.anim.SetBool("isWalking", false);
            if (agent.enableRest == true)
                agent.anim.SetBool("isResting", false);
            if (agent.enableEat == true)
                agent.anim.SetBool("isEating", false);
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
            Debug.Log("ERROR :: TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO REST");

        idleTimer = 0.0F;
        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO EAT");

        idleTimer = 0.0F;
        agent.currentState = agent.eat;
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO WANDER");

        idleTimer = 0.0F;
        agent.currentState = agent.wander;
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO FLEE");

        idleTimer = 0.0F;
        agent.currentState = agent.flee;
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO CHASE");

        idleTimer = 0.0F;
        agent.lastPositionBeforeChase = agent.transform.localPosition;
        agent.currentState = agent.chase;
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO ATTACK");

        idleTimer = 0.0F;
        agent.currentState = agent.attack;
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: IDLE TO DEAD");

        idleTimer = 0.0F;
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
        idleTimer += Time.deltaTime;

        if (idleTimer < agent.idletime)
            return;
        if (idleTimer > agent.idletime)
            idleTimer = agent.idletime;
        if (idleTimer == agent.idletime)
            ToWander();
    }
}

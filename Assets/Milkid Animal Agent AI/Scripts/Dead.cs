//Copyright(c) 2017, itsMilkid

using UnityEngine;

public class Dead : StateInterface {

	 private readonly Agent agent;

    private float decayTimer;

    public Dead(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        Timer();

        Tracking.activeObjects.Remove(agent.agent);
        Tracking.deadObjects.Add(agent.agent);

        if (agent.useAnimator == true)
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
                agent.anim.SetBool("isAttacking", false);
            if (agent.enableDead == true)
                agent.anim.SetBool("isDead", true);
        }
    }

    public void ToIdle()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR :: CAN'T SWITCH STATES WHEN DEAD!");
    }

    public void Timer()
    {
        decayTimer += Time.deltaTime;

        if (decayTimer < agent.decaytime)
            return;
        if (decayTimer > agent.decaytime)
            decayTimer = agent.decaytime;
        if (decayTimer == agent.decaytime)
            RemoveCorpse();
    }

    public void RemoveCorpse()
    {
        agent.gameObject.SetActive(false);
        Tracking.deadObjects.Remove(agent.agent);
        Pooling pool = GameObject.FindGameObjectWithTag("Pool").GetComponent<Pooling>();
        pool.poolsDictionairy[agent.agentType].Add(agent.gameObject);
    }
}

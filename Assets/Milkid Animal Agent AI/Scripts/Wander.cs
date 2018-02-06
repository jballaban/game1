//Copyright(c) 2017, itsMilkid

using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Wander : StateInterface {

	private readonly Agent agent;

    private string enemyType;

    private Vector3 waypoint;
    private int waypointIndex;
    private List<Vector3> currentWaypoints = new List<Vector3>();

    public Wander(Agent _agent)
    {
        agent = _agent;
    }

    public void UpdateState()
    {
        if (agent.enableChase == true || agent.enableFlee == true)
        {
            if (agent.currentTarget == null)
                Look();
        }

        if (agent.currentMood == Agent.MOOD.HUNGRY)
            LookForFood();

        if (currentWaypoints.Count < agent.maxWaypoints)
            RandomWaypoint();

        if (currentWaypoints.Count == agent.maxWaypoints && agent.currentMood != Agent.MOOD.HUNGRY)
            Wandering();

        if (agent.useAnimator == true)
			Animate();

        if (agent.currentTarget != null && agent.mode == Agent.MODE.AGGRESSIVE)
            ToChase();

        if (agent.currentTarget != null && agent.mode == Agent.MODE.PASSIVE)
            ToFlee();

        if (agent.mode == Agent.MODE.DEFENSIVE && agent.aggro == true)
            ToChase();
    }

	public void Animate(

	){
		agent.anim.SetBool("isIdle", false);
            agent.anim.SetBool("isWalking", true);
            if (agent.enableRest == true)
                agent.anim.SetBool("isResting", true);
            if (agent.enableEat == false)
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
            Debug.Log("SWITCHING STATES :: WANDER TO IDLE");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.currentState = agent.idle;
    }

    public void ToRest()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO REST");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.currentState = agent.rest;
    }

    public void ToEat()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO EAT");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.currentState = agent.eat;
    }

    public void ToWander()
    {
        if (agent.showLogs == true)
            Debug.Log("ERROR::TRYING TO SWITCH INTO CURRENTLY ACTIVE STATE!");
    }

    public void ToFlee()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO FLEE");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.currentState = agent.flee;
    }

    public void ToChase()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO CHASE");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.lastPositionBeforeChase = agent.transform.localPosition;
        agent.currentState = agent.chase;
    }

    public void ToAttack()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO ATTACK");

        waypointIndex = 0;
        currentWaypoints.Clear();
        agent.currentState = agent.attack;
    }

    public void ToDead()
    {
        if (agent.showLogs == true)
            Debug.Log("SWITCHING STATES :: WANDER TO DEAD");

        waypointIndex = 0;
        currentWaypoints.Clear();
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

    public void OnFoodFound(TrackableObject _target,bool _success)
	{
		if(_success == true){
			agent.foodTarget = _target;
		} else {
			agent.foodTarget = null;
		}
	}

    public void LookForFood()
    {
        if (agent.foodTarget == null)
        {
            if (agent.carnivore == true)
            {
                FoodTrackRequestManager.RequestTrackingFood(new FoodTrackRequest(agent.agent,agent.sightRange,OnFoodFound));
            }
        }
        else
        {
            WanderToFood();
        }
    }

    public void WanderToFood()
    {
        if (agent.foodTarget != null)
        {
            agent.navAgent.isStopped = false;
            agent.navAgent.speed = agent.walkspeed;

            agent.navAgent.SetDestination(agent.foodTarget.objectPosition);
            if (agent.navAgent.remainingDistance <= agent.eatrange)
            {
                agent.navAgent.isStopped = true;
                ToEat();
            }
        }
    }

    public void RandomWaypoint()
    {
        if (agent.dynamicWandering == true)
        {
            waypoint.x = Random.Range(agent.transform.position.x - agent.wanderrange, agent.transform.position.x + agent.wanderrange);
            waypoint.y = agent.transform.localPosition.y;
            waypoint.z = Random.Range(agent.transform.position.z - agent.wanderrange, agent.transform.position.z + agent.wanderrange);
            CheckWaypoint(waypoint);
        }
        else if (agent.dynamicWandering == false)
        {
            waypoint.x = Random.Range(agent.spawnpoint.x - agent.wanderrange, agent.spawnpoint.x + agent.wanderrange);
            waypoint.y = agent.spawnpoint.y;
            waypoint.z = Random.Range(agent.spawnpoint.z - agent.wanderrange, agent.spawnpoint.z + agent.wanderrange);
            CheckWaypoint(waypoint);
        }
    }

    public void CheckWaypoint(Vector3 _waypoint)
    {
        waypoint = RandomNavSphere(_waypoint, 1.0F);
        currentWaypoints.Add(waypoint);
    }

    public Vector3 RandomNavSphere(Vector3 _waypoint, float _distance)
    {
        NavMeshHit navHit;

        Vector3 randomDirection = Random.insideUnitSphere * _distance;
        randomDirection += _waypoint;

        NavMesh.SamplePosition(_waypoint, out navHit, _distance, (1 << NavMesh.GetAreaFromName("Walkable")));
        return navHit.position;
    }

    public void Wandering()
    {
        agent.navAgent.isStopped = false;
        agent.navAgent.speed = agent.walkspeed;
        agent.navAgent.SetDestination(currentWaypoints[waypointIndex]);
        agent.agent.objectPosition = agent.transform.position;

        if (agent.navAgent.remainingDistance == 0)
        {
            agent.navAgent.isStopped = true;
            waypointIndex++;
        }

        if (waypointIndex == currentWaypoints.Count)
        {
            agent.navAgent.isStopped = true;
            ToIdle();
        }
    }
}

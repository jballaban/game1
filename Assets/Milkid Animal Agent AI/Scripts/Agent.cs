//Copyright(c) 2017, itsMilkid 

using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

	#region Inspector

	[Header("AI AGENT SETTINGS:")]
	[Header("General")]
	public string agentType;
	public bool useAnimator;
	public MODE mode;

	[Header("States")]
	public bool enableRest;
    public bool enableEat;
    public bool enableChase;
    public bool enableFlee;
    public bool enableAttack;
    public bool enableDead;

	[Header("Vitality")]
    public int maxHealth;
    public int healthTickRate;
    public float healthTickTime;

    public int maxEnergy;
    public int energyTickRate;
    public float energyTickTime;

    public int maxSatisfaction;
    public int hungerTickRate;
    public float hungerTickTime;

	[Header("Food")]
    public bool carnivore;

    [Header("Natural Enemies")]
    public string[] enemies;

    [Header("Damage")]
    public int maxDamage;
    public int minDamage;
    public float aggroTime;

    [Header("Movement")]
    public float walkspeed;
    public float runspeed;

    [Header("Sight")]
    public float sightRange;

    [Header("Wander")]
    public bool dynamicWandering;
    public float wanderrange;
    public int maxWaypoints;

    [Header("Range")]
    public float chaserange;
    public float fleerange;
    public float attackrange;
    public float eatrange;

    [Header("Timing")]
    public float idletime;
    public float resttime;
    public float eattime;
    public float chasetime;
    public float fleetime;
    public float decaytime;

    [Header("Debugging")]
    public bool showLogs;

	#endregion

	[HideInInspector] public TrackableObject agent;

    [HideInInspector] public Damage[]  damageOutput;

    [HideInInspector] public StateInterface currentState;

    [HideInInspector] public Idle idle;
    [HideInInspector] public Rest rest;
    [HideInInspector] public Eat eat;
    [HideInInspector] public Wander wander;
    [HideInInspector] public Chase chase;
    [HideInInspector] public Flee flee;
    [HideInInspector] public Attack attack;
    [HideInInspector] public Dead dead;

    [HideInInspector] public MOOD currentMood;

    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator anim;

    [HideInInspector] public Tracking tracker;

    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public TrackableObject foodTarget;

    [HideInInspector] public Vector3  spawnpoint;
    [HideInInspector] public Vector3 lastPositionBeforeChase;

    [HideInInspector] public int currentEnergy;
    [HideInInspector] public int currentSatisfaction;
    [HideInInspector] public int currentHealth;

    [HideInInspector] public bool aggro;

    private float healthTimer;
    private float energyTimer;
    private float hungerTimer;
    private float aggroTimer;

    private int damage;

    public enum MODE
    {
        AGGRESSIVE,
        DEFENSIVE,
        PASSIVE
    }

    public enum MOOD
    {
        SATISFIED,
        TIRED,
        HUNGRY,
        HURT
    }

	private void Awake()
    {
        if(gameObject.activeSelf == true)
        {
            idle    = new Idle(this);
            rest    = new Rest(this);
            eat     = new Eat(this);
            wander  = new Wander(this);
            chase   = new Chase(this);
            flee    = new Flee(this);
            attack  = new Attack(this);
            dead    = new Dead(this);

            navAgent = GetComponentInParent<NavMeshAgent>();

            tracker = GameObject.FindGameObjectWithTag("Tracker").GetComponent<Tracking>();

            damageOutput = gameObject.GetComponentsInChildren<Damage>();

            if (useAnimator == true)
                anim = GetComponent<Animator>();

            if (dynamicWandering == false)
                spawnpoint = transform.position;
        }
    }

    private void Start()
    {
        if (gameObject.activeSelf == true)
        {
            agent = new TrackableObject(agentType, transform.parent.gameObject, transform.position);
            Tracking.activeObjects.Add(agent);

            for (int i = 0; i < damageOutput.Length; i++) {
                damageOutput[i].minDamage = minDamage;
                damageOutput[i].maxDamage = maxDamage;
            }

            currentHealth = maxHealth;
            currentSatisfaction = maxSatisfaction;
            currentEnergy = maxEnergy;

            healthTimer = healthTickTime;
            energyTimer = energyTickTime;
            hungerTimer = hungerTickTime;

            currentState = idle;
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf == true)
        {
            if(enableEat == true)
                CheckAndGetHungry();

            if(enableRest == true)
                CheckAndBurnEnergy();

            if(enableDead == true)
                CheckAndRegenerateHealth();

            if(aggro == true)
                AggroTimer();
            
            if(aggro == false)
                aggroTimer = aggroTime;

            currentState.UpdateState();
        }
    }

	#region Vitality Checks

    private void CheckAndRegenerateHealth()
    {
        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth < maxHealth && currentHealth > 0)
        {
            currentMood = MOOD.HURT;
            healthTimer -= Time.deltaTime;
            if (healthTimer < 0)
                healthTimer = 0;
            if (healthTimer > 0)
                return;
            if (healthTimer == 0)
                currentHealth += healthTickRate;

            healthTimer = healthTickTime;
        }
        else
        {
            currentMood = MOOD.SATISFIED;
        }
    }

    private void CheckAndGetHungry()
    {
        if (currentSatisfaction < 0)
            currentSatisfaction = 0;

        if (currentSatisfaction < maxSatisfaction)
        {
            currentMood = MOOD.HUNGRY;
        }
        else
        {
            currentMood = MOOD.SATISFIED;
        }

        hungerTimer -= Time.deltaTime;
        if (hungerTimer < 0)
            hungerTimer = 0;
        if (hungerTimer > 0)
            return;
        if (hungerTimer == 0)
            currentSatisfaction -= hungerTickRate;

        hungerTimer = hungerTickTime;
    }

    private void CheckAndBurnEnergy()
    {
        if (currentEnergy < 0)
            currentEnergy = 0;

        if (currentEnergy < maxEnergy)
        {
            currentMood = MOOD.TIRED;
        }
        else
        {
            currentMood = MOOD.SATISFIED;
        }

        energyTimer -= Time.deltaTime;
        if (energyTimer < 0)
            energyTimer = 0;
        if (energyTimer > 0)
            return;
        if (energyTimer == 0)
            currentEnergy -= energyTickRate;

        energyTimer = energyTickTime;
    }

	#endregion

	#region Damage Checks

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Damage Output")
        {
            int minInputDamage = other.gameObject.GetComponent<Damage>().minDamage;
            int maxInputDamage = other.gameObject.GetComponent<Damage>().maxDamage;
            ApplyDamage(minInputDamage, maxInputDamage);

            aggro = true;
            currentTarget = other.gameObject;
        }
    }

    private void AggroTimer(){
        aggroTimer -= Time.deltaTime;
        if (aggroTimer < 0)
            aggroTimer = 0;
        if (aggroTimer > 0)
            return;
        if (aggroTimer == 0)
            aggro = false;
    }

    private void ApplyDamage(int _minInput, int _maxInput)
    {
        damage = Random.Range(_minInput, _maxInput);
        currentHealth -= damage;
    }

	#endregion
}

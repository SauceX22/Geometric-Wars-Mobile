using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public GameObject agentSpawnParticle;
    public Transform player;
    public Transform field;
    public FlockBehavior behavior;
    public DifficultyBehavior difficultyLevel;
    public List<Transform> targets;
    [HideInInspector]
    public List<FlockAgent> agents = new List<FlockAgent>();

    //Spawning Variables
    private float batchCooldownTimer;
    private float minPlayerDistance = 7f;
    private int batchSize;
    private int batchSizeMin;
    private int batchSizeMax;
    private bool timerSet = false;

    //Keeping in field variables
    private float agentWidth;
    private float agentHeight;
    private float fieldWidth;
    private float fieldHeight;  
    private float borderOffset = .4f;

    [Range(1f, 5f)]
    public float cohesionDist = 2f;
    [Range(1f, 20f)]
    public float speed = 12f;
    //[Range(1f, 10f)]
    [HideInInspector]
    public float neighborRadius = 1.5f;
    //[Range(0f, 1.4f)]
    public float avoidanceRadiusMultiplier = 1f;

    float sqrMaxSpeed;
    float sqrNeighborRadius;
    float sqrAvoidanceRadius;
    public float SqrAvoidanceRadius { get { return sqrAvoidanceRadius; } }

    void Awake()
    {
        //Utilities
        sqrNeighborRadius = neighborRadius * neighborRadius;
        sqrAvoidanceRadius = sqrNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        timerSet = true;

        //Agent Sprite for limiting to field
        agentWidth = agentPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        agentHeight = agentPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
        fieldWidth = field.transform.localScale.x * 10 / 2; 
        fieldHeight = field.transform.localScale.z * 10 / 2;
    }

    void Update()
    {
        if (timerSet)
        {
            batchCooldownTimer = difficultyLevel.cooldownTimer;
            timerSet = false;
        }

        batchSizeMin = difficultyLevel.batchSizeMin;
        batchSizeMax = difficultyLevel.batchSizeMax;

        //Spawning Enemies Timer
        batchCooldownTimer -= Time.deltaTime;
        if (batchCooldownTimer <= 0.00f)
        {
            //Spawning Enemies
            batchSize = Random.Range(batchSizeMin, batchSizeMax);
            for (int i = 0; i < batchSize; i++)
            {
                Vector2 choosenPoint = new Vector2(Random.Range(-fieldWidth, fieldWidth), Random.Range(-fieldHeight, fieldHeight));
                Quaternion chosenRot = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
                float distanceFromPlayer = Vector2.Distance(player.position, choosenPoint);

                if (distanceFromPlayer >= minPlayerDistance)
                    StartCoroutine(SpawnEnemy(choosenPoint, chosenRot, i));
                else
                    return;
            }
            timerSet = true;
        }

        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector2 move = behavior.CalculateMove(player, agent, context, this, targets);
            move *= difficultyLevel.driveFactor;

            if (move.sqrMagnitude > sqrMaxSpeed)
            {
                move = move.normalized * difficultyLevel.maxSpeed;
            }

            agent.Move(move);
        }
    }

    IEnumerator SpawnEnemy(Vector2 choosenPoint, Quaternion chosenRot, int i)
    {
        GameObject particle = Instantiate(agentSpawnParticle, choosenPoint, chosenRot, transform);
        yield return new WaitForSeconds(4f);
        Destroy(particle);
        FlockAgent newAgent = Instantiate(agentPrefab, choosenPoint, chosenRot, transform);
        newAgent.name = "Agent " + i;
        newAgent.Initialize(this);
        agents.Add(newAgent);
    }

    private void FixedUpdate()
    {
        sqrMaxSpeed = difficultyLevel.maxSpeed * difficultyLevel.maxSpeed;
    }

    void LateUpdate()
    {
        //Clamp enemies
        for (int i = 0; i < agents.Count; i++)
        {
            Vector3 viewPos = agents[i].transform.position;
            viewPos.x = Mathf.Clamp(viewPos.x, fieldWidth * -1 + agentWidth + borderOffset, fieldWidth - agentWidth - borderOffset);
            viewPos.y = Mathf.Clamp(viewPos.y, fieldHeight * -1 + agentHeight + borderOffset, fieldHeight - agentHeight - borderOffset);
            agents[i].transform.position = viewPos;
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
         
        return context;
    }
}

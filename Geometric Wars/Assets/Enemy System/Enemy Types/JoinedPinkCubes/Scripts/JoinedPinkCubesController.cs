using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

[RequireComponent(typeof(DifficultyHandler))]
public class JoinedPinkCubesController : MonoBehaviour
{
    //Assignd Objects
    public Transform field;
    public Transform player;
    public GameObject pinkCubePrefab;
    public GameObject pinkCubeSpawnParticle;

    [HideInInspector]
    public List<JoinedPinkCube> myCubes = new List<JoinedPinkCube>();

    private int[] spawningDirections = new int[] { 90, 180, 270, 360 };
    int directionIdx;

    //Inspector stuff
    [Range(0.001f, 2)]
    public float step = 2;
    [Range(0.001f, 5)]
    public float movingTime = 1;
    [Range(0, 10)]
    public int flipTimer = 4;
    [Range(1, 10)]
    public float fieldOffset = 0.25f;
    
    //Keeping in field variables
    private float spriteWidth;
    private float spriteHeight;
    private float fieldWidth;
    private float fieldHeight;
    private float borderOffset = .4f;

    //Utilities
    private int batchSize;
    private EnemyDifficultyLevel level;
    private DifficultyHandler myDifficultyHandler;


    public void SpawnEnemies()
    {
        //Spawning Enemies
        batchSize = Random.Range(level.batchSizeMin, level.batchSizeMax);
        directionIdx = Random.Range(0, 3);
        for (int i = 0; i < batchSize; i++)
        {
            //Choosing Position and Rotation
            Vector2 choosenPoint = new Vector2(Random.Range(-(fieldWidth - fieldOffset), (fieldWidth - fieldOffset)),
                Random.Range(-(fieldHeight - fieldOffset), (fieldHeight - fieldOffset)));
            Vector3 chosenRot = Vector3.forward * spawningDirections[Random.Range(0, 3)];

            //Checking Player Position
            float distanceFromPlayer = Vector2.Distance(player.transform.position, choosenPoint);

            StartCoroutine(SpawnEnemyWithParitcle(choosenPoint, chosenRot, i));
        }
    }

    IEnumerator SpawnEnemyWithParitcle(Vector2 choosenPoint, Vector3 chosenRot, int i)
    {
        GameObject particle = Instantiate(pinkCubeSpawnParticle.gameObject, choosenPoint, Quaternion.Euler(chosenRot));
        yield return new WaitForSeconds(4f);
        Destroy(particle);
        GameObject newPinkCube = Instantiate(pinkCubePrefab, choosenPoint, Quaternion.Euler(chosenRot));
        JoinedPinkCube pinkCube = newPinkCube.GetComponent<JoinedPinkCube>();
        pinkCube.Initialize(this);
        newPinkCube.transform.name = "Pink Cube " + i;
        myCubes.Add(pinkCube);
    }
    
    private void Awake()
    {
        myDifficultyHandler = transform.GetComponent<DifficultyHandler>();
        //Agent Sprite for limiting to field
        fieldWidth = field.transform.localScale.x * 10 / 2;
        fieldHeight = field.transform.localScale.z * 10 / 2;
    }
   
    private void FixedUpdate()
    {
        level = myDifficultyHandler.handlerDifficultyLevel;
    }
}

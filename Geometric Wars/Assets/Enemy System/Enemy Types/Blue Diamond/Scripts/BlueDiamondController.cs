using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(DifficultyHandler))]
public class BlueDiamondController : MonoBehaviour
{
    //Assignd Objects
    public Transform field;
    public Transform player;
    public GameObject blueDiamondPrefab;
    public GameObject blueDiamondSpawnParticle;

    [HideInInspector]
    public List<BlueDiamond> myDiamonds = new List<BlueDiamond>();

    //Inspector stuff
    public float neighborRadius;
    public float smoothTime = 125f;

    //Public Movement Utilities
    [HideInInspector]
    public float smoothness;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float stopRadius;

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
    Vector2 velocity = Vector2.zero;


    void FixedUpdate()
    {
        for (int i = 0; i < myDiamonds.Count; i++)
        {
            Vector2 displacementFromPlayer = myDiamonds[i].gameObject.transform.position - player.position;
            Vector2 directionToPlayer = displacementFromPlayer.normalized;
            Vector2 velocity = directionToPlayer * speed;
            float distanceToPlayer = displacementFromPlayer.magnitude;

            GetNearbyNeighbors(myDiamonds[i]);

            myDiamonds[i].transform.LookAt(player.position);

            myDiamonds[i].gameObject.transform.position += speed * myDiamonds[i].transform.forward * Time.deltaTime;

            //myDiamonds[i].gameObject.transform.position = Vector2.MoveTowards(myDiamonds[i].gameObject.transform.position, player.position, speed * Time.deltaTime);

            if (distanceToPlayer <= (stopRadius + 1))
            {
                stopRadius -= Time.deltaTime;
            }
            else if (distanceToPlayer > (stopRadius + 1))
            {
                stopRadius += Time.deltaTime;
            }

            if (stopRadius > 3)
                stopRadius = 3f;
            else if (stopRadius < 0)
                stopRadius = 0f;

            //myDiamonds[i].transform.GetChild(0).transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
        level = myDifficultyHandler.handlerDifficultyLevel;
    }

    void GetNearbyNeighbors(BlueDiamond diamond)
    {
        for (int i = 0; i < myDiamonds.Count; i++)
        {
            if (myDiamonds[i] != diamond)
            {
                Vector2 displacementFromNeighbor = myDiamonds[i].gameObject.transform.position - diamond.gameObject.transform.position;
                float distanceToNeighbor = displacementFromNeighbor.magnitude;
                if (distanceToNeighbor > stopRadius)
                {
                    diamond.gameObject.transform.position = Vector2.MoveTowards(diamond.gameObject.transform.position,
                        myDiamonds[i].gameObject.transform.position, speed * Time.deltaTime);
                }
                else if (distanceToNeighbor <= stopRadius)
                {
                    diamond.transform.position = Vector2.SmoothDamp(diamond.transform.position,
                        (diamond.transform.position - myDiamonds[i].transform.position).normalized * stopRadius + myDiamonds[i].transform.position,
                        ref velocity, smoothTime * Time.deltaTime);
                }
            }
        }
    }

    public void SpawnEnemies()
    {
        //Spawning Enemies
        batchSize = Random.Range(level.batchSizeMin, level.batchSizeMax);
        for (int i = 0; i < batchSize; i++)
        {
            //Choosing Position and Rotation
            Vector2 choosenPoint = new Vector2(Random.Range(-fieldWidth, fieldWidth),
                Random.Range(-fieldHeight, fieldHeight));
            Quaternion chosenRot = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));

            //Checking Player Position
            float distanceFromPlayer = Vector2.Distance(player.transform.position, choosenPoint);

            StartCoroutine(SpawnEnemyWithParitcle(choosenPoint, chosenRot, i));
        }
    }

    IEnumerator SpawnEnemyWithParitcle(Vector2 choosenPoint, Quaternion chosenRot, int i)
    {
        GameObject particle = Instantiate(blueDiamondSpawnParticle.gameObject, choosenPoint, chosenRot);
        yield return new WaitForSeconds(4f);
        Destroy(particle);
        GameObject newDiamond = Instantiate(blueDiamondPrefab, choosenPoint, chosenRot);
        BlueDiamond diamond = newDiamond.GetComponent<BlueDiamond>();
        newDiamond.transform.name = "Diamond " + i;
        myDiamonds.Add(diamond);
        diamond.Initialize(this);
    }

    private void Awake()
    {
        myDifficultyHandler = transform.GetComponent<DifficultyHandler>();
        //Agent Sprite for limiting to field
        fieldWidth = field.transform.localScale.x * 10 / 2;
        fieldHeight = field.transform.localScale.z * 10 / 2;
        spriteWidth = blueDiamondPrefab.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        spriteHeight = blueDiamondPrefab.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    private void LateUpdate()
    {
        for (int i = 0; i < myDiamonds.Count; i++)
        {
            Vector3 viewPos = myDiamonds[i].transform.position;
            viewPos.x = Mathf.Clamp(viewPos.x, fieldWidth * -1 + spriteWidth + borderOffset, fieldWidth - spriteWidth - borderOffset);
            viewPos.y = Mathf.Clamp(viewPos.y, fieldHeight * -1 + spriteHeight + borderOffset, fieldHeight - spriteHeight - borderOffset);
            myDiamonds[i].transform.position = viewPos;
        }
    }

}


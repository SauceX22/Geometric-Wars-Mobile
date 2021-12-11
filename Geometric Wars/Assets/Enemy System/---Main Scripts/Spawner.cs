using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject player;
    public GameObject agentSpawnParticle;
    public Transform field;
    
    public float batchCooldownTimer;

    //Difficulty
    public enum difficulty
    {
        Easy,
        Normal,
        Hard
    };
    public difficulty difficultyLevel;

    [Header("Controllers ( Enemy Type )")]
    public GameObject[] enemyControllers;

    //--------------------------------------------------------------------------------------Declaring each controller type (Veriables)
    //Controller references 
    //------------------------------------------------------------Add Enemy Var Here------------------------------------------------------------
    //private BlueDiamondController blueDiamondController;

    
    //Utilities
    private GameObject enemyClone;
    private EnemyDifficultyLevel level;
    private DifficultyHandler difficultyHandler;
    private bool timerSet = false;
    private int batchSize;
    private float fieldWidth;
    private float fieldHeight;
    private float minPlayerDistance = 7f;


    private void Start()
    {
        if (enemyControllers.Length == 0)
        {
            Debug.LogError("Add an Enemy Controllers To Start.");
            return;
        }
        
        fieldWidth = field.transform.localScale.x * 10 / 2;
        fieldHeight = field.transform.localScale.z * 10 / 2;

        //----------------------------------------------( Declaring each controller type )----------------------------------------------
        
        //        foreach (GameObject controller in enemyControllers)
        //        {
        //            //------------------------------------------------------------Add Enemy Case Here------------------------------------------------------------
        //            string controllerName = controller.transform.name;
        //            switch (controllerName)
        //            {
        //                //------------------------------------------------------------Assign Each Enemy To His Var Here------------------------------------------------------------
        //                case "Blue Diamond Controller":
        //                    blueDiamondController = controller.GetComponent<BlueDiamondController>();
        //                    break;
        //
        //                default:
        //                    Debug.LogError("Unknown Controller Found!" + controllerName);
        //                    break;
        //            }
        //        }
    }

    private void Update()
    {
        // Setting controllers 
        GameObject chosenEnemyController = enemyControllers[Random.Range(0, enemyControllers.Length - 1)];
        enemyClone = chosenEnemyController;
        difficultyHandler = enemyClone.GetComponent<DifficultyHandler>();
        if (difficultyLevel == difficulty.Easy)
        {
            level = enemyClone.GetComponent<DifficultyHandler>().easy;
            UpdateEnemyDifficulty(difficultyHandler, level);
        }
        else if (difficultyLevel == difficulty.Normal)
        {
            level = enemyClone.GetComponent<DifficultyHandler>().normal;
            UpdateEnemyDifficulty(difficultyHandler, level);
        }
        else if (difficultyLevel == difficulty.Hard)
        {
            level = enemyClone.GetComponent<DifficultyHandler>().hard;
            UpdateEnemyDifficulty(difficultyHandler, level);
        }

        //Spawning Enemies on Timer
        if (timerSet)
        {
            batchCooldownTimer = level.cooldownTimer;
            timerSet = false;
        }
        SpawnEnemy(difficultyHandler, enemyClone);
    }

    void SpawnEnemy(DifficultyHandler chosenDifficultyHandler, GameObject chosenEnemyController)
    {
        batchCooldownTimer -= Time.deltaTime;
        if (batchCooldownTimer <= 0.00f)
        {
            //------------------------------------------------------------( Add Enemy Case Here )------------------------------------------------------------
            switch (chosenDifficultyHandler.thisEnemyType)
            {
                case DifficultyHandler.enemyType.BlueDiamond: BlueDiamondController blueDiamondController = chosenEnemyController.GetComponent<BlueDiamondController>();
                    blueDiamondController.SpawnEnemies();
                    break;
                case DifficultyHandler.enemyType.JoindPinkCubes: JoinedPinkCubesController joinedPinkCubesController = chosenEnemyController.GetComponent<JoinedPinkCubesController>();
                    joinedPinkCubesController.SpawnEnemies();
                    break;

                default: Debug.LogError("Unknown Enemy Type Found!" + chosenDifficultyHandler.gameObject.transform.name);
                    break;
            }
            timerSet = true;
        }
    }

    void UpdateEnemyDifficulty(DifficultyHandler difficultyHandler, EnemyDifficultyLevel chosenDifficulty)
    {
        if (difficultyHandler != null)
        {
            difficultyHandler.handlerDifficultyLevel = chosenDifficulty;
        }
    }
}

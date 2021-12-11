using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    [HideInInspector]
    public EnemyDifficultyLevel handlerDifficultyLevel;

    public enum enemyType
    {
        BlueDiamond,
        JoindPinkCubes
    };
    public enemyType thisEnemyType;

    public EnemyDifficultyLevel easy;
    public EnemyDifficultyLevel normal;
    public EnemyDifficultyLevel hard;
}

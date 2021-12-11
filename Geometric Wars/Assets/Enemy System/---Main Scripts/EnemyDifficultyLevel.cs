using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Difficulity Level")]
public class EnemyDifficultyLevel : ScriptableObject
{
    [Header("Enemy Variables")]

    [Range(1, 20)]
    public int speed = 7;
    [Range(1f, 5f)]
    public float smoothness = 3f;
    [Range(1f, 10f)]
    public float stopRadius = 1.5f;
    [Range(2, 20)]
    public int driveFactor = 10;
    //[Range(5, 20)]
    //public int maxSpeed = 13;

    [Header("Wave Variables")]
    [Range(0, 20)]
    public int cooldownTimer = 10;
    [Range(0, 20)]
    public int batchSizeMin = 6;
    [Range(3, 20)]
    public int batchSizeMax = 10;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Difficulty Level")]
public class DifficultyBehavior : ScriptableObject
{
    [Range(0, 20)]
    public float cooldownTimer = 10f;
    [Range(0, 10)]
    public int batchSizeMin = 6;
    [Range(3, 20)]
    public int batchSizeMax = 10;
    [Range(2, 20)]
    public float driveFactor = 10f;
    [Range(5, 20)]
    public float maxSpeed = 13f;
}

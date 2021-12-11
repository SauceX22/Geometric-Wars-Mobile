using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Boreder Bounce")]
public class BorderBouncingBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        Vector2 BounceMove = Vector2.zero;
        return BounceMove;
    }
}

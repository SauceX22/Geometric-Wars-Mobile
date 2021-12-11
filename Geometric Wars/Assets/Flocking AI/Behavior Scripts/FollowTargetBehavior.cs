using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Follow Target")]
public class FollowTargetBehavior : FlockBehavior
{
    Vector2 curretVelocity;
    [Range(0.01f, 0.3f)]
    public float agentSmoothTime = 1f;

    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        //If no target, return no adjusments
        if (targets.Count == 0)
            return Vector2.zero;

        //Otherwise, check how many there are
        Vector2 followMove = Vector2.zero;

        if (targets.Count == 1)
            followMove = (Vector2)targets[0].position;
        else if (targets.Count > 1)
        {
            followMove = (Vector2)targets[Random.Range(0, targets.Count - 1)].position;
        }

        //Create offset from agent position
        followMove -= (Vector2)agent.transform.position;
        followMove = Vector2.SmoothDamp(agent.transform.up, followMove, ref curretVelocity, agentSmoothTime);

        return followMove;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        //If no neighbors, maintain current alignmet
        if (context.Count == 0)
            return agent.transform.up;

        //Otherwise, Add all points then average
        Vector2 AvoidanceMove = Vector2.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SqrAvoidanceRadius)
            {
                nAvoid++;
                AvoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }
        if (nAvoid > 0)
            AvoidanceMove /= nAvoid;

        return AvoidanceMove;
    }
}

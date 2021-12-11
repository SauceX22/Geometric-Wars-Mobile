using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior
{
    Vector2 curretVelocity;
    [Range(0.01f, 0.3f)]
    public float agentSmoothTime = 0.1f;

    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        //If no neighbors, maintain current alignmet
        if (context.Count == 0)
            return agent.transform.up;

        //Otherwise, Add all points then average
        Vector2 alignmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.transform.up;
        }
        alignmentMove /= context.Count;

        alignmentMove = Vector2.SmoothDamp(agent.transform.up, alignmentMove, ref curretVelocity, agentSmoothTime);

        return alignmentMove;   
    }
}

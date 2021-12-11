using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{
    Vector2 curretVelocity;
    [Range(0.01f, 0.3f)]
    public float agentSmoothTime = 0.1f;

    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        //If no neighbors, return no adjusments
        if (context.Count == 0)
            return Vector2.zero;

        //Otherwise, Add all points then average
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //Create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref curretVelocity, agentSmoothTime);

        return cohesionMove;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Get Close")]
public class GetCloseBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(Transform player, FlockAgent agent, List<Transform> context, Flock flock, List<Transform> targets)
    {
        //If no neighbors, return no adjusments
        if (context.Count == 0)
            return Vector2.zero;

        //Otherwise, Add all points then average
        Vector2 displacement = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            displacement = (Vector2)item.transform.position - (Vector2)agent.transform.position;
        }

        if (displacement.magnitude > flock.cohesionDist)
            return displacement;
        else
            return Vector2.zero;
    }
}

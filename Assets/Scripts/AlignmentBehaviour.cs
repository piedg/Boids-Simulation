using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class AlignmentBehaviour : SteeringBehaviour
{
    public float maxAcceleration = 5f;
    
    public override SteeringOutput GetSteering(Agent agent)
    {
        var settings = agent.Settings;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.Position.xy, settings.AlignmentRadius, settings.BoidLayer);
        List<Agent> neighbors = new List<Agent>();
        
        foreach (var col in colliders)
        {
            Agent neighbor = col.GetComponent<Agent>();
            if (neighbor != agent)
            {
                neighbors.Add(neighbor);
            }
        }

        if (neighbors.Count == 0)
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };

        float3 avgVel = float3.zero;
        foreach (var b in neighbors)
            avgVel += b.LinearVelocity;
        avgVel /= neighbors.Count;

        avgVel.z = 0;
        float3 desired = math.normalizesafe(avgVel) * maxAcceleration;

        return new SteeringOutput
        {
            Linear  = desired,
            Angular = 0f
        };
    }
}
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class SeparationBehaviour : SteeringBehaviour
{
    public float maxAcceleration = 10f;

    public override SteeringOutput GetSteering(Agent agent)
    {
        var settings = agent.Settings;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.Position.xy, settings.SeparationRadius, settings.BoidLayer);

        var neighbors = colliders
            .Select(collider => collider.GetComponent<Agent>())
            .Where(neighborAgent => neighborAgent != agent)
            .ToArray();

        if (neighbors.Length == 0)
        {
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };
        }

        float3 repulse = float3.zero;
        
        foreach (var neighbor in neighbors)
        {
            float3 offset = agent.Position - neighbor.Position;
            offset.z = 0;
            repulse += math.normalize(offset) / math.length(offset);
        }

        repulse = math.normalizesafe(repulse) * maxAcceleration;

        return new SteeringOutput
        {
            Linear = repulse,
            Angular = 0f
        };
    }
}
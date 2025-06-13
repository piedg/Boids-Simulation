using System.Linq;
using System;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class SeparationBehaviour : SteeringBehaviour
{
    public float maxAcceleration = 10f;

    public override SteeringOutput GetSteering(Agent agent)
    {
        var settings = agent.Settings;
        var neighbors = GameObject.FindObjectsOfType<Agent>()
            .Where(b => b != agent && math.distance(agent.Position.xy, b.Position.xy) < settings.SeparationRadius)
            .ToArray();

        if (neighbors.Length == 0)
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };

        float3 repulse = float3.zero;
        foreach (var b in neighbors)
        {
            float3 offset = agent.Position - b.Position;
            offset.z = 0;
            repulse += math.normalize(offset) / math.length(offset);
        }

        repulse = math.normalizesafe(repulse) * maxAcceleration;

        return new SteeringOutput
        {
            Linear  = repulse,
            Angular = 0f
        };
    }
}
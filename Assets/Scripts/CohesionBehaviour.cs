using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class CohesionBehaviour : SteeringBehaviour
{
    public float maxAcceleration = 5f;

    public override SteeringOutput GetSteering(Agent agent)
    {
        var settings = agent.Settings;
        var neighbors = GameObject.FindObjectsOfType<Agent>()
            .Where(b => b != agent && math.distance(agent.Position.xy, b.Position.xy) < settings.CohesionRadius)
            .ToArray();

        if (neighbors.Length == 0)
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };

        float3 center = float3.zero;
        foreach (var b in neighbors) center += b.Position;
        center /= neighbors.Length;

        float3 dir = center - agent.Position;
        dir.z = 0;
        dir = math.normalizesafe(dir);

        return new SteeringOutput
        {
            Linear  = dir * maxAcceleration,
            Angular = 0f
        };
    }
}
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class AlignmentBehaviour : SteeringBehaviour
{
    public float maxAcceleration = 5f;

    public override SteeringOutput GetSteering(Agent agent)
    {
        var settings = agent.Settings;

        var neighbors = GameObject.FindObjectsOfType<Agent>()
            .Where(b => b != agent && math.distance(agent.Position.xy, b.Position.xy) < settings.AlignmentRadius)
            .ToArray();

        if (neighbors.Length == 0)
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };

        float3 avgVel = float3.zero;
        foreach (var b in neighbors)
            avgVel += b.LinearVelocity;
        avgVel /= neighbors.Length;

        avgVel.z = 0;
        float3 desired = math.normalizesafe(avgVel) * maxAcceleration;

        return new SteeringOutput
        {
            Linear  = desired,
            Angular = 0f
        };
    }
}
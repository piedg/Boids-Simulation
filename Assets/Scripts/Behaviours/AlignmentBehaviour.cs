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
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.Position.xy, settings.AlignmentRadius, settings.BoidLayer);

        var neighbors = colliders
            .Select(collider => collider.GetComponent<Agent>())
            .Where(neighborAgent => neighborAgent != agent)
            .ToArray();

        if (neighbors.Length == 0)
        {
            return new SteeringOutput
            {
                Linear = float3.zero, 
                Angular = 0f
            }; 
        }

        float3 averageVelocity = float3.zero;
        foreach (var neighbor in neighbors)
        {
            averageVelocity += neighbor.LinearVelocity;
        }
        averageVelocity /= neighbors.Length;

        averageVelocity.z = 0;
        float3 desiredVelocity = math.normalizesafe(averageVelocity) * maxAcceleration;

        return new SteeringOutput
        {
            Linear  = desiredVelocity,
            Angular = 0f
        };
    }
}
using Unity.Mathematics;
using UnityEngine;

public class LookWhereYouMoveBehaviour : SteeringBehaviour
{
    public override SteeringOutput GetSteering(Agent agent)
    {
        if (math.length(agent.LinearVelocity.xy) < 0.01f)
            return new SteeringOutput { Linear = float3.zero, Angular = 0f };

        float targetOrientation = math.atan2(agent.LinearVelocity.y, agent.LinearVelocity.x);

        float delta = MapToRange(targetOrientation - agent.Orientation);

        float targetAngularVelocity = delta * agent.MaxAngularSpeed;

        float angularAcceleration = (targetAngularVelocity - agent.AngularVelocity) / Time.fixedDeltaTime;

        angularAcceleration = math.clamp(angularAcceleration, -agent.MaxAngularSpeed, agent.MaxAngularSpeed);

        return new SteeringOutput
        {
            Linear = float3.zero,
            Angular = angularAcceleration
        };
    }

    private float MapToRange(float angle)
    {
        const float pi = math.PI;
        const float twoPi = 2f * pi;

        while (angle > pi)  angle -= twoPi;
        while (angle < -pi) angle += twoPi;
        return angle;
    }
}
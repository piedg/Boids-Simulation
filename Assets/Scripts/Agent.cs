using System;
using Unity.Mathematics;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private BoidSettings settings;
    public BoidSettings Settings => settings;
    [SerializeField] private float maxLinearSpeed = 5f;
    [SerializeField] private float maxAngularSpeed = 180f;
    [SerializeReference, SubclassSelector] private SteeringBehaviour[] steerings = Array.Empty<SteeringBehaviour>();

    private float3 _velocity;
    private float _angularVelocity;

    public float MaxLinearSpeed => maxLinearSpeed;
    public float MaxAngularSpeed => math.radians(maxAngularSpeed);

    public float3 LinearVelocity => _velocity;
    public float AngularVelocity => _angularVelocity;

    public float3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }

    public float Orientation
    {
        get => math.radians(transform.eulerAngles.z);
        private set => transform.rotation = quaternion.Euler(0, 0, value);
    }

    private void FixedUpdate()
    {
        float3 linearAcceleration = float3.zero;
        float angularAcceleration = 0f;
        float dt = Time.fixedDeltaTime;

        foreach (var steeringBehaviour in steerings)
        {
            var steeringOutput = steeringBehaviour.GetSteering(this);
            linearAcceleration += steeringOutput.Linear;
            angularAcceleration += steeringOutput.Angular;
        }

        _velocity += linearAcceleration * dt;
        _angularVelocity += angularAcceleration * dt;

        _velocity = math.normalizesafe(_velocity) * math.min(math.length(_velocity), MaxLinearSpeed);
        _angularVelocity = math.clamp(_angularVelocity, -MaxAngularSpeed, MaxAngularSpeed);

        Position += _velocity * dt;
        Orientation += _angularVelocity * dt;

        while (Position.x > 10)
        {
            Position = new float3(Position.x - 20, Position.y, 0);
        }

        while (Position.x < -10)
        {
            Position = new float3(Position.x + 20, Position.y, 0);
        }

        while (Position.y > 5)
        {
            Position = new float3(Position.x, Position.y - 10, 0);
        }

        while (Position.y < -5)
        {
            Position = new float3(Position.x, Position.y + 10, 0);
        }

        var targetOrientation = math.atan2(LinearVelocity.y, LinearVelocity.x);
        Vector3 dir = new Vector3(math.cos(targetOrientation), math.sin(targetOrientation), 0);
        Debug.DrawRay(Position, dir, Color.green);

        if (math.length(LinearVelocity.x) > 0.01f)
        {
            float angleDeg = Mathf.Atan2(LinearVelocity.y, LinearVelocity.x) * Mathf.Rad2Deg;
            angleDeg -= 90f;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        }
    }
}
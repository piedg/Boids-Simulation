using System;
using Unity.Mathematics;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private BoidSettings _settings;
    public BoidSettings Settings => _settings;
    [SerializeField] private float _maxLinearSpeed = 5f;
    [SerializeField] private float _maxAngularSpeedDeg = 180f; 
    [SerializeReference, SubclassSelector]
    private SteeringBehaviour[] _steering = Array.Empty<SteeringBehaviour>();

    private float3 _velocity;
    private float   _angularVelocity; 

    public float  MaxLinearSpeed   => _maxLinearSpeed;
    public float  MaxAngularSpeed  => math.radians(_maxAngularSpeedDeg);

    public float3 LinearVelocity   => _velocity;
    public float   AngularVelocity => _angularVelocity;

    public float3 Position
    {
        get => (float3)transform.position;
        private set => transform.position = (Vector3)value;
    }

    public float Orientation
    {
        get => math.radians(transform.eulerAngles.z);
        private set => transform.rotation = quaternion.Euler(0, 0, value);
    }

    private void FixedUpdate()
    {
        float3 linearAccel = float3.zero;
        float  angularAccel = 0f;
        float  dt = Time.fixedDeltaTime;

        foreach (var sb in _steering)
        {
            var s = sb.GetSteering(this);
            linearAccel  += s.Linear;
            angularAccel += s.Angular;
        }

        _velocity        += linearAccel * dt;
        _angularVelocity += angularAccel * dt;

        _velocity = math.normalizesafe(_velocity) * math.min(math.length(_velocity), MaxLinearSpeed);
        _angularVelocity = math.clamp(_angularVelocity, -MaxAngularSpeed, MaxAngularSpeed);

        Position    += _velocity * dt;
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

            // se il tuo sprite punta "su", sottrai 90°; altrimenti togli questo offset
            angleDeg -= 90f;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        }
    }
}
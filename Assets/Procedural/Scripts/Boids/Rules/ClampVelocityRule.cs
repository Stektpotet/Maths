using System;
using UnityEngine;

public class ClampVelocityRule : BoidRule
{
    protected override void Start()
    {
        base.Start();
        blend = Override;
    }
    public float clampSpeed = 5f;
    public override Vector2 Rule(Boid boid)
    { return Vector2.ClampMagnitude(boid.velocity, 5); }
}
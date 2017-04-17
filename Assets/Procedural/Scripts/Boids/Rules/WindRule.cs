using System;
using UnityEngine;

public class WindRule : BoidRule
{
    protected override void Start()
    {
        base.Start();
    }
    public float windSpeed = 5f;
    [Tooltip("Direction of the wind, it will be normalized [0-1]")]
    public Vector2 windDirection;

    public override Vector2 Rule(Boid boid)
    { return windDirection.normalized * windSpeed; }
}
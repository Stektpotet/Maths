using System;
using UnityEngine;

public class FlyForwardRule : BoidRule
{
    public float speed = 1.0f;
   
    public override Vector2 Rule(Boid boid)
    { return speed*boid.direction; }
}
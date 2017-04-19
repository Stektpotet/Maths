using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FollowStreamRule : BoidRule
{
    protected override void Start()
    {
        base.Start();
        blend = AverageBlend;
    }

    public override Vector2 Rule(Boid boid)
    {
        Vector2 percievedVelocity = ( group.boids.Count < 2) ? Vector2.zero :
            (group.boids.Count * group.velocity - boid.position)/(group.boids.Count - 1);
            
        percentage = 1.0f - ( 1.0f + Vector2.Dot(
            percievedVelocity.normalized, boid.velocity.normalized) ) * 0.5f;
        //Debug.DrawLine(boid.position, boid.position + percievedVelocity, Color.yellow);
        return percievedVelocity;
    }
}

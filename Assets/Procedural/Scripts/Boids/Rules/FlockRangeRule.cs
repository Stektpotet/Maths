using System;
using UnityEngine;

class FlockRangeRule : BoidRule
{
    public float flockingRange;
    public float flockingStrength;

    public override Vector2 Rule(Boid boid)
    {
        Vector2 v = Vector2.zero;
        Collider2D[] collisionColliders = Physics2D.OverlapCircleAll(boid.position, flockingRange);
        foreach(Collider2D c in collisionColliders)
        {
            Vector2 antiCollide = ( (Vector2)c.transform.position - boid.position);
            v += antiCollide * flockingRange / ( antiCollide.magnitude + 0.0001f );
        }
        v /= collisionColliders.Length; v *= flockingStrength;
        Debug.DrawLine(boid.position, boid.position + v, Color.white);
        return v;
    }
}
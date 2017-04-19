using UnityEngine;

public class AntiCollisionRule : BoidRule
{
    public float collisionRange = 1.5f;
    public float antiCollideStrength = 1.0f;
    protected override void Start()
    {
        base.Start();
        blend = AverageBlend;
    }

    public override Vector2 Rule(Boid boid)
    {
        Vector2 v = Vector2.zero;
        Collider2D[] collisionColliders = Physics2D.OverlapCircleAll(boid.position, collisionRange);
        foreach(Collider2D c in collisionColliders)
        {
            Vector2 antiCollide = ( boid.position - (Vector2)c.transform.position );
            v += antiCollide * collisionRange/(antiCollide.magnitude + 0.0001f);
        }
        v /= collisionColliders.Length; v *= antiCollideStrength;
        //Debug.DrawLine(boid.position, boid.position + v, Color.magenta);
        return v;
    }
}
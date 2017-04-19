using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : BoidGroup
{
    public GameObject boidPrefab;

    public List<IBoidRule> swarmRules { get { return groupRules; } }
    
    public float homeRange = 30f;
    public float groupingRange = 5f;

    protected void Start()
    {
        foreach(BoidRule rule in GetComponents<BoidRule>())
        { swarmRules.Add(rule); }
    }

    protected override void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space)) { boids.Add(Instantiate(boidPrefab).GetComponent<Boid>()); Debug.Log("count:   " + boids.Count); }
        base.FixedUpdate();
        foreach(Boid boid in boids)
        {
            boid.velocity = Vector2.zero;
            //Perform the rules per boid body
            foreach(var rule in swarmRules)
            { rule.ApplyRule(boid); }
        }
    }

    //private Vector2 AntiCollide(Boid boid)
    //{
    //    Vector2 antiCollision = Vector2.zero;
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(boid.position, 2f);
    //    Vector2 relative;
    //    foreach(Collider2D c in colliders)
    //    {
    //        relative = ( boid.position - (Vector2)c.transform.position );
    //        antiCollision += relative / (relative.magnitude+0.0001f);
    //    }
    //    return antiCollision / colliders.Length;
    //}
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centerOfMass, 0.35f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, homeRange);
    }
    
}
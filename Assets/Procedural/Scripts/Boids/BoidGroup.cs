using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A dynamic group structure for boids.
/// Allowing for rules to be speciffic for only that selection.
/// </summary>
public class BoidGroup : MonoBehaviour
{
    protected Vector2 m_centerOfMass;
    protected Vector2 m_velocity;
    public Vector2 centerOfMass { get { return m_centerOfMass; } }
    public Vector2 velocity     { get { return m_velocity; } }
    public Vector2 direction    { get { return m_velocity.normalized; } } // vel/vel.magnitude

    public HashSet<Boid> boids = new HashSet<Boid>();
    protected List<IBoidRule> groupRules = new List<IBoidRule>();

    public float groupRadius = 5;

    protected virtual void FixedUpdate()
    {
        int boidCount = boids.Count;
        if(boidCount > 0)
        {
            m_centerOfMass *= 0;
            //m_velocity *= 0;
            foreach(Boid b in boids)
            {
                m_centerOfMass += b.position;
                m_velocity += b.velocity;
            }
            m_centerOfMass /= boidCount;
            m_velocity /= boidCount;
        }
    }
}
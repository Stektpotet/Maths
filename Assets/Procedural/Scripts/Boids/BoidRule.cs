using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Interface for component-base rules.
/// Will have the advantage of direct access to it's boid group
/// </summary>
[RequireComponent(typeof(BoidGroup))]
public abstract class BoidRule : MonoBehaviour, IBoidRule
{
    protected BoidGroup group;
    protected virtual void Start()
    {
        group = GetComponent<BoidGroup>();
        m_rule = Rule;
    }

    protected System.Func<Boid, Vector2> m_rule;
    protected System.Func<Vector2, Vector2, Vector2> m_blend = AdditiveBlend;
    public float percentage = 1;

    public Func<Boid, Vector2>              rule    { get { return m_rule;  } set { m_rule = value;  } }
    public Func<Vector2, Vector2, Vector2>  blend   { get { return m_blend; } set { m_blend = value; } }

    public Vector2 this[Boid boid] { get { return RuleEffect(boid); } }
    public Vector2 RuleEffect(Boid boid)
    { return blend(rule(boid) * percentage, boid.velocity); }

    public void ApplyRule(Boid boid)
    { boid.velocity = RuleEffect(boid); }

    //======================= Vector Blending Modes =========================
    public static Vector2 AdditiveBlend(Vector2 self, Vector2 other)
    { return self + other; }
    public static Vector2 AverageBlend(Vector2 self, Vector2 other)
    { return AdditiveBlend(self, other) * 0.5f; }
    public static Vector2 Override(Vector2 self, Vector2 other)
    { return self; }
    public abstract Vector2 Rule(Boid boid);
    //=======================================================================
}

//public class ScriptedBoidRule : IBoidRule
//{
//    protected System.Func<Boid, Vector2> m_rule;
//    protected System.Func<Vector2, Vector2, Vector2> m_blend = AdditiveBlend;
//    public float percentage = 1;

//    public Func<Boid, Vector2> rule { get { return m_rule; } }
//    public Func<Vector2, Vector2, Vector2> blend { get { return m_blend; } }

//    public Vector2 this[Boid boid] { get { return RuleEffect(boid); } }
//    public Vector2 RuleEffect(Boid boid)
//    { return blend(rule(boid) * percentage, boid.velocity); }

//    public void ApplyRule(Boid boid)
//    { boid.velocity = RuleEffect(boid); }

//    //======================= Vector Blending Modes =========================
//    public static Vector2 AdditiveBlend(Vector2 self, Vector2 other)
//    { return self + other; }
//    public static Vector2 AverageBlend(Vector2 self, Vector2 other)
//    { return AdditiveBlend(self, other) * 0.5f; }
//    public static Vector2 Override(Vector2 self, Vector2 other)
//    { return self; }
//    //=======================================================================
//}

public interface IBoidRule
{
    System.Func<Boid, Vector2>              rule { get; set; }
    System.Func<Vector2, Vector2, Vector2> blend { get; set; }
    void ApplyRule(Boid boid);
    Vector2 RuleEffect(Boid boid);
    Vector2 Rule(Boid boid);
}
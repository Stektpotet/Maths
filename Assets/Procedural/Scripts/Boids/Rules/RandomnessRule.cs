using UnityEngine;

public class RandomnessRule : BoidRule
{

    public float minRandomness = 0;
    public float maxRandomness = 1;
    public float x = 10f;

    void OnValidate()
    {
        minRandomness = Mathf.Min(maxRandomness, minRandomness);
    }

    protected override void Start()
    {
        base.Start();
        //blend = Override;
    }

    public override Vector2 Rule(Boid boid)
    {
        //percentage = (x/boid.velocity.magnitude+1);
        Vector2 v = new Vector2(
        Mathf.Sin(Time.time), Mathf.Cos(Time.time)) *
        Random.Range(minRandomness, maxRandomness);
        Debug.DrawLine(boid.position, boid.position + v, Color.cyan);
        return v;
    }
}
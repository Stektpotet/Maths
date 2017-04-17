using UnityEngine;

public class FlockRule : BoidRule
{
    protected override void Start()
    {
        base.Start();
        blend = AverageBlend;
    }
    public float maxFlocking = 1f;
    public override Vector2 Rule(Boid boid)
    {
        Vector2 v = Vector2.zero;
        if(group.boids.Count > 1)
        {
            //(com - b / 4) (4 / 3)
            Vector2 percievedCOM = 
                ( group.boids.Count * group.centerOfMass - boid.position) / (group.boids.Count - 1);
            v = ( percievedCOM - boid.position );
            Debug.DrawLine(boid.position, percievedCOM, Color.green);
            percentage = Mathf.Min(( v.magnitude / group.groupRadius ), maxFlocking);
        }
        //Debug.DrawLine(boid.position, boid.position + v, Color.red);
        return v;
    }
}
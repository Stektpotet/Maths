using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class KeepSwarmInView : MonoBehaviour
{
    public Swarm swarm;

    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        transform.position = (Vector3)swarm.centerOfMass + transform.position.z * Vector3.forward;
        float maxHeight = 5;
        foreach(Boid b in swarm.boids)
        {
            maxHeight = Mathf.Max( ((Vector2)transform.position-b.position).magnitude, maxHeight);
        }

        cam.orthographicSize = maxHeight;
    }
}

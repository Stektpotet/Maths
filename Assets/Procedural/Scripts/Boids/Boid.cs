//using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Boid : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public Vector2 direction { get { return transform.up; } }
    public Vector2 position { get { return transform.position; } set { transform.position = value; } }
    public Quaternion rotation { get { return transform.rotation; } set { transform.rotation = value; } }

    private Quaternion rotationTarget;

    void Start()
    {
        velocity = Vector2.right * Mathf.Cos(Time.time) + Vector2.up * Mathf.Sin(Time.time);
        Move();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        position += velocity * Time.fixedDeltaTime;
        rotationTarget = Quaternion.LookRotation(Vector3.forward, velocity);
    }
    void Update()
    {
        rotation = Quaternion.Slerp(rotation, rotationTarget, (Time.time - Time.fixedTime)*4);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)velocity);
        Gizmos.DrawSphere(transform.position + (Vector3)velocity, 0.25f);
        //Gizmos.DrawWireSphere(transform.position, 2);
    }
}

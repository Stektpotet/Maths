using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates projectiles at its transform's origin
/// </summary>
public class ProjectileLauncher : MonoBehaviour, IPointerDownCollision, IKeyDownEvent
{
    public Rigidbody projectilePrefab;

    public Transform target;

    public float speed = 10;
    public float delay = 0.5f;
    
    private static float g { get { return Physics.gravity.magnitude; } }
    

    bool firing = false;

    public void OnPointerDown(Vector3 clickHit)
    { target.position = clickHit; }

    private void Start()
    { StartCoroutine(FireEverySecond()); firing = true; }

    





    public bool CalculateAngle(out float alpha, out float beta)
    {
        float y = target.position.y - transform.position.y;
        Vector3 displaceXZ = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
        float x = displaceXZ.magnitude;
        float sqrSpeed = speed * speed;
        float insideSqrt = sqrSpeed * sqrSpeed - g * (g * x * x + 2 * y * sqrSpeed);
        if (insideSqrt > 0) //else, no real number solution
        {
            alpha = Mathf.Atan((sqrSpeed + Mathf.Sqrt(insideSqrt)) / (g * x));
            beta = Mathf.Atan((sqrSpeed - Mathf.Sqrt(insideSqrt)) / (g * x));
            return true;
        }
        else
        {
            alpha = beta = 0;
            return false;
        }
    }

    public Vector3 CalculateInitialVelocity()
    {
        float a, b;
        Vector3 velocity;
        Vector3 displaceXY = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);

        if (CalculateAngle(out a,out b))
        {
            velocity = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * speed;
            Vector3 right = Vector3.Cross(displaceXY, Vector3.up);
            velocity = Quaternion.LookRotation(right, Vector3.up) * velocity;
        }
        else
        { velocity = Vector3.zero; }
        return velocity;
    }




    IEnumerator FireEverySecond()
    {
        Rigidbody projectile;
        while (true)
        {
            //Debug.Log("Test");
            projectile = Instantiate(projectilePrefab.gameObject, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectile.velocity = (CalculateInitialVelocity());
            yield return new WaitForSeconds(delay);
        }
        
    }

    public void OnKeyDown(KeyCode key)
    {
        if(key == KeyCode.Space)
        {
            if(firing)
            { StopCoroutine(FireEverySecond()); }
            else
            { StartCoroutine(FireEverySecond()); }
            firing = !firing;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 lastPos = Vector3.zero;
        //for (float step = 0; step < (transform.position - target.position).magnitude; step += 0.1f)
        //{
        //    //Gizmos.DrawLine(lastPos, )
        //}
        //Gizmos.DrawLine(transform.position, transform.position + CalculateInitialVelocity());
    }
}

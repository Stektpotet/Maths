using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    Transform followTarget;
    void LateUpdate()
    {
        transform.position = Vector3.right * followTarget.position.x +
                             Vector3.up * followTarget.position.y;
    }
}

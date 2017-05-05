using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Lifetime - Created by Halvor, DESKTOP-A2KUJ84 @ 5/5/2017 1:16:34 AM
/// 
/// </summary>
public class Lifetime : MonoBehaviour
{
    public float time = 10;
    float m_livedTime = 0;
    private void Update()
    {
        m_livedTime += Time.deltaTime;
        if(time <m_livedTime)
        { Destroy(gameObject); }
    }
}

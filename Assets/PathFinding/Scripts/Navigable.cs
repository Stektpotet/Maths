using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
/// <summary>
/// Navigable - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:48:25 PM
/// 
/// </summary>
public class Navigable<T> where T : Node
{
    public int size = 32;
    T[,] nodes;

}
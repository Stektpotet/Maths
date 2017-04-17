using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStartColor : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
	}
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class SplineCurve : MonoBehaviour
{
	public List<SplineNode> controlPoints = new List<SplineNode>();
	public float period = 1.0f;

	//void OnEnable()
	//{
	//	while(controlPoints.Count < 4)
	//	{
	//		controlPoints.Add(new SplineNode());
	//	}
	//}

	public Vector3 GetPosition (float t) {
		return transform.TransformPoint(GetLocalPosition(t));
	}

	protected abstract Vector3 GetLocalPosition(float t);
}

[System.Serializable]
public class SplineNode
{
	public float t = 0;

	public Vector3 position = Vector3.zero;
	public Quaternion rotation = Quaternion.identity;
	public Vector3 scale = Vector3.one;

	public bool positionEnabled = false;
	public bool rotationEnabled = false;
	public bool scaleEnabled = false;

	public SplineNode(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}
	
	public SplineNode (Vector3 position)
	{
		this.position = position;
	}

	public SplineNode(){ Debug.Log("Spline Node created :)"); }
}

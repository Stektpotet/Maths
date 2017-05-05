﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatmullRomSpline : SplineCurve {
	public bool looping = false; //When looping, the curve ignores the first and last control points, and uses points on the other side of the array instead

	/// <summary>
	/// Calculates and returns point t on this Catmull-Rom curve
	/// </summary>
	/// <param name="t">Time passed. Determines how far along the curve the returned point is</param>
	/// <returns></returns>
	protected override Vector3 GetLocalPosition (float t) {
		float normalTimeElapsed = (t / period)%1;
		int segmentCount = controlPoints.Count - 3;
		if (looping) {
			segmentCount++;
		}
		int currentSegment = Mathf.FloorToInt(normalTimeElapsed * segmentCount);
		float localT = (normalTimeElapsed * segmentCount) % 1;
		if (looping) {
			if (currentSegment == 0) {
				return GetLocalPosition(localT,controlPoints[controlPoints.Count-2], controlPoints[1], controlPoints[2], controlPoints[3]);
			} else if (currentSegment == segmentCount - 2) {
				return GetLocalPosition(localT, controlPoints[controlPoints.Count-4], controlPoints[controlPoints.Count - 3], controlPoints[controlPoints.Count - 2], controlPoints[1]);
			} else if (currentSegment == segmentCount -1) {
				return GetLocalPosition(localT, controlPoints[controlPoints.Count - 3], controlPoints[controlPoints.Count - 2], controlPoints[1], controlPoints[2]);
			}
		}
		return GetLocalPosition(localT, currentSegment);
	}

	/// <summary>
	/// Calculates and returns point t on a specified segment of this Catmull-Rom curve
	/// </summary>
	/// <param name="t">Time between 0 and 1. Determines how far along the curve the returned point is</param>
	/// <param name="segment">The segment in which the desired point is located</param>
	/// <returns></returns>
	Vector3 GetLocalPosition (float t, int segment) {
		if (controlPoints.Count < 4) {
			return Vector3.zero;
		}
		return GetLocalPosition(t, controlPoints.GetRange(segment, 4));
	}

	/// <summary>
	/// Calculates and returns the point on a Catmull-Rom curve segment described by a t between 0 and 1, and four sequential control points
	/// </summary>
	/// <param name="t">A value between 0 and 1 describing how far along the curve the returned point should be</param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <param name="d"></param>
	/// <returns></returns>
	public static Vector3 GetLocalPosition (float t, SplineNode a, SplineNode b, SplineNode c, SplineNode d) {
		return GetLocalPosition(t, new List<SplineNode>() { a, b, c, d });
	}

	/// <summary>
	/// Calculates and returns the point on a Catmull-Rom curve segment described by a t between 0 and 1, and a list of four control points
	/// </summary>
	/// <param name="t">A value between 0 and 1 describing how far along the curve the returned point should be</param>
	/// <param name="P">A list of four points used to describe the curve</param>
	/// <returns></returns>
	public static Vector3 GetLocalPosition (float t, List<SplineNode> P) {
		if (P.Count < 4) {
			return Vector3.zero;
		}
		return 0.5f * (
			(2 * P[1].position) +
			(P[2].position - P[0].position) * t +
			(2 * P[0].position - 5 * P[1].position + 4 * P[2].position - P[3].position) * t * t +
			(3 * P[1].position - P[0].position - 3 * P[2].position + P[3].position) * t * t * t);
	}
}

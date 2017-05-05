using UnityEngine;
using System.Collections;

public class SplineCurveFollower : ControlledBehaviour
{
	public SplineCurve spline;

	public override float t { get{return _t;} set{_t = value%spline.period; atEnd = false;}}
    private float _t;

	public bool loopingMovement = true;
	public bool paused { get; private set; }
	public bool followPosition = true;
	public bool followRotation = true;
	public bool followScale = true;
	private bool atEnd = false;

	public float derivationDelta = 0.0001f;
	
	SplineCurveFollower ()
	{
		paused = true;
	}

	void Start()
	{
		if (!paused)
			Play();
	}

	internal virtual IEnumerator Movement()
	{
		while (true)
		{
			IncrementTime();
			if ((t >= spline.period || t < 0) && !loopingMovement)
			{
				//Makes sure the animation stops at the end, instead of jumping to the beginning position
				t = -0.000001f;
				atEnd = true;
				Pause();
			}
			Clampt();
			if (followPosition)
				UpdatePosition();
			if (followRotation)
				UpdateRotation();
			yield return null;
		}
	}


	/// <summary>
	/// Clamps t between 0 and period. Pronounced "Clamp tee"
	/// </summary>
	protected void Clampt()
	{
		//The extra + and % is to remove the possibility of a negative t
		t = (t % spline.period + spline.period) % spline.period;
	}

	protected void IncrementTime()
	{
		t += Time.deltaTime * speedFactor;
	}

	protected void UpdatePosition ()
	{
		transform.position = spline.GetPosition(t);
	}

	protected void UpdateRotation()
	{
		Vector3 nextPosition = spline.GetPosition(t + derivationDelta);
		transform.LookAt(nextPosition);
		Debug.DrawLine(transform.position, nextPosition, Color.red);
	}

	public override void Restart()
	{
		Pause();
		t = 0.0f;
		if (followPosition)
			UpdatePosition();
		if (followRotation)
			UpdateRotation();
	}

	public override void Play()
	{
		if (atEnd)
			Restart();
		paused = false;
		if (Application.isPlaying)
			StartCoroutine("Movement");
	}

	public override void Pause()
	{
		paused = true;
		StopCoroutine("Movement");
	}

	/// <summary>
	/// Increases, decreases or reverses the speed of the follower
	/// </summary>
	/// <param name="factor">The factor by which the speed increases</param>
	public void SetSpeedByFactor (float factor)
	{
		speedFactor *= factor;
	}
}

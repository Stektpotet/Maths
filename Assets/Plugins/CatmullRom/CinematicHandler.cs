using UnityEngine;
using System.Collections.Generic;

public abstract class ControlledBehaviour : MonoBehaviour
{
	public virtual void Play() { Debug.LogWarning(this.GetType().ToString() + ".Play() was not defined"); }
	public virtual void Pause() { Debug.LogWarning(this.GetType().ToString() + ".Pause() was not defined"); }
	public virtual void Restart() { Debug.LogWarning(this.GetType().ToString() + ".Restart() was not defined"); }
	public virtual float t { get; set; }
	public float speedFactor = 1f;
}

public class CinematicHandler : MonoBehaviour
{
	public List<ControlledBehaviour> cinematicObjects;

	void PlayIndex (int i)
	{
		cinematicObjects[i].Play();
	}
	void PauseIndex (int i)
	{
		cinematicObjects[i].Pause();
	}
	void RestartIndex (int i)
	{
		cinematicObjects[i].Restart();
	}

}

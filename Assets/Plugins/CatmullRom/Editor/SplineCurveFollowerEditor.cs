using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineCurveFollower),true)]
class SplineCurveFollowerEditor : Editor
{
	SplineCurveFollower follower;
	void OnEnable()
	{
		follower = target as SplineCurveFollower;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginHorizontal();
		{
			float h = 20.0f;
			if (GUILayout.Button(new GUIContent(EditorGUIUtility.FindTexture("Animation.PrevKey"), "Stop the follower, and place it at the start of the curve"), EditorStyles.miniButtonLeft, GUILayout.Height(h)))
			{
				follower.Restart();
			}

			EditorGUI.BeginChangeCheck();
			bool playing = GUILayout.Toggle(!follower.paused, new GUIContent(EditorGUIUtility.FindTexture("Animation.Play"), "Play or Pause the movement"), EditorStyles.miniButtonMid, GUILayout.Height(h));
			if (EditorGUI.EndChangeCheck())
			{
				if (playing)
				{
					follower.Play();
				}
				else
				{
					follower.Pause();
				}
			}

			if (GUILayout.Button(new GUIContent("Speed up", "Speeds up the object to 2x its current speed"), EditorStyles.miniButtonMid, GUILayout.Height(h)))
			{
				follower.SetSpeedByFactor(2.0f);
			}
			if (GUILayout.Button(new GUIContent("Slow down", "Slows down the object to half its current speed"), EditorStyles.miniButtonMid, GUILayout.Height(h)))
			{
				follower.SetSpeedByFactor(0.5f);
			}
			if (GUILayout.Button(new GUIContent("Reverse", "Reverses the object's speed"), EditorStyles.miniButtonRight, GUILayout.Height(h)))
			{
				follower.SetSpeedByFactor(-1.0f);
			}
		}
		GUILayout.EndHorizontal();
	}
}

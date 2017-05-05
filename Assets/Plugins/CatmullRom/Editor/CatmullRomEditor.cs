﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
[CustomEditor(typeof(CatmullRomSpline)), CanEditMultipleObjects]
public class CatmullRomEditor : Editor
{
    private const float handleSize = 0.10f;
    private float splineGirth = 2f;
    private bool drawNodes = true, drawSpline = true;

    private ReorderableList controlPointList;
	private bool canRemove { get { return controlPointList.count > 4; } }

    private CatmullRomSpline spline;
    private SerializedProperty
		periodProp,
		loopingProp;
    

    private Transform handleTransform;
    private Quaternion handleRotation;

    private int selectedControlPointIndex = -1;
	private Vector3? clipboardPoint = null;

	private static class Styles
	{
        public static GUIContent nodeLabel = new GUIContent("Nodes");
        public static GUIContent splineLabel = new GUIContent("Spline");

        public static Color endPointElementBackground = new Color(0.3f,0.3f,0.7f);
		public static Color elementBackground = Color.gray;
		public static Color selectedElementBackground = new Color(0.3f, 0.3f, 1);

		public static Color controlPointColor = Color.blue;
		public static Color splineColor = Color.green;

		public static Color guiColor = GUI.color;

		public static GUIStyle visibilityToggle = "VisibilityToggle";
        public static GUIStyle sceneViewWindow = "TL Range Overlay";
	}

    void OnEnable()
    {
        spline = target as CatmullRomSpline;
        controlPointList = new ReorderableList(serializedObject, serializedObject.FindProperty("controlPoints"), true, true, true, true);
        periodProp = serializedObject.FindProperty("period");
		loopingProp = serializedObject.FindProperty("looping");

        handleTransform = spline.transform;
        handleRotation = ( Tools.pivotRotation == PivotRotation.Local ) ? handleTransform.rotation : Quaternion.identity;


		controlPointList.elementHeight *= 2;
        controlPointList.drawHeaderCallback += HeaderDraw;
        controlPointList.drawElementCallback += ElementDraw;
        controlPointList.drawFooterCallback += FooterDraw;
		controlPointList.drawElementBackgroundCallback += ElementBackgroundDraw;

	}

    void OnDisable()
    {
		controlPointList.elementHeight /= 2;
		controlPointList.drawHeaderCallback -= HeaderDraw;
        controlPointList.drawElementCallback -= ElementDraw;
        controlPointList.drawFooterCallback -= FooterDraw;
		controlPointList.drawElementBackgroundCallback -= ElementBackgroundDraw;
	}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
		controlPointList.DoLayoutList();
		EditorGUILayout.Space();
        periodProp.floatValue = Mathf.Max(EditorGUILayout.DelayedFloatField("Period", periodProp.floatValue),0.001f);
		serializedObject.ApplyModifiedProperties();
    }

    #region listCallbacks

    void HeaderDraw(Rect hRect)
    {
        hRect.y += 1f;
		hRect.height -= 2;
        GUI.Label(new Rect(hRect.x, hRect.y, 100F, hRect.height),"Control Points");
		
		EditorGUI.BeginChangeCheck();
		bool loop = EditorGUI.Toggle(new Rect(hRect.x + hRect.width - 125F, hRect.y, 70F, hRect.height), loopingProp.boolValue, EditorStyles.miniButton);
		if (EditorGUI.EndChangeCheck())
		{
			loopingProp.boolValue = loop;
		}
		GUI.Label(new Rect(hRect.x + hRect.width - 112F, hRect.y-2F, 70F, hRect.height+2F), new GUIContent("Looping", "Should the spline loop?"), EditorStyles.miniBoldLabel);
		GUI.enabled = canRemove;
		EditorGUI.BeginChangeCheck();
        int value = EditorGUI.DelayedIntField(new Rect(hRect.x+hRect.width-45F,hRect.y,45F, hRect.height), controlPointList.count);
        if(EditorGUI.EndChangeCheck())
        {
			value = Mathf.Max(value, 4);
            if(controlPointList.count > value)
            {
                while(controlPointList.count > value)
                {
                    controlPointList.serializedProperty.DeleteArrayElementAtIndex(controlPointList.count-1);
                }
            }
            else if (controlPointList.count < value)
            {
                while(controlPointList.count < value)
                {
                    controlPointList.serializedProperty.InsertArrayElementAtIndex(controlPointList.count);
                }
            }
        }
		GUI.enabled = true;
    }

    void ElementDraw(Rect eRect, int i, bool active, bool focused)
    {
		SerializedProperty element = controlPointList.serializedProperty.GetArrayElementAtIndex (i);
		SerializedProperty position = element.FindPropertyRelative("position");
        if (spline.looping && (i == 0 || i == controlPointList.count - 1))
		{
			GUI.enabled = false;
		}
		eRect.y += 1;
		eRect.height *= 0.5f;
        eRect.height -= 2;

        //Set Button
        if(GUI.Button(new Rect(eRect.x + 1, eRect.y, 38, eRect.height), new GUIContent("SET", "Use the current Scene-view camera position")))
        {
			position.vector3Value = spline.transform.InverseTransformPoint(SceneView.lastActiveSceneView.camera.transform.position);
        }
        eRect.xMax -= 2;
        eRect.y += 2;
        eRect.height -= 1;
		
        EditorGUI.BeginChangeCheck();
		Vector3 value = EditorGUI.Vector3Field(new Rect (eRect.x+40, eRect.y, eRect.width-40, eRect.height-2),GUIContent.none, position.vector3Value);
        if(EditorGUI.EndChangeCheck())
        {
			position.vector3Value = value;
        }
		eRect.y += eRect.height;


		//Copy Button
		if (GUI.Button(new Rect(eRect.x + 1, eRect.y, eRect.width * 0.5f, eRect.height), new GUIContent("Copy", "Copy position"),EditorStyles.miniButtonLeft))
		{
			clipboardPoint = position.vector3Value;
        }


		//Paste Button
		if(clipboardPoint == null)
		{
			GUI.enabled = false;
		}
		if (GUI.Button(
				new Rect(eRect.x + 1 + eRect.width * 0.5f, eRect.y, eRect.width * 0.5f, eRect.height),
				new GUIContent("Paste", ((clipboardPoint != null) ? "Paste " + clipboardPoint.ToString() : "Clipboard empty")),
				EditorStyles.miniButtonRight)
			)
		{
			GUI.FocusControl("");
			if(clipboardPoint != null)
				position.vector3Value = clipboardPoint.GetValueOrDefault();
		}
		GUI.enabled = true;
	}

	void ElementBackgroundDraw(Rect ebRect, int i, bool active, bool focused)
	{
		SerializedProperty element = controlPointList.serializedProperty.GetArrayElementAtIndex (i);
		SerializedProperty position = element.FindPropertyRelative("position");
		if (spline.looping && (i == 0 || i == controlPointList.count - 1)) {
			GUI.enabled = false;
		}
		
		if (focused && active)
		{
			selectedControlPointIndex = i;
			SceneView.RepaintAll();
		}
		Event current = Event.current;
		if (current.GetTypeForControl(GUIUtility.GetControlID(FocusType.Passive)) == EventType.MouseDown)
		{
			if (current.clickCount == 2)
			{
				if (ebRect.Contains(current.mousePosition))
				{
					SceneView.lastActiveSceneView.LookAt(spline.transform.TransformPoint(position.vector3Value));
				}
			}
		}
		ebRect.y += 1;
		ebRect.height -= 2;
		ebRect.x += 1;
		ebRect.width -= 2;

		if (active)
		{
			GUI.color = Styles.selectedElementBackground;
		}
		else if (focused)
		{
			GUI.color = 0.5f * ((i == 0 || i == controlPointList.count - 1) ? Styles.endPointElementBackground : Styles.elementBackground);
		}
		else
		{
			GUI.color = (i == 0 || i == controlPointList.count - 1) ? Styles.endPointElementBackground : Styles.elementBackground;
		}
		GUI.Box(ebRect, GUIContent.none);
		GUI.color = Styles.guiColor;
		GUI.enabled = true;
	}

	void FooterDraw(Rect fRect)
	{
		fRect.x += EditorGUI.indentLevel + 1;
		fRect.y -= 3;
		fRect.height += 4;
		GUI.enabled = canRemove;
		GUI.Box(new Rect(fRect.x + 2, fRect.y, 38F, fRect.height), GUIContent.none, EditorStyles.toolbarButton);

		//Point Count
		EditorGUI.BeginChangeCheck();
		int value = EditorGUI.DelayedIntField(new Rect(fRect.x + 2, fRect.y, 38F, fRect.height), controlPointList.count);
		if (EditorGUI.EndChangeCheck())
		{
			value = Mathf.Max(value, 4);
			if (controlPointList.count > value)
			{
				while (controlPointList.count > value)
				{
					controlPointList.serializedProperty.DeleteArrayElementAtIndex(controlPointList.count - 1);
				}
			}
			else if (controlPointList.count < value)
			{
				while (controlPointList.count < value)
				{
					controlPointList.serializedProperty.InsertArrayElementAtIndex(controlPointList.count);
				}
			}
		}
		GUI.enabled = true;
		if (clipboardPoint == null)
		{
			GUI.enabled = false;
		}
		if (GUI.Button(new Rect(fRect.x + 40F, fRect.y, fRect.width - 100F, fRect.height), new GUIContent("Paste as new", ((clipboardPoint != null) ? "Paste " + clipboardPoint.ToString() : "Clipboard empty")), EditorStyles.toolbarButton))
		{
			if (clipboardPoint != null)
			{
				int i = controlPointList.count;
				controlPointList.serializedProperty.InsertArrayElementAtIndex(i);
				controlPointList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector3Value = clipboardPoint.GetValueOrDefault();
			}
		}
		GUI.enabled = true;
		if (GUI.Button(new Rect(fRect.x + fRect.width - 64F, fRect.y, 30F, fRect.height), EditorGUIUtility.IconContent("Toolbar Plus", "Add to list"), EditorStyles.toolbarButton))
		{
			ReorderableList.defaultBehaviours.DoAddButton(controlPointList);
		}
		GUI.enabled = canRemove;
        if (GUI.Button(new Rect(fRect.x + fRect.width - 34F, fRect.y, 30F, fRect.height), EditorGUIUtility.IconContent("Toolbar Minus", "Remove element from list"), EditorStyles.toolbarButton))
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(controlPointList);
		}
		GUI.enabled = true;
	}

    #endregion

    #region SceneView

    void OnSceneGUI()
	{
		Rect area = new Rect(Screen.width - 210, Screen.height-130, 200, 100);
		Handles.BeginGUI();
        GUILayout.Window(200,area,DrawSceneWindow,"", Styles.sceneViewWindow);
		Handles.EndGUI();

        if(drawSpline)
        {
			DrawCurve();
		}

        if(drawNodes)
        {
            for(int i = 0; i < spline.controlPoints.Count; i++)
            {
                DrawControlPoint(i);
            }
        }
    }

    private void DrawSceneWindow(int id)
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        // Content of window here
        EditorGUIUtility.labelWidth = 1;
        EditorGUILayout.BeginHorizontal();
        drawNodes = EditorGUILayout.Toggle(drawNodes, Styles.visibilityToggle);
        GUILayout.Label(Styles.nodeLabel);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        drawSpline = EditorGUILayout.Toggle(drawSpline, Styles.visibilityToggle);
        GUILayout.Label(Styles.splineLabel);
        if(drawSpline)
        {
            splineGirth = EditorGUILayout.FloatField(splineGirth);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawControlPoint(int index)
    {
		if (spline.looping && (index == 0 || index == spline.controlPoints.Count - 1))
		{
			return;
		}
        Vector3 worldSpaceOriginalPoint = spline.transform.TransformPoint(spline.controlPoints[index].position);
		float hSize = HandleUtility.GetHandleSize(worldSpaceOriginalPoint) * handleSize;
		Handles.color = Styles.controlPointColor;
		if (selectedControlPointIndex == index)
        {
			//Do position handle
			Handles.DotCap(100, worldSpaceOriginalPoint, Quaternion.identity, hSize);
            EditorGUI.BeginChangeCheck();
            Vector3 worldSpacePoint = Handles.DoPositionHandle(worldSpaceOriginalPoint, handleRotation);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Catmull-Rom control point");
                controlPointList.index = -1;
                EditorUtility.SetDirty(spline);
                spline.controlPoints[index].position = spline.transform.InverseTransformPoint(worldSpacePoint);
            }
        }
        else
        {
            //Do selection button    
            if(Handles.Button(worldSpaceOriginalPoint, handleRotation, hSize, hSize + 0.05f, Handles.DotCap))
            {
                selectedControlPointIndex = index;
            }
        }
    }

    void DrawCurve()
    {
        Handles.color = Styles.splineColor;
        Vector3 v = spline.GetPosition(0.0f);
        Vector3 u;
        for (float i = 0.03f; i <= spline.period; i += 0.03f)
        {
			u = spline.GetPosition(i);
            Handles.DrawAAPolyLine(splineGirth * 200.0f/Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, v), 2, v, u);
            v = u;
        }
        u = spline.GetPosition(spline.period-0.001f);//just to make sure it reaches the endpoint visually
        Handles.DrawAAPolyLine(20f, 2, v, u);

    }
    #endregion
}
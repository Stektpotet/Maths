using UnityEngine;
using UnityEditor;

public class MidpointDisplacement : MonoBehaviour
{
    public int size = 16;
    public float maxHeight = 10f;
    public float stepSize = 0.05f;
    public int seed; //Randomizer seed
    public int curve = -1; //use the alpha value instead if -1
    public AnimationCurve[] alphaCurves;

    System.Random rand; 

    public System.Func<int, float> displaceFunc;

    int nodes { get { return size + 1; } }

    float[] heights;

    void Start()
    { Redraw(); }

    void OnValidate()
    {
        size = Mathf.ClosestPowerOfTwo(size); //only accept powOfTwo
        curve = Mathf.Min(curve, alphaCurves.Length - 1);
        rand = new System.Random(seed);
        displaceFunc = x => ( curve < 0 ) ? DefaultDisplace(x) : CurveDisplace(x);
    }

    void Update()
    { if(Input.GetKeyDown(KeyCode.Space)) { Redraw(); } }

    void Redraw()
    {
        size = Mathf.ClosestPowerOfTwo(size); //only accept powOf2
        heights = new float[nodes];
        DoMidpointDisplacement();
        SceneView.lastActiveSceneView.Repaint(); //Repaint the view
    }
    void DoMidpointDisplacement()
    {
        heights[0] = 0 * maxHeight;
        heights[size] = 0 * maxHeight;
        SampleHeight(0, size);
    }
    void SampleHeight(int y1, int y2)
    {
        int yMid = ( y1 + y2 ) / 2;
        //-1 / a² x² + x / (a / 2)
        heights[yMid] = ( heights[y1] + heights[y2] ) / 2 + displaceFunc(Mathf.Abs(y2 - y1));
        if(Mathf.Abs(y2 - y1) > 2)
        {
            SampleHeight(y1, yMid);
            SampleHeight(yMid, y2);
        }
    }

    float DefaultDisplace(float dist)
    {
        return ( (float)rand.NextDouble() - 0.5f )*(dist/size)* maxHeight;
    }

    float CurveDisplace(float dist)
    {
        return ( (float)rand.NextDouble() - 0.5f ) * alphaCurves[curve].Evaluate(dist / size) * maxHeight;
    }

    void OnDrawGizmos()
    {
        for(int i = 0; i < size; i++)
        {
            Gizmos.color = Color.Lerp(Color.green, Color.red, heights[i] / maxHeight);
            int pos = i - ( size / 2 );
            Gizmos.DrawLine(
              new Vector3(pos * stepSize, heights[i]),
              new Vector3(( pos + 1 ) * stepSize, heights[i + 1]));
        }
    }

}

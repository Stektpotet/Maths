using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainMe : MonoBehaviour
{
    public TerrainData td;
    public DiamondSquare diamondSquare;

    void Start()
    {
        diamondSquare.size = td.heightmapWidth - 1;
        Trigger();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        { Trigger(); }
    }
    
    public void Trigger()
    {
        diamondSquare.Generate();
        diamondSquare.Create(values => 
        {
            td.SetHeights(0, 0, values);
            return 0; //Create needs a return type
        });
    }
}

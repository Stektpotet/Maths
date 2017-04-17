using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMe : MonoBehaviour
{
    public Gradient colorPalette;
    public DiamondSquare diamondSquare;
    Renderer rend;
    [Range(0.001f, 5.0f)]
    public float timeScale = 1f;
    void Start()
    {
        rend = GetComponent<Renderer>();
        Trigger();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        { Trigger(); }
        rend.material.mainTexture = diamondSquare.Create(CreateTexture);
    }

    public void Trigger()
    {
        diamondSquare.Generate();
        rend.material.mainTexture = diamondSquare.Create(CreateTexture);
    }

    public Texture2D CreateTexture(float[,] values)
    {
        Texture2D tex = new Texture2D(values.GetLength(0), values.GetLength(1), TextureFormat.RGBA32, false);
        for(int y = 0; y < tex.height; y++)
        {
            
            for(int x = 0; x < tex.width; x++)
            { tex.SetPixel(x, y, colorPalette.Evaluate((/*Time.time*timeScale+*/values[x, y]) % 1.0f)); }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point; 
        return tex;
    }
}

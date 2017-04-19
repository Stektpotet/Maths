using UnityEngine;
[System.Serializable]
public class DiamondSquare
{
    public int seed;
    public int size;
    public int curve = 0;
    public AnimationCurve[] alphaCurves;
    System.Random rand;
    float[,] m_values;
    int m_nodes
    { get { return size + 1; } }

    private float min, max;

    public float[,] Generate()
    {
        //local "constants", (they should and will never change during the function)
        int maxDepth = (int)Mathf.Log(size, 2);
        //local tempvariables
        int stepSize = size;
        int halfSize;

        //b -> bottom, t -> top, m -> middle, l -> left, r -> right
        //float bl, bm, br, lm, m, rm, tl, tm, tr;

        //Setup
        rand = new System.Random(seed);
        m_values = new float[m_nodes, m_nodes];
        //corners = 0.0f - 1.0f (inclusive)
        m_values[0, 0] = (float)rand.NextDouble(); m_values[size, size] = (float)rand.NextDouble();
        m_values[0, size] = (float)rand.NextDouble(); m_values[size, 0] = (float)rand.NextDouble();

        //m_values[0, 0] = m_values[size, size] = m_values[0, size] = m_values[size, 0] = 1;

        //THE ALGORITHM
        for(int depth = 0; depth < maxDepth; depth++) // 2^(2*depth) squares
        {
            halfSize = stepSize / 2;
            for(int y = 0; y < size; y += stepSize)
            {
                for(int x = 0; x < size; x += stepSize)
                {
                    float value = Average(
                        m_values[x, y],
                        m_values[x + stepSize, y],
                        m_values[x, y + stepSize],
                        m_values[x + stepSize, y + stepSize]) + Displace(stepSize);
                    m_values[x + halfSize, y + halfSize] = value;
                    if(m_values[x + halfSize, y + halfSize] < min) { min = m_values[x + halfSize, y + halfSize]; }
                    if(m_values[x + halfSize, y + halfSize] > max) { max = m_values[x + halfSize, y + halfSize]; }

                    if(x == 0) { m_values[size, y] = value; }
                    if(y == 0) { m_values[x, size] = value; }
                }
            }
            for(int dStart = 0; dStart < m_nodes; dStart += halfSize)
            {
                for(int d = dStart + halfSize; d < m_nodes; d += stepSize)
                {
                    Diamond(dStart, d, halfSize);
                    Diamond(d, dStart, halfSize);
                }
            }
            stepSize = halfSize;
        }
        for(int y = 0; y < m_nodes; y++)
        {
            for(int x = 0; x < m_nodes; x++)
            {
                m_values[x, y] = (Mathf.Clamp(m_values[x, y], min, max)) / (max - min);
            }
        }
        return m_values;
    }
    float Displace(float stepSize)
    {
        return ( (float)rand.NextDouble() - 0.5f ) * alphaCurves[curve].Evaluate(stepSize / size);
    }

    void Diamond(int x, int y, int halfStep)
    {
        float value = Average(
            m_values[( x + halfStep ) % size, y],
            m_values[( x - halfStep + size ) % size, y],
            m_values[x, ( y + halfStep ) % size],
            m_values[x, ( y - halfStep + size ) % size]);// + Displace(halfStep);
        m_values[x, y] = value;
        if(m_values[x, y] < min) { min = m_values[x, y]; }
        if(m_values[x, y] > max) { max = m_values[x, y]; }
        if(x == 0) { m_values[size, y] = value; }
        if(y == 0) { m_values[x, size] = value; }
    }

    private float Average(params float[] items)
    {
        float avg = 0;
        for(int i = 0; i < items.Length; i++)
        { avg += items[i]; }
        return ( items.Length == 0 ) ? 0 : (avg / items.Length);
    }

    public T Create<T>(System.Func<float[,], T> creatorFunc)
    { return creatorFunc(m_values); }
}
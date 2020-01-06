using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterplotGenerator : MonoBehaviour
{
    public GameObject defaultGlyph; // 3D model to generate as a glyph
    public GameObject pointsHolder; // Empty Parent to hold all the points, to avoid clutter

    [SerializeField]
    DataReader reader;

    DataReader.DataPoint[] dataPoints;

    List<GameObject> glyphList = new List<GameObject>();

    Dictionary<string, int> ClusterColorMap = new Dictionary<string, int>();
    int colorMapCounter = 0;

    Color[] clusterColors = { Color.magenta, Color.red, Color.gray, Color.yellow };

    int rowIndex = 0;
    bool dropFlag = false;

    int rowCount, columnCount;

    float xMin, yMin, zMin;
    float xMax, yMax, zMax;

    private float plotScale = 0.25f;

    void Start()
    {
        reader.LoadData();

        dataPoints = reader.DataFrame;
        rowCount = reader.RowCount;
        columnCount = reader.ColumnCount;

        foreach (string clusterName in reader.ClusterMap.Keys)
        {
            ClusterColorMap.Add(clusterName, colorMapCounter++);
        }

        CreateScatterplot();
    }

    void Update()
    {
    }

    void CreateScatterplot()
    {
        int i = 0;

        float[] minValues = GetMinValues();
        float[] maxValues = GetMaxValues();

        SetMinMax(minValues, maxValues);

        Vector3 objectPosition;
        GameObject glyph;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            float plotOffset = 0.025f; // value offset to make sure point (0, 0) is not at the origin but a little away from it

            // normalize each data point & add offset
            float x = ((point.X - xMin) / (xMax - xMin)) + plotOffset;
            float y = ((point.Y - yMin) / (yMax - yMin)) + plotOffset;
            float z = ((point.Z - zMin) / (zMax - zMin)) + plotOffset;

            objectPosition = new Vector3(x, y, z);
            //print(objectPosition); // Object Position normalized, uncomment for debugging
            glyph = Instantiate(defaultGlyph, (objectPosition - new Vector3(0.5f, 0f, 0.5f)) * plotScale, Quaternion.identity);
            glyph.name = i++.ToString();
            glyph.transform.parent = pointsHolder.transform;
            print(ClusterColorMap[point.Cluster] + ": " + ClusterColorMap.Count);
            if (ClusterColorMap[point.Cluster] < clusterColors.Length)
            {
                glyph.GetComponent<Renderer>().material.color = clusterColors[ClusterColorMap[point.Cluster]];
            }
            else
            {
                glyph.GetComponent<Renderer>().material.color = Color.white;
            }
            glyphList.Add(glyph);
        }

        /* 
        print(xMin + " " + yMin + " " + zMin);
        print(xMax + " " + yMax + " " + zMax);
        
        for (int j=0; j<rowCount; j++)
        {
            Invoke("AnimateDisplay", Random.Range(0.15f, 0.20f) * 0.1f + 0.1f * j);
        }
        */

    }


    void AnimateDisplay()
    {
        glyphList[rowIndex++].SetActive(true);
        if (rowIndex == rowCount - 1)
        {
            dropFlag = true;
        }
    }


    float[] GetMinValues()
    {
        float[] minValues = new float[3]; // { 0: x, 1: y, 2: z}

        minValues[0] = dataPoints[0].X;
        minValues[1] = dataPoints[0].Y;
        minValues[2] = dataPoints[0].Z;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            if (point.X < minValues[0])
            {
                minValues[0] = point.X;
            }
            if (point.Y < minValues[1])
            {
                minValues[1] = point.Y;
            }
            if (point.Z < minValues[2])
            {
                minValues[2] = point.Z;
            }
        }

        return minValues;
    }

    float[] GetMaxValues()
    {
        float[] maxValues = new float[3]; // { 0: x, 1: y, 2: z}

        maxValues[0] = dataPoints[0].X;
        maxValues[1] = dataPoints[0].Y;
        maxValues[2] = dataPoints[0].Z;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            if (point.X > maxValues[0])
            {
                maxValues[0] = point.X;
            }
            if (point.Y > maxValues[1])
            {
                maxValues[1] = point.Y;
            }
            if (point.Z > maxValues[2])
            {
                maxValues[2] = point.Z;
            }
        }

        return maxValues;
    }

    void SetMinMax(float[] minValues, float[] maxValues)
    {
        xMin = minValues[0];
        yMin = minValues[1];
        zMin = minValues[2];

        xMax = maxValues[0];
        yMax = maxValues[1];
        zMax = maxValues[2];
    }
}

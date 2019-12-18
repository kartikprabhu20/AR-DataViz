using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScatterplotGenerator : MonoBehaviour
{
    public GameObject defaultGlyph; // 3D model to generate as a glyph
    public GameObject graphGen;
    public GameObject pointsHolder; // Empty Parent to hold all the points, to avoid clutter

    [SerializeField]
    DataReader reader;

    DataReader.DataPoint[] dataPoints;

    List<GameObject> glyphList = new List<GameObject>();
    int rowIndex = 0;
    int rowCount, columnCount;

    float xMin, yMin, zMin;
    float xMax, yMax, zMax;

    float plotscale = 0f;


    void Start()
    {
        reader.LoadData();
        dataPoints = reader.DataFrame;
        rowCount = reader.RowCount;
        columnCount = reader.ColumnCount;

        StartCoroutine(CreateScatterplot());
    }

    //public void assignPointsHolder(GameObject pointsHolderClone)
    //{
    //    pointsHolder = pointsHolderClone;
    //}

    public void assignGraphGen(GameObject graphGenClone)
    {
        graphGen = graphGenClone;
        pointsHolder = graphGen.transform.Find("PointsHolder").gameObject;
    }

    IEnumerator CreateScatterplot()
    {

        float[] minValues = GetMinValues();
        float[] maxValues = GetMaxValues();

        SetMinMax(minValues, maxValues);
        Vector3 objectPosition;
        GameObject glyph;

        int i = 0;
        foreach (DataReader.DataPoint point in dataPoints)
        {
            i++;
            // normalize each data point
            float x = (point.X - xMin) / (xMax - xMin);
            float y = (point.Y - yMin) / (yMax - yMin);
            float z = (point.Z - zMin) / (zMax - zMin);

            bool colorFlag = point.X == xMin || point.Y == yMin || point.Z == zMin || point.X == xMax || point.Y == yMax || point.Z == zMax;


            plotscale = graphGen.transform.localScale.x;
            Vector3 offsetToZeroCoordinate = new Vector3(-0.5f * plotscale, 0f, -0.5f * plotscale); //(-0.5,0,-0.5) is ZeroCordinates for plane
            objectPosition = (new Vector3(x, y, z)) * plotscale + pointsHolder.transform.position + offsetToZeroCoordinate;

            if (i % 5 == 0)
            {
                yield return new WaitForSeconds(.000000001f);
            }
            //yield return new WaitForSeconds(.000000001f);
            glyph = Instantiate(defaultGlyph, objectPosition, Quaternion.identity);
            glyph.transform.localScale *= plotscale; // Scale of GraphGen 

            glyph.name = "Data Point " + i++.ToString();
            //Debug.Log("Glyph: " + glyph.name + " " + glyph.transform.localScale);
            glyph.transform.rotation = pointsHolder.transform.rotation;
            glyph.transform.parent = pointsHolder.transform;

            glyph.GetComponent<Renderer>().material.color = (colorFlag) ? Color.white : new Color(x, y, z);

            glyphList.Add(glyph);
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
            if(point.X < minValues[0])
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

    public List<GameObject> getGlyphList()
    {
        //Debug.Log("getGlyphList: " + glyphList.Count);

        return glyphList;
    }

    public GameObject getGraphGen()
    {
        //Debug.Log("getGlyphList: " + glyphList.Count);

        return graphGen;
    }
}

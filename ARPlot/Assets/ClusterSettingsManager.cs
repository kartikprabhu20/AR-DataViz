using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClusterSettingsManager : MonoBehaviour
{

    public MeshFilter square;
    public MeshFilter sphere;
    public MeshFilter diamond;

    string clusterName;
    DataReader dataReader;
    GameObject pointsHolder;
    List<int> list;

    void Start()
    {
        clusterName = GetClusterName();
        Debug.Log("In the cluster 1 " + clusterName);

    }

    public void init(ScatterplotGenerator scatterplotGenerator)
    {
        clusterName = GetClusterName();
        Debug.Log("In the cluster " + clusterName);

        pointsHolder = GameObject.Find("PointsHolder");
        dataReader = scatterplotGenerator.GetComponent<DataReader>();

        list = dataReader.ClusterMap[clusterName];
    }

    public void ToggleClusterActivation(Toggle toggle)
    {
        foreach (int i in list)
        {
            pointsHolder.transform.GetChild(i).gameObject.SetActive(toggle.isOn);
        }
    }


    string GetClusterName()
    {
        //Debug.Log("GetClusterName " + gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>().text);
        return gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>().text;
    }

    public void OnClickSquare()
    {
        Debug.Log("OnClickSquare " + GetClusterName() + ":" + list.Count);

        foreach (int i in list)
        {
            pointsHolder.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = square.sharedMesh;
        }
    }

    public void OnClickCircle()
    {
        Debug.Log("OnClickCircle " + GetClusterName()+ ":" + list.Count);
        foreach (int i in list)
        {
            pointsHolder.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = sphere.sharedMesh;
        }
    }

  

    public void OnClickDiamond()
    {
        Debug.Log("OnClickDiamond " + GetClusterName() + ":" + list.Count);

        foreach (int i in list)
        {
            pointsHolder.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = diamond.sharedMesh;
        }
    }

}

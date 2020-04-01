using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance = null;
    private ScatterplotGenerator scatterplotGenerator;
    private GameObject pointsHolder;
    public GameObject uiCluster;
    private DataReader dataReader;
    private List<int> outlierList;


    private bool isOutlierHighlighting;

    void Start()
    {
        isOutlierHighlighting = false;
        //InitSetup();
    }

    public void initSettings()
    {
        pointsHolder = GameObject.Find("PointsHolder");
        dataReader = scatterplotGenerator.GetComponent<DataReader>();
        outlierList = dataReader.OutlierList;
        Debug.Log("OutlierList size:" + outlierList.Count);
        Debug.Log("Cluster size:" + dataReader.ClusterMap.Keys.Count);
        //Invoke("setClusterSettings", 3f);
        setClusterSettings();
    }

    public void setClusterSettings()
    {
        int i = 0;
        foreach (string key in dataReader.ClusterMap.Keys)
        {
            Debug.Log("Settingmanager Cluster:"+key);
            uiCluster = Instantiate(uiCluster, GameObject.Find("ClusterSettings").transform, false);
            uiCluster.transform.localPosition = new Vector3(0f, i++ * -70f, 0f);
            uiCluster.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = key;

            uiCluster.GetComponent<ClusterSettingsManager>().init(scatterplotGenerator);

        }
    }


    private SettingsManager() 
    {
    }

    public static SettingsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SettingsManager();
            }
            return instance;
        }
    }
    

    public void setScatterPlotGenerator(ScatterplotGenerator scatterplotGen)
    {
        scatterplotGenerator = scatterplotGen;
    }

    public ScatterplotGenerator getScatterPlotGenerator()
    {
        return scatterplotGenerator;
    }

    public void onSettingsButtonClick(Animator anim)
    {

        anim.SetBool("IsSettingsPanelDisplayed", true);
    }

    public void onSettingsCancelButtonClick(Animator anim)
    {
        anim.SetBool("IsSettingsPanelDisplayed", false);
    }

    public void onTransparencySliderChanged(Slider slider)
    {
        StartCoroutine(changeTransparency(slider));
    }

    public IEnumerator changeTransparency(Slider slider)
    {
        float value = slider.value / 10;
        //Debug.Log("onSliderChanged:" + value);
        //Debug.Log("count:" + pointsHolder.transform.childCount);

        //for (int i=0; i<pointsHolder.transform.childCount; i++)
        for (int i = 0; i < pointsHolder.transform.childCount; i++)
        {

            //Debug.Log("Slider value:" + value + " Gameobject position:" + pointsHolder.transform.GetChild(i).transform.position);
            //Debug.Log("Current value:" + pointsHolder.transform.GetChild(i).GetComponent<Renderer>().material.color);

            Renderer transparencyRenderer = pointsHolder.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            Color meshColor = transparencyRenderer.material.color;
            meshColor.a = value;
            transparencyRenderer.material.color = meshColor;
            if (i % 30 == 0)
            {
                yield return 0;
            }
        }
    }


    private Vector3 previousScale = new Vector3(0, 0, 0);

    public void onChangeScale(Slider slider)
    {
        StartCoroutine(changeScale(slider));
    }

    public IEnumerator changeScale(Slider slider)
    {
        float newScaleFactor = slider.value/100;
        Vector3 scalingVector = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor) - previousScale;
        previousScale = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor);

        //Debug.Log("onSliderChanged:" + newScaleFactor);

        for (int i = 0; i < pointsHolder.transform.childCount; i++)
        {
            //Debug.Log("Slider value:" + newScaleFactor + " Gameobject scale:" + gameObject.transform.localScale);
            pointsHolder.transform.GetChild(i).transform.localScale += scalingVector;
            if (i % 10 == 0)
            {
                yield return 0;
            }
        }
    }



    public void onOutlierToggle(Toggle toggle)
    {
        isOutlierHighlighting = toggle.isOn;
        StartCoroutine(highlightOutliers());

        if (!isOutlierHighlighting)
        {
            foreach (int i in outlierList)
            {
                pointsHolder.transform.GetChild(i).transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
            }
        }
    }

    public IEnumerator highlightOutliers()
    {
        int toggler = 0;

        while (isOutlierHighlighting)
        {
            if(toggler == 0)
            {
                outlierList = dataReader.OutlierList;

                foreach (int i in outlierList)
                {
                    pointsHolder.transform.GetChild(i).transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                }
                toggler = 1;
            }
            else
            {
                foreach (int i in outlierList)
                {
                    pointsHolder.transform.GetChild(i).transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                }
                toggler = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void onGridToggleChanged(Toggle toggle)
    {
        GameObject grids = scatterplotGenerator.getGraphGen().transform.Find("Planes").gameObject;
        grids.SetActive(toggle.isOn);
    }

}

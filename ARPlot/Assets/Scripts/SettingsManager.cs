using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance = null;
    private ScatterplotGenerator scatterplotGenerator;
    private GameObject tempGrid;

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

    //private void Start()
    //{
    //    instance = SettingsManager.Instance;
    //}

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

        int i = 0;
        foreach (var glyph in scatterplotGenerator.getGlyphList())
        {
            i++;
            //Debug.Log("Slider value:" + value + " Gameobject position:" + glyph.transform.position);
            //Debug.Log("Current value:" + glyph.GetComponentInChildren<MeshRenderer>().material.color.a);

            Renderer transparentRenderer = glyph.GetComponent<Renderer>();

            //Get current Color
            Color meshColor = transparentRenderer.material.color;
            //Set Alpha
            meshColor.a = value;
            //Apply the new color to the material
            transparentRenderer.material.color = meshColor;
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

        int i = 0;
        foreach (var glyph in scatterplotGenerator.getGlyphList())
        {
            i++;
            //Debug.Log("Slider value:" + newScaleFactor + " Gameobject scale:" + gameObject.transform.localScale);
            glyph.transform.localScale += scalingVector;
            if (i % 10 == 0)
            {
                yield return 0;
            }
        }
    }

    public void onGridToggleChanged(Toggle toggle)
    {
        GameObject grids = scatterplotGenerator.getGraphGen().transform.Find("Grid").gameObject;
        grids.SetActive(toggle.isOn);
    }

    public void onGridSliderChanged(Slider slider)
    {
        int gridSize = (int)slider.value;
        GameObject graphGen = scatterplotGenerator.getGraphGen();
        GameObject grids = graphGen.transform.Find("Grid").gameObject;
        GameObject poles = graphGen.transform.Find("Poles").gameObject;

        GameObject poleY = poles.transform.Find("PoleY").gameObject;
        GameObject poleZ = poles.transform.Find("PoleZ").gameObject;

        //Debug.Log("poleY:" + poleY.transform.position);
        //Debug.Log("poleZ:" + poleZ.transform.position);

        GameObject yParallel = grids.transform.Find("yParallel").gameObject;
        GameObject yParallel2 = grids.transform.Find("yParallel2").gameObject;

        //Debug.Log("poleY:" + yParallel.transform.position);
        //Debug.Log("poleZ:" + yParallel2.transform.position);

        GameObject xParallel = grids.transform.Find("xParallel").gameObject;
        GameObject xParallel2 = grids.transform.Find("xParallel2").gameObject;
        GameObject zParallel = grids.transform.Find("zParallel").gameObject;

        float plotscale = graphGen.transform.localScale.x;

        Destroy(tempGrid);
        tempGrid = new GameObject();
        Instantiate(tempGrid, graphGen.transform.position, Quaternion.identity);
        tempGrid.transform.rotation = grids.transform.rotation;
        tempGrid.transform.parent = grids.transform;

        for (int i = 1; i < gridSize; i++)
        {
            GameObject gridLineVertical = Instantiate(yParallel, new Vector3(yParallel.transform.position.x + i * (yParallel2.transform.position.x - yParallel.transform.position.x) / gridSize, yParallel.transform.position.y, yParallel.transform.position.z), Quaternion.identity);
            gridLineVertical.transform.localScale *= plotscale;
            gridLineVertical.transform.rotation = tempGrid.transform.rotation;
            gridLineVertical.transform.parent = tempGrid.transform;

            gridLineVertical = Instantiate(yParallel, new Vector3(yParallel.transform.position.x, yParallel.transform.position.y, yParallel.transform.position.z + i * (poleY.transform.position.z - yParallel.transform.position.z) / gridSize), Quaternion.identity);
            gridLineVertical.transform.localScale *= plotscale;
            gridLineVertical.transform.rotation = tempGrid.transform.rotation;
            gridLineVertical.transform.parent = tempGrid.transform;

            GameObject gridLineHorizontal = Instantiate(zParallel, new Vector3(zParallel.transform.position.x, zParallel.transform.position.y + i * (poleZ.transform.position.y - zParallel.transform.position.y) / gridSize, zParallel.transform.position.z), Quaternion.identity);
            gridLineHorizontal.transform.localScale *= plotscale;
            //gridLineHorizontal.transform.rotation = tempGrid.transform.rotation;
            gridLineHorizontal.transform.parent = tempGrid.transform;

            gridLineHorizontal = Instantiate(xParallel, new Vector3(xParallel.transform.position.x, xParallel.transform.position.y + i * (xParallel2.transform.position.y - xParallel.transform.position.y) / gridSize, xParallel.transform.position.z), Quaternion.identity);
            gridLineHorizontal.transform.localScale *= plotscale;
            //gridLineHorizontal.transform.rotation = tempGrid.transform.rotation;
            gridLineHorizontal.transform.parent = tempGrid.transform;
        }

    }

}

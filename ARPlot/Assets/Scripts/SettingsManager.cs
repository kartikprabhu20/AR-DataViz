using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance = null;
    private ScatterplotGenerator scatterplotGenerator;

    private SettingsManager()
    {
        //scatterplotGenerator = scatterplotGen;
        //Debug.Log("Glyph: " + scatterplotGenerator == null);
        //Debug.Log("Glyph: " + scatterplotGenerator.getGlyphList().Count);

        //Debug.Log("Glyph: " + scatterplotGenerator.getGlyphList() == null);

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
        Debug.Log("setScatterPlotGenerator: " + scatterplotGenerator.getGlyphList().Count);
        Debug.Log("setScatterPlotGenerator: " + scatterplotGenerator == null);
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

    public void onSliderChanged(Slider slider)
    {
        float value = slider.value/10;
        Debug.Log("onSliderChanged:" + value);

        foreach (var glyph in scatterplotGenerator.getGlyphList())
        {
            Debug.Log("Slider value:" + value + " Gameobject position:" + glyph.transform.position);


            Debug.Log("Current value:" + glyph.GetComponentInChildren<MeshRenderer>().material.color.a);

            Renderer transparentRenderer = glyph.GetComponent<Renderer>();

            //Get current Color
            Color meshColor = transparentRenderer.material.color;
            //Set Alpha
            meshColor.a = value;
            //Apply the new color to the material
            transparentRenderer.material.color = meshColor;

            //var col = gameObject.GetComponentInChildren<MeshRenderer>().material.color;
            //col.a = value;
        }
    }


   
    private Vector3 previousScale = new Vector3(0,0,0);

    public void onChangeScale(Slider slider)
    {

        float newScaleFactor = slider.value/100;
        Vector3 scalingVector = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor) - previousScale;
        previousScale = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor);

        Debug.Log("onSliderChanged:" + newScaleFactor);

        foreach (var glyph in scatterplotGenerator.getGlyphList())
        {
            Debug.Log("Slider value:" + newScaleFactor + " Gameobject scale:" + gameObject.transform.localScale);

            glyph.transform.localScale += scalingVector;
        }
    }
}

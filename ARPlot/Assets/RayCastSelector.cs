using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RayCastSelector : MonoBehaviour
{
    public float rayCasteRange = 50f;                                   // Distance in Unity units over which the player can fire
    public Transform rayShooterPosition;                                // Holds a reference to the end of ray shooter, marking the muzzle location of the shooter
    public Camera fpsCam;                                               // Holds a reference to the first person camera
    //public GameObject plotObject;

    
    private LineRenderer laserLine;                                     // Reference to the LineRenderer component which will display our laserline
    private Color previousGameObjectColor;
    private GameObject previousGameObject;

    void Start()
    {
        // Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer>();
        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        // Create a vector at the center of our camera's viewport
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));


        if (laserLine.enabled)
        {
            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, rayShooterPosition.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, rayCasteRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);

                //Debug.Log("color1:" + previousGameObjectColor);

                if (previousGameObject != hit.collider.gameObject)
                {
                    resetGameObject();
                    previousGameObject = hit.collider.gameObject;
                    previousGameObjectColor = previousGameObject.GetComponent<MeshRenderer>().material.color;
                }
                previousGameObject.GetComponent<MeshRenderer>().material.color = new Color(230, 224, 209);
                //Debug.Log("color2:"+previousGameObjectColor);
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * rayCasteRange));
                resetGameObject();

            }
        }
        else
        {    
           //Reset gameobject
           resetGameObject();
        }

    }

    private void resetGameObject()
    {
        if(previousGameObject != null)
            previousGameObject.GetComponent<MeshRenderer>().material.color = previousGameObjectColor;
    }

    public void OnPointerDown()
    {
        //plotObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

        laserLine.enabled = true;
    }

    public void OnPointerUp()
    {
        laserLine.enabled = false;
    }
}

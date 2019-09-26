using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public TextMeshProUGUI text;

    private ARRaycastManager arRaycast;
    private Pose placementPose;
    private Vector3 screenCenter;
    private bool placementPoseValid = false;

    void Start()
    {
        arRaycast = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacePose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacePose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f,0));
        var hits = new List<ARRaycastHit>();
        arRaycast.Raycast(screenCenter, hits);
        text.text = "Perform raycast";

  
        placementPoseValid = hits.Count > 0;
        //if (change) {
        //    text.text = "hits.Count = " + hits.Count;
        //}
        if (placementPoseValid)
        {
            placementPose = hits[0].pose;
            text.text = "True";

            var cameraFoward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraFoward.x, 0, cameraFoward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }
}

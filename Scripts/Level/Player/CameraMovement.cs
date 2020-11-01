using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraMovement : MonoBehaviour
{

    
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float zoomSensitivity = 10f;
    [SerializeField] private float moveSensitivity = 10f;
    [SerializeField] private float range;
    public Camera GameCamera;
    public GameObject CenterOfView = null;

    [Range(0 , 1)] private float angle;
    private int cameraViewSideIndex;
    private Vector2 firstScreenTouh;
    private Vector2 screenSize = new Vector2(Screen.width, Screen.height);
    private void Start()
    {
        firstScreenTouh = -Vector2.zero;
        CenterOfView.transform.eulerAngles = new Vector3(45, 360f * angle, 0);
        transform.position = (-CenterOfView.transform.forward * range) + CenterOfView.transform.position;
        transform.LookAt(CenterOfView.transform);
    }
    
    public float GetRange()
    {
        return range;
    }
    public float GetMoveSensitivity()
    {
        return moveSensitivity;
    }
    public float GetZoomSensitivity()
    {
        return zoomSensitivity;
    }
    public float GetSensitivity()
    {
        return sensitivity;
    }
    public void AddToNormalizedAngle(float value)
    {
        angle += value;

        if (angle > 1)
        {
            int wholePart = (int)angle;
            angle = angle - wholePart;
        }

        if (angle < 0) 
        {
            int wholePart = (int)angle;
            angle = 1 + (angle + wholePart);
        }
    }
    public void UpdateCameraTransform()
    {
        CenterOfView.transform.eulerAngles = new Vector3(45, 360f * angle, 0);
        transform.position = (-CenterOfView.transform.forward * range) + CenterOfView.transform.position;       
        transform.LookAt(CenterOfView.transform);  
        GameCamera.orthographicSize = range;
    }
     public void AddRange(float _value)
    {
         range += _value;
        if(range < 0.01f)
        {
            range = 0.01f;
        }
        if(range > 30f)
        {
            range = 30f;
        }
       
    }

  
  


}

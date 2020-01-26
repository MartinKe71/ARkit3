/*
    created by Jiadong Chen

    email: chenjd1024@gmail.com
 */

using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARFoundationObjectDetectDemo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    [SerializeField]
    Camera m_Cam;

    [SerializeField]
    ObjectDetector m_ObjectDetector;

    [SerializeField]
    GameObject m_TargetGo;

    public GameObject targetGo 
    {
        get => m_TargetGo;
        set => m_TargetGo = value;
    }

    public GameObject m_Go;

    [SerializeField]
    RawImage m_RawImage;

    public Material m_BlitMat;

    private Texture2D m_TexFromNative = null;
    public Texture2D textureFromNative
    {
        get => m_TexFromNative;
        set => m_TexFromNative = value;
    }


    void OnEnable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += OnCameraFrameReceived;
        }

        if(m_ObjectDetector != null)
        {
            m_ObjectDetector.OnObjectDeteced += OnObjectDetectorCompleted;
        }
    }

    void OnDisable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= OnCameraFrameReceived;
        }

        if(m_ObjectDetector != null)
        {
            m_ObjectDetector.OnObjectDeteced -= OnObjectDetectorCompleted;
        }
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {

#if !UNITY_EDITOR && UNITY_IOS

        var cameraParams = new XRCameraParams
        {
            zNear = m_Cam.nearClipPlane,
            zFar = m_Cam.farClipPlane,
            screenWidth = Screen.width,
            screenHeight = Screen.height,
            screenOrientation = Screen.orientation
        };

        XRCameraFrame frame;
        if (cameraManager.subsystem.TryGetLatestFrame(cameraParams, out frame))
        {
            if (m_ObjectDetector.IsIdle)
            {
                m_ObjectDetector.StartDetect(frame.nativePtr);
            }
        }

#endif
    }

    private void OnObjectDetectorCompleted(object sender, Vector2 pos)
    {
       var objectPos = new Vector3();
       objectPos.x = pos.x;
       objectPos.y = 1 - pos.y;
       objectPos.z = 5;//m_Cam.nearClipPlane;
       var objectWorldPos = m_Cam.ViewportToWorldPoint(objectPos);

       IntPtr texHandler = m_ObjectDetector.GetDetectedTexture();

       if(texHandler == IntPtr.Zero)
       {
           return;
       }

       if(m_TexFromNative == null)
       {
           m_TexFromNative = Texture2D.CreateExternalTexture(128, 128, TextureFormat.ARGB32, false, false, texHandler);
       }

       m_TexFromNative.UpdateExternalTexture(texHandler);

    //    m_RawImage.texture = m_TexFromNative;

       return; 
        

       if(m_Go == null)
       {
          m_Go = Instantiate(m_TargetGo, objectWorldPos, Quaternion.identity);
       }

       m_Go.transform.position = objectWorldPos;
       m_Go.transform.LookAt(m_Cam.transform);
    }


}

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    enum DetectionStatus
    {
        None = 0,
        Detecting = 1,
        Detected = 2
    }

    #region Native Bindings

    [DllImport("__Internal")]
    private static extern bool ObjectDetector_StartDetect(IntPtr buffer);

    [DllImport("__Internal")]
    private static extern IntPtr ObjectDetector_GetDetectedTexture();

    #endregion

    #region Fields

    private DetectionStatus m_DetectionStatus = DetectionStatus.None;

    #endregion

    #region Callback

    public event EventHandler<Vector2> OnObjectDeteced;

    #endregion

    #region Methods

    public void StartDetect(IntPtr buffer)
    {
        if (buffer == IntPtr.Zero)
        {
            Debug.LogError("buffer is NULL!");
            return;
        }

        bool success = ObjectDetector_StartDetect(buffer);

        if (success)
        {
            m_DetectionStatus = DetectionStatus.Detecting;
        }
    }

    public IntPtr GetDetectedTexture()
    {
        return ObjectDetector_GetDetectedTexture();
    }

    private void OnObjectDetecedFromNative(string data)
    {
        m_DetectionStatus = DetectionStatus.Detected;
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        string[] temp = data.Split(',');

        if (temp.Length != 2)
        {
            return;
        }

        float x, y = 0.0f;

        if (!Single.TryParse(temp[0], out x) || !Single.TryParse(temp[1], out y))
        {
            return;
        }

        Vector2 vPortSpacePos = new Vector2(x, y);

        if (OnObjectDeteced != null)
        {
            OnObjectDeteced(this, vPortSpacePos);
        }
    }

    #endregion


    #region Properties

    public bool IsIdle
    {
        get => m_DetectionStatus != DetectionStatus.Detecting;
    }

    #endregion
}

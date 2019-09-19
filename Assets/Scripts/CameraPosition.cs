using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public TextMeshProUGUI cameraPos;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cameraPos.text = "(" + gameObject.transform.position.x + ", " + gameObject.transform.position.y + ", " + gameObject.transform.position.z + ")";
    }
}

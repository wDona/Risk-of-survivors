using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera[] cameras;
    private int current = 0;

    void Start()
    {
        // activa solo la primera
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[current].gameObject.SetActive(false);
            current = (current + 1) % cameras.Length;
            cameras[current].gameObject.SetActive(true);
        }
    }
}

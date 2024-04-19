using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBoxer : MonoBehaviour
{    
    public enum ReferenceMode { DesignedAspectRatio, OrginalResolution };

    public Color matteColor = new Color(0, 0, 0, 1);
    public ReferenceMode referenceMode; 
    public float x = 16;
    public float y = 9;  
    public float width = 960;
    public float height = 540;
    public bool onAwake = true;
    public bool onUpdate = true;

    private Camera camera;
    private Camera letterBoxerCamera;

    public void Awake()
    {
        // Reference the main camera 
        camera = GetComponent<Camera>();
        AddLetterBoxingCamera();

        // Initially add letter box
        if (onAwake)
            PerformSizing();
    }

    public void Update()
    {
        // Automatically update the letter box size based on the settled resolution
        if (onUpdate)
            PerformSizing();
    }

    private void OnValidate()
    {
        x = Mathf.Max(1, x);
        y = Mathf.Max(1, y);
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
    }

    private void AddLetterBoxingCamera()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera camera in allCameras)
        {             
            // Check if the depth of camera is equal to -100
            if (camera.depth == -100)
            {
                Debug.LogError("Set " + camera.name + "'s depth higher than -100");
            }
        }

        // Set the letter boxer camera
        letterBoxerCamera = new GameObject().AddComponent<Camera>();
        letterBoxerCamera.backgroundColor = matteColor;
        letterBoxerCamera.cullingMask = 0;
        letterBoxerCamera.depth = -100;
        letterBoxerCamera.farClipPlane = 1;
        letterBoxerCamera.useOcclusionCulling = false;
        letterBoxerCamera.allowHDR = false;
        letterBoxerCamera.allowMSAA = false;
        letterBoxerCamera.clearFlags = CameraClearFlags.Color;
        letterBoxerCamera.name = "LetterBox";        
    }

    private void PerformSizing()
    {
        float targetRatio = x / y;

        if (referenceMode == LetterBoxer.ReferenceMode.OrginalResolution)
            targetRatio = width / height;

        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetRatio;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}

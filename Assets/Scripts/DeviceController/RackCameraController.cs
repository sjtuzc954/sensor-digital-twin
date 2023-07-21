using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackCameraController : MonoBehaviour
{
    Camera Rackcamera;
    // Start is called before the first frame update
    void Start()
    {
        Rackcamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Texture2D CameraCapture()
    {
        RenderTexture rt = new RenderTexture(160, 120, 16);
        Rackcamera.targetTexture = rt;
        Rackcamera.Render();
        RenderTexture.active = rt;
        Texture2D t = new Texture2D(160, 120);
        t.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0);
        t.Apply();
        return t;
    }

}

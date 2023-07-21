using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotionController : MonoBehaviour
{
    public Transform CapturePos;
    public Transform HomePos;
    public float speed = 10.0f;

    private Vector3 PositiveDirection;
    // Start is called before the first frame update
    void Start()
    {
        PositiveDirection = (CapturePos.position - HomePos.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Capture()
    {
        Vector3 capture2this = CapturePos.transform.position - this.transform.position;
        if (capture2this.x * PositiveDirection.x >= 0 && capture2this.y * PositiveDirection.y >= 0 && capture2this.z * PositiveDirection.z >= 0)
        {
            this.transform.position += PositiveDirection * speed * Time.deltaTime;
        }
    }

    public void Home()
    {
        Vector3 this2home = this.transform.position - HomePos.transform.position;
        if (this2home.x * PositiveDirection.x >= 0 && this2home.y * PositiveDirection.y >= 0 && this2home.z * PositiveDirection.z >= 0)
        {
            this.transform.position -= PositiveDirection * speed * Time.deltaTime;
        }
    }
}

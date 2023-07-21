using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureController : MonoBehaviour
{
    public Transform PressPos;
    public Transform OriginalPos;
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressure()
    {
        if (this.transform.position.y > PressPos.position.y)
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    public void Home()
    {
        if (this.transform.position.y < OriginalPos.position.y)
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
}

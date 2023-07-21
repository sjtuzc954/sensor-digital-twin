using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyBeltMove : MonoBehaviour
{
    public bool belton = false;
    public Transform startpoint;
    public Transform endpoint;
    public float speed = 1f;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.Normalize(endpoint.transform.position - startpoint.transform.position);
    }

    // Update is called once per frame
    // 处理刚体要使用FixedUpdate替代Update
    void FixedUpdate()
    {
        belton = GameManager.isStart;

        if (belton)
        {
            Vector3 pos = this.GetComponent<Rigidbody>().position;
            Vector3 temp = -direction * speed * Time.deltaTime;
            this.GetComponent<Rigidbody>().position += temp;
            this.GetComponent<Rigidbody>().MovePosition(pos);
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerController : MonoBehaviour
{
    public Transform Block1;
    public Transform Block2;
    public Transform bottom;
    public bool auto = false;

    private float height;
    private bool autoStart = false;
    private bool block = true;
    
    System.Timers.Timer t = new System.Timers.Timer(500);
    System.Timers.Timer t2 = new System.Timers.Timer(2000);
    // Start is called before the first frame update
    void Start()
    {
       
        height = Block1.position.y;
        t.Elapsed += new System.Timers.ElapsedEventHandler(AutoDown); //到达时间的时候执行事件； 
        t.AutoReset = false;
        t2.Elapsed += new System.Timers.ElapsedEventHandler(AutoUp); //到达时间的时候执行事件； 
        t2.AutoReset = false;
        
    }

    public void AutoDown(object source, System.Timers.ElapsedEventArgs e)
    {
        block = false;
        t2.Start();
    }
    public void AutoUp(object source, System.Timers.ElapsedEventArgs e)
    {
        block = true;
        autoStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (auto && block) Up();
        else if (auto && !block) Down();
    }

    public void Up()
    {
        if (Block1.position.y < height)
        {
            Block1.position += new Vector3(0, 1, 0) * 10f * Time.deltaTime;
            Block2.position += new Vector3(0, 1, 0) * 10f * Time.deltaTime;
        }
    }

    public void Down()
    {
        if (Block1.position.y > bottom.position.y - 0.3f)
        {
            Block1.position += new Vector3(0, -1, 0) * 10f * Time.deltaTime;
            Block2.position += new Vector3(0, -1, 0) * 10f * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Vehicle")
        {
            collision.gameObject.GetComponent<VehicleController>().isBlock = true;
            if (auto && !autoStart) {
	autoStart = true;
	t.Start();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Vehicle")
        {
            collision.gameObject.GetComponent<VehicleController>().isBlock = false;
        }
    }
}

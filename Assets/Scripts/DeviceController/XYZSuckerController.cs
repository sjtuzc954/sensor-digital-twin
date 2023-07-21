using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZSuckerController : MonoBehaviour
{
    public float Slippermovespeed = 10.0f;
    public float Ymovespeed = 1.0f;
    public Transform YAxis;

    /*public Transform xMax;
    public Transform xMin;
    public Transform yMax;
    public Transform yMin;
    public Transform zMax;
    public Transform zMin;*/

    public Transform SuckMark;
    public Transform CheckMark;
    public Transform PutMark;
    public Transform HomeMark;

    private float targetx;
    private float targety;
    private float targetz;
    private float positionoffset = 0.5f;
    private float resety;

    private int suckSequential = -1;
    private int checkSequential = -1;
    private int putSequential = -1;
    private int resetSequential = -1;

    System.Timers.Timer t = new System.Timers.Timer(1500);   //实例化Timer类，设置间隔时间为1500毫秒;
    System.Timers.Timer t2 = new System.Timers.Timer(1500);
    System.Timers.Timer t3 = new System.Timers.Timer(1500);

    // Start is called before the first frame update
    void Start()
    {
        targetx = this.transform.position.x;
        targety = this.transform.position.y;
        targetz = this.transform.position.z;
        resety = targety;

        t.Elapsed += new System.Timers.ElapsedEventHandler(SuckOver); //到达时间的时候执行事件；   
        t.AutoReset = false;
        t2.Elapsed += new System.Timers.ElapsedEventHandler(CheckOver); //到达时间的时候执行事件；   
        t2.AutoReset = false;
        t3.Elapsed += new System.Timers.ElapsedEventHandler(PutOver); //到达时间的时候执行事件；   
        t3.AutoReset = false;
    }

    public void SuckOver(object source, System.Timers.ElapsedEventArgs e)
    {
        suckSequential = 2;
    }
    public void CheckOver(object source, System.Timers.ElapsedEventArgs e)
    {
        checkSequential = -1;
        StartPut();
    }
    public void PutOver(object source, System.Timers.ElapsedEventArgs e)
    {
        putSequential = -1;
        Reset();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (suckSequential >= 0) GotoSuck();
        if (checkSequential >= 0) GotoCheck();
        if (putSequential >= 0) GotoPut();
        if (resetSequential >= 0) GotoReset();

        if (this.transform.position.z > targetz + positionoffset)
        {
            this.transform.position += Vector3.back * Slippermovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.back * Slippermovespeed * Time.deltaTime;
        }
        else if (this.transform.position.z < targetz - positionoffset)
        {
            this.transform.position += Vector3.forward * Slippermovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.forward * Slippermovespeed * Time.deltaTime;
        }
        else if (this.transform.position.x > targetx + positionoffset)
        {
            this.transform.position += Vector3.left * Slippermovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.left * Slippermovespeed * Time.deltaTime;
        }
        else if (this.transform.position.x < targetx - positionoffset)
        {
            this.transform.position += Vector3.right * Slippermovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.right * Slippermovespeed * Time.deltaTime;
        }
        else if (this.transform.position.y > targety + positionoffset)
        {
            this.transform.position += Vector3.down * Ymovespeed * Time.deltaTime;
        }
        else if (this.transform.position.y < targety - positionoffset)
        {
            this.transform.position += Vector3.up * Ymovespeed * Time.deltaTime;
        }
    }

    public void SetTargetPosition(float x, float y, float z)
    {
        targetx = x;
        targety = y;
        targetz = z;
        /*Mathf.Clamp(targetx, xMin.position.x, xMax.position.x);
        Mathf.Clamp(targety, yMin.position.y, yMax.position.y);
        Mathf.Clamp(targetz, zMin.position.z, zMax.position.z);*/
    }

    public void SetTargetPosition(Transform transform)
    {
        SetTargetPosition(transform.position.x, transform.position.y, transform.position.z);
    }

    public void StartSuck()
    {
        suckSequential = 0;
        if (checkSequential != -1) checkSequential = -1;
        if (putSequential != -1) putSequential = -1;
        if (resetSequential != -1) resetSequential = -1;
    }

    public void StartCheck()
    {
        checkSequential = 0;
        if (suckSequential != -1) suckSequential = -1;
        if (putSequential != -1) putSequential = -1;
        if (resetSequential != -1) resetSequential = -1;
    }

    public void StartPut()
    {
        putSequential = 0;
        if (suckSequential != -1) suckSequential = -1;
        if (checkSequential != -1) checkSequential = -1;
        if (resetSequential != -1) resetSequential = -1;
    }

    public void Reset()
    {
        resetSequential = 0;
        if (suckSequential != -1) suckSequential = -1;
        if (checkSequential != -1) checkSequential = -1;
        if (putSequential != -1) putSequential = -1;
    }

    private bool CheckPosition()
    {
        return (this.transform.position.x <= targetx + positionoffset && this.transform.position.x >= targetx - positionoffset &&
                this.transform.position.y <= targety + positionoffset && this.transform.position.y >= targety - positionoffset &&
                this.transform.position.z <= targetz + positionoffset && this.transform.position.z >= targetz - positionoffset);
    }

    private void GotoSuck()
    {
        if (suckSequential == 0)
        {
            SetTargetPosition(SuckMark.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) suckSequential = 1;
        }
        else if (suckSequential == 1)
        {
            SetTargetPosition(SuckMark);
            if (CheckPosition())
            {
                t.Start();
            }
        }
        else if (suckSequential == 2)
        {
            SetTargetPosition(this.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) suckSequential = -1;
        }
    }

    private void GotoCheck()
    {
        if (checkSequential == 0)
        {
            SetTargetPosition(this.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) checkSequential = 1;
        }
        else if (checkSequential == 1)
        {
            SetTargetPosition(CheckMark);
            if (CheckPosition())
            {
                t2.Start();
            }
        }
    }

    private void GotoPut()
    {
        if (putSequential == 0)
        {
            SetTargetPosition(this.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) putSequential = 1;
        }
        else if (putSequential == 1)
        {
            SetTargetPosition(PutMark);
            if (CheckPosition())
            {
                t3.Start();
            }
        }
    }

    private void GotoReset()
    {
        if (resetSequential == 0)
        {
            SetTargetPosition(this.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) resetSequential = 1;
        }
        else if (resetSequential == 1)
        {
            SetTargetPosition(HomeMark);
            if (CheckPosition()) resetSequential = -1;
        }
    }

    public int CheckState()
    {
        int state = 0;
        if (suckSequential != -1)
        {
            state = 1;
        }
        else if (checkSequential != -1)
        {
            state = 2;
        }
        else if (putSequential != -1)
        {
            state = 3;
        }
        else if (resetSequential != -1)
        {
            state = 4;
        }
        
        return state;
    }
}

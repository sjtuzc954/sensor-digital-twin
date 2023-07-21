using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZRobotArmController : MonoBehaviour
{
    [Header("滑道参数")]
    public float movespeed = 10.0f;
    public float Ymovespeed = 10.0f;
    public Transform YAxis;
    public Transform ZAxis;

    /*public Transform xMax;
    public Transform xMin;
    public Transform yMax;
    public Transform yMin;
    public Transform zMax;
    public Transform zMin;*/

    public Transform GrabMark;
    public Transform PutMark;
    public Transform HomeMark;

    [Header("旋转部件参数")]
    public float rotatespeed = 100.0f;
    public Vector3 rotateDirection;
    public bool isRack5 = false;
    public Transform gripperPart;
    public Vector3 gripperRotateDirection;

    [Header("夹爪参数")]
    public Transform LeftGripper;
    public Transform RightGripper;
    public float gripperspeed = 0.01f;

    private float targetx;
    private float targety;
    private float targetz;
    private float positionoffset = 0.5f;
    private float resety;

    private bool isGrabing = false;
    private bool isRotating = false;
    private bool isReallyGrabingLeft = false;
    private bool isReallyGrabingRight = false;
    private GameObject GrabedObject;
    private bool RobotArmRotate; // false 初始角度， true 旋转90度
    private bool GripperRotate; // false 初始角度， true 旋转90度

    private int grabingSequential = -1;
    private int putSequential = -1;
    private int resetSequential = -1;

    private float currentAngle1;
    private float currentAngle2;
    // private float angleoffset = 1f;
    // Start is called before the first frame update

System.Timers.Timer t = new System.Timers.Timer(500);   //实例化Timer类，设置间隔时间为1500毫秒;
    System.Timers.Timer t2 = new System.Timers.Timer(1500);
    void Start()
    {
        GripperRotate = true;
        RobotArmRotate = false;
        targetx = this.transform.position.x;
        targety = this.transform.position.y;
        targetz = this.transform.position.z;
        resety = targety;
        currentAngle1 = 0;
        currentAngle2 = 0;
 t.Elapsed += new System.Timers.ElapsedEventHandler(GrabOver); //到达时间的时候执行事件；   
        t.AutoReset = false;
        t2.Elapsed += new System.Timers.ElapsedEventHandler(PutOver); //到达时间的时候执行事件；   
        t2.AutoReset = false;
    }
public void GrabOver(object source, System.Timers.ElapsedEventArgs e)
    {
        StartPut();
    }
    public void PutOver(object source, System.Timers.ElapsedEventArgs e)
    {
        Reset();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (grabingSequential >= 0) GotoGrab();
        if (putSequential >= 0) GotoPut();
        if (resetSequential >= 0) GotoReset();
        
        if (this.transform.position.z > targetz + positionoffset)
        {
            this.transform.position += Vector3.back * movespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.back * movespeed * Time.deltaTime;
        }
        else if (this.transform.position.z < targetz - positionoffset)
        {
            this.transform.position += Vector3.forward * movespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.forward * movespeed * Time.deltaTime;
        }
        else if (this.transform.position.x > targetx + positionoffset)
        {
            this.transform.position += Vector3.left * movespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.left * movespeed * Time.deltaTime;
            if (ZAxis != null) ZAxis.transform.position += Vector3.left * movespeed * Time.deltaTime;
        }
        else if (this.transform.position.x < targetx - positionoffset)
        {
            this.transform.position += Vector3.right * movespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.right * movespeed * Time.deltaTime;
            if (ZAxis != null) ZAxis.transform.position += Vector3.right * movespeed * Time.deltaTime;
        }
        else if (this.transform.position.y > targety + positionoffset)
        {
            this.transform.position += Vector3.down * Ymovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.down * Ymovespeed * Time.deltaTime;
        }
        else if (this.transform.position.y < targety - positionoffset)
        {
            this.transform.position += Vector3.up * Ymovespeed * Time.deltaTime;
            if (YAxis != null) YAxis.transform.position += Vector3.up * Ymovespeed * Time.deltaTime;
        }

        isRotating = false;
        if (RobotArmRotate)
        {
            if (currentAngle1 < 90.0f)
            {
                this.transform.Rotate(rotateDirection * Time.deltaTime * rotatespeed);
                currentAngle1 += Time.deltaTime * rotatespeed;
                isRotating = true;
            }
        }
        else
        {
            if (currentAngle1 > 0)
            {
                isRotating = true;
                this.transform.Rotate(-rotateDirection * Time.deltaTime * rotatespeed);
                currentAngle1 -= Time.deltaTime * rotatespeed;
            }
        }

        if (isRack5)
        {
            if (GripperRotate)
            {
                if (currentAngle2 < 90.0f)
                {
                    gripperPart.transform.Rotate(gripperRotateDirection * Time.deltaTime * rotatespeed);
                    currentAngle2 += Time.deltaTime * rotatespeed;
                }
            }
            else
            {
                if (currentAngle2 > 0)
                {
                    gripperPart.transform.Rotate(-gripperRotateDirection * Time.deltaTime * rotatespeed);
                    currentAngle2 -= Time.deltaTime * rotatespeed;
                }
            }
        }

        if (isGrabing)
        {
            if (!isReallyGrabingLeft)
            {
                if (LeftGripper.transform.localPosition.z < -0.005f)
                {
                    LeftGripper.transform.localPosition += Vector3.forward * gripperspeed * Time.deltaTime;
                }
            }
            if (!isReallyGrabingRight)
            {
                if (RightGripper.transform.localPosition.z > 0.005f)
                {
                    RightGripper.transform.localPosition += Vector3.back * gripperspeed * Time.deltaTime;
                }
            }
            if (isReallyGrabingLeft && isReallyGrabingRight)
            {
                if (GrabedObject != null)
                {
                    GrabedObject.GetComponent<Rigidbody>().isKinematic = true;
                    GrabedObject.transform.parent = LeftGripper;
                }
            }
        }
        else
        {
            if (LeftGripper.transform.localPosition.z > -0.03f)
            {
                LeftGripper.transform.localPosition += Vector3.back * gripperspeed * Time.deltaTime;
            }
            if (RightGripper.transform.localPosition.z < 0.03f)
            {
                RightGripper.transform.localPosition += Vector3.forward * gripperspeed * Time.deltaTime;
            }
            isReallyGrabingLeft = isReallyGrabingRight = false;
            if (GrabedObject != null)
            {
                GrabedObject.GetComponent<Rigidbody>().isKinematic = false;
                GrabedObject.transform.parent = null;
                GrabedObject = null;
            }
        }
    }

    public void SetIsReallyGrabingLeft()
    {
        isReallyGrabingLeft = true;
    }

    public void SetIsReallyGrabingRight()
    {
        isReallyGrabingRight = true;
    }

    public void SetGrabedObject(GameObject gameObject)
    {
        GrabedObject = gameObject;
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

    public void StartGrab()
    {
        grabingSequential = 0;
        if (putSequential != -1) putSequential = -1;
        if (resetSequential != -1) resetSequential = -1;
    }

    public void StartPut()
    {
        putSequential = 0;
        if (grabingSequential != -1) grabingSequential = -1;
        if (resetSequential != -1) resetSequential = -1;
    }

    public void Reset()
    {
        resetSequential = 0;
        if (grabingSequential != -1) grabingSequential = -1;
        if (putSequential != -1) putSequential = -1;
    }

    private bool CheckPosition()
    {
        return (this.transform.position.x <= targetx + positionoffset && this.transform.position.x >= targetx - positionoffset &&
                this.transform.position.y <= targety + positionoffset && this.transform.position.y >= targety - positionoffset &&
                this.transform.position.z <= targetz + positionoffset && this.transform.position.z >= targetz - positionoffset);
    }

    private void GotoGrab()
    {
        if (grabingSequential == 0)
        {
            SetTargetPosition(this.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) grabingSequential = 1;
        }
        else if (grabingSequential == 1)
        {
            GripperRotate = true;
            RobotArmRotate = false;
            grabingSequential = 2;
        }
        else if (grabingSequential == 2)
        {
            SetTargetPosition(GrabMark);
            if (CheckPosition()) grabingSequential = 3;
        }
        else if (grabingSequential == 3)
        {
            if (isRotating) return;
            isGrabing = true;
            if (isReallyGrabingLeft && isReallyGrabingRight) {
	t.Start();
	grabingSequential = -1;
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
            GripperRotate = false;
            RobotArmRotate = true;
            putSequential = 2;
      
        }
        else if (putSequential == 2)
        {
            SetTargetPosition(PutMark);
            if (CheckPosition()) putSequential = 3;
        }
        else if (putSequential == 3)
        {
            if (isRotating) return;
            isGrabing = false;
            t2.Start();
            putSequential = -1;
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
            GripperRotate = true;
            RobotArmRotate = false;
            resetSequential = 2;

        }
        else if (resetSequential == 2)
        {
            SetTargetPosition(HomeMark.transform.position.x, resety, this.transform.position.z);
            if (CheckPosition()) resetSequential = 3;
        }
        else if (resetSequential == 3)
        {
            SetTargetPosition(HomeMark);
            if (CheckPosition()) resetSequential = 4;
        }
        else if (resetSequential == 4)
        {
            if (isRotating) return;
            isGrabing = false;
            resetSequential = -1;
        }
    }

    public int CheckState()
    {
        int state = 0;
        if (grabingSequential != -1)
        {
            state = 1;
        }
        else if (putSequential != -1)
        {
            state = 2;
        }
        else if (resetSequential != -1)
        {
            state = 3;
        }

        return state;
    }
}

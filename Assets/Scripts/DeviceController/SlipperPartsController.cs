using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperPartsController : MonoBehaviour
{
    public float slipperspeed = 10.0f;
    public Transform SlipperLeftMark;
    public Transform SlipperRightMark;
    public Transform RobotArm;
    public Transform SlipperGrabMark;
    public Transform SlipperPutMark;
    public Transform SlipperHomeMark;

    private Vector3 SlipperRLDirection;
    private int slipperState;// 0:Still, 1:Left, 2:Right

    private int grabingSequential = -1;
    private Transform tempTransform;
    private int putSequential = -1;
    private int resetSequential = -1;

    // Start is called before the first frame update
    void Start()
    {
        SlipperRLDirection = (SlipperLeftMark.position - SlipperRightMark.position).normalized;
        slipperState = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slipperState == 1) SlipLeft();
        if (slipperState == 2) SlipRight();
        if (grabingSequential >= 0) GotoGrab();
        if (putSequential >= 0) GotoPut();
        if (resetSequential >= 0) GotoReset();
    }

    public void SetSlipperState(int state)
    {
        slipperState = state;
    }

    private void SlipLeft()
    {
        Vector3 left2this = SlipperLeftMark.transform.position - this.transform.position;
        if (left2this.x * SlipperRLDirection.x >= 0 && left2this.y * SlipperRLDirection.y >= 0 && left2this.z * SlipperRLDirection.z >= 0)
        {
            this.transform.position += SlipperRLDirection * slipperspeed * Time.deltaTime;
            RobotArm.transform.position += SlipperRLDirection * slipperspeed * Time.deltaTime;
        }
        else
        {
            slipperState = 0;
        }
    }

    private void SlipRight()
    {
        Vector3 this2right = this.transform.position - SlipperRightMark.transform.position;
        if (this2right.x * SlipperRLDirection.x >= 0 && this2right.y * SlipperRLDirection.y >= 0 && this2right.z * SlipperRLDirection.z >= 0)
        {
            this.transform.position += -SlipperRLDirection * slipperspeed * Time.deltaTime;
            RobotArm.transform.position += -SlipperRLDirection * slipperspeed * Time.deltaTime;
        }
        else
        {
            slipperState = 0;
        }
    }

    public int GetGrabingSequential()
    {
        return grabingSequential;
    }

    public void StartGrab()
    {
        grabingSequential = 0;
    }

    public void OverGrab()
    {
        grabingSequential = -1;
    }

    private void GotoGrab()
    {
        if (grabingSequential == 0 || grabingSequential == 1) Slip2Target(SlipperGrabMark, ref grabingSequential);
    }

    public int GetPutSequential()
    {
        return putSequential;
    }

    public void StartPut()
    {
        putSequential = 0;
    }

    public void OverPut()
    {
        putSequential = -1;
    }

    private void GotoPut()
    {
        if (putSequential == 0 || putSequential == 1) Slip2Target(SlipperPutMark, ref putSequential);
    }

    public int GetResetSequential()
    {
        return resetSequential;
    }

    public void StartReset()
    {
        resetSequential = 0;
    }

    public void OverReset()
    {
        resetSequential = -1;
    }

    private void GotoReset()
    {
        if (resetSequential == 0 || resetSequential == 1) Slip2Target(SlipperHomeMark, ref resetSequential);
    }

    private void Slip2Target(Transform targetTransform, ref int sequential)
    {
        if (sequential == 0)
        {
            tempTransform = targetTransform;
            Vector3 target2this = targetTransform.position - this.transform.position;
            sequential = 1;
            if (target2this.x * SlipperRLDirection.x >= 0 && target2this.y * SlipperRLDirection.y >= 0 && target2this.z * SlipperRLDirection.z >= 0)
            {
                slipperState = 1;
            }
            else slipperState = 2;
        }
        else if (sequential == 1)
        {
            Vector3 target2this = tempTransform.position - this.transform.position;
            if (slipperState == 1 && target2this.x * SlipperRLDirection.x <= 0 && target2this.y * SlipperRLDirection.y <= 0 && target2this.z * SlipperRLDirection.z <= 0)
            {
                slipperState = 0;
                sequential = 2;
            }
            if (slipperState == 2 && target2this.x * SlipperRLDirection.x >= 0 && target2this.y * SlipperRLDirection.y >= 0 && target2this.z * SlipperRLDirection.z >= 0)
            {
                slipperState = 0;
                sequential = 2;
            }
        }
    }
}

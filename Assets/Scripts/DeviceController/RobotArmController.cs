using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{
    [Header("滑道参数")]
    public GameObject Slipper = null;
    public Transform SlipperForwardMark;
    public Transform SlipperBackMark;
    public Transform GrabMark;// 该变量目前为写死的点位，若能根据物体位置自动推测，则不必
    public Transform PutMark;
    public Transform HomeMark;
    public float slipperspeed = 10.0f;

    [Header("旋转部件参数")]
    public Transform RotatePart1;
    public Transform RotatePart2;
    public Transform RotatePart3;
    public Transform RotatePart4;
    public Transform RotatePart5;
    public float rotatespeed = 100.0f;

    [Range(-1, 1)]
    public float rotatePart1 = 0;
    [Range(-1, 1)]
    public float rotatePart2 = 0;
    [Range(-1, 1)]
    public float rotatePart3 = 0;
    [Range(-1, 1)]
    public float rotatePart4 = 0;
    [Range(-1, 1)]
    public float rotatePart5 = 0;

    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;
    public Slider slider5;

    [Header("夹爪参数")]
    public Transform LeftGripper;
    public Transform RightGripper;
    public BoxCollider LeftGripperCollider;
    public BoxCollider RightGripperCollider;
    public float gripperspeed = 10.0f;

    [Header("控制点位")]
    // 下列值目前是写死的点位，若能根据物体位置自动推测，则应由相关计算函数生成
    public float grabRetrieveSlider1Value;
    public float grabRetrieveSlider2Value;
    public float grabRetrieveSlider3Value;
    public float grabRetrieveSlider4Value;
    public float grabRetrieveSlider5Value;
    public float putRetrieveSlider1Value;
    public float putRetrieveSlider2Value;
    public float putRetrieveSlider3Value;
    public float putRetrieveSlider4Value;
    public float putRetrieveSlider5Value;
    public float grabSlider1Value;
    public float grabSlider2Value;
    public float grabSlider3Value;
    public float grabSlider4Value;
    public float grabSlider5Value;
    public float putSlider1Value;
    public float putSlider2Value;
    public float putSlider3Value;
    public float putSlider4Value;
    public float putSlider5Value;
    public float resetSlider1Value;
    public float resetSlider2Value;
    public float resetSlider3Value;
    public float resetSlider4Value;
    public float resetSlider5Value;

    private Vector3 SlipperForwardDirection;
    private int slipperState;// 0:Still, 1:Forward, 2:Back

    private float currentPart1angle;
    private float currentPart2angle;
    private float currentPart3angle;
    private float currentPart4angle;
    private float currentPart5angle;
    private float angleoffset = 1f;
    private bool isRotating = false;

    private int grabingSequential = -1;
    private Transform tempTransform;
    private float currentGripperAngleLeft;
    private float currentGripperAngleRight;
    private bool isGrabing = false;
    private bool isReallyGrabingLeft = false;
    private bool isReallyGrabingRight = false;
    private GameObject GrabedObject = null;

    private int putSequential = -1;

    private int resetSequential = -1;

    System.Timers.Timer t1 = new System.Timers.Timer(3000);   //实例化Timer类，设置间隔时间为1500毫秒;
    System.Timers.Timer t2 = new System.Timers.Timer(3000);

    // Start is called before the first frame update
    void Start()
    {
        SlipperForwardDirection = (SlipperForwardMark.position - SlipperBackMark.position).normalized;
        slipperState = 0;
        currentPart1angle = rotatePart1 * 90.0f;
        currentPart2angle = rotatePart2 * 30.0f;
        currentPart3angle = rotatePart3 * 30.0f;
        currentPart4angle = rotatePart4 * 120.0f;
        currentPart5angle = rotatePart5 * 180.0f;
        currentGripperAngleLeft = -10.0f;
        currentGripperAngleRight = -10.0f;

        t1.Elapsed += new System.Timers.ElapsedEventHandler(GrabOver); //到达时间的时候执行事件；   
        t1.AutoReset = false;
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
        if (slipperState == 1) RobotArmSlipForward();
        if (slipperState == 2) RobotArmSlipBack();
        if (grabingSequential >= 0) GotoGrab();
        if (putSequential >= 0) GotoPut();
        if (resetSequential >= 0) GotoReset();

        isRotating = false;
        if (currentPart1angle < rotatePart1 * 90.0f - angleoffset)
        {
            RotatePart1.transform.Rotate(Vector3.forward * Time.deltaTime * rotatespeed);
            currentPart1angle += Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        if (currentPart1angle > rotatePart1 * 90.0f + angleoffset)
        {
            RotatePart1.transform.Rotate(Vector3.back * Time.deltaTime * rotatespeed);
            currentPart1angle -= Time.deltaTime * rotatespeed;
            isRotating = true;
        }

        if (currentPart2angle < rotatePart2 * 30.0f - angleoffset)
        {
            RotatePart2.transform.Rotate(Vector3.up * Time.deltaTime * rotatespeed);
            currentPart2angle += Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        if (currentPart2angle > rotatePart2 * 30.0f + angleoffset)
        {
            RotatePart2.transform.Rotate(Vector3.down * Time.deltaTime * rotatespeed);
            currentPart2angle -= Time.deltaTime * rotatespeed;
            isRotating = true;
        }

        if (currentPart3angle < rotatePart3 * 30.0f - angleoffset)
        {
            RotatePart3.transform.Rotate(Vector3.up * Time.deltaTime * rotatespeed);
            currentPart3angle += Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        if (currentPart3angle > rotatePart3 * 30.0f + angleoffset)
        {
            RotatePart3.transform.Rotate(Vector3.down * Time.deltaTime * rotatespeed);
            currentPart3angle -= Time.deltaTime * rotatespeed;
            isRotating = true;
        }

        if (currentPart4angle < rotatePart4 * 120.0f - angleoffset)
        {
            RotatePart4.transform.Rotate(Vector3.up * Time.deltaTime * rotatespeed);
            currentPart4angle += Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        if (currentPart4angle > rotatePart4 * 120.0f + angleoffset)
        {
            RotatePart4.transform.Rotate(Vector3.down * Time.deltaTime * rotatespeed);
            currentPart4angle -= Time.deltaTime * rotatespeed;
            isRotating = true;
        }

        if (currentPart5angle < rotatePart5 * 180.0f - angleoffset)
        {
            RotatePart5.transform.Rotate(Vector3.up * Time.deltaTime * rotatespeed);
            currentPart5angle += Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        if (currentPart5angle > rotatePart5 * 180.0f + angleoffset)
        {
            RotatePart5.transform.Rotate(Vector3.down * Time.deltaTime * rotatespeed);
            currentPart5angle -= Time.deltaTime * rotatespeed;
            isRotating = true;
        }
        
        if (isRotating)
        {
            LeftGripperCollider.enabled = false;
            RightGripperCollider.enabled = false;
        }
        else
        {
            LeftGripperCollider.enabled = true;
            RightGripperCollider.enabled = true;
        }

        if (isGrabing)
        {
            if (currentGripperAngleLeft < 25.0f || currentGripperAngleRight < 25.0f)
            {
                if (!isReallyGrabingLeft)
                {
                    LeftGripper.transform.Rotate(Vector3.down * Time.deltaTime * gripperspeed);
                    currentGripperAngleLeft += Time.deltaTime * gripperspeed;
                }
                if (!isReallyGrabingRight)
                {
                    RightGripper.transform.Rotate(Vector3.up * Time.deltaTime * gripperspeed);
                    currentGripperAngleRight += Time.deltaTime * gripperspeed;
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

        if (!isGrabing)
        {
            if (currentGripperAngleLeft > -10.0f)
            {
                LeftGripper.transform.Rotate(Vector3.up * Time.deltaTime * gripperspeed);
                currentGripperAngleLeft -= Time.deltaTime * gripperspeed;
            }
            if (currentGripperAngleRight > -10.0f)
            {
                RightGripper.transform.Rotate(Vector3.down * Time.deltaTime * gripperspeed);
                currentGripperAngleRight -= Time.deltaTime * gripperspeed;
            }
            if (GrabedObject != null)
            {
                GrabedObject.GetComponent<Rigidbody>().isKinematic = false;
                GrabedObject.transform.parent = null;
                GrabedObject = null;
            }
            isReallyGrabingLeft = isReallyGrabingRight = false;
            LeftGripperCollider.enabled = false;
            RightGripperCollider.enabled = false;
        }
    }

    public void SetSlipperState(int state)
    {
        slipperState = state;
    }

    private void RobotArmSlipForward()
    {
        Vector3 forward2this = SlipperForwardMark.transform.position - this.transform.position;
        if (forward2this.x * SlipperForwardDirection.x >= 0 && forward2this.y * SlipperForwardDirection.y >= 0 && forward2this.z * SlipperForwardDirection.z >= 0)
        {
            this.transform.position += SlipperForwardDirection * slipperspeed * Time.deltaTime;
        }
        else
        {
            slipperState = 0;
        }
    }

    private void RobotArmSlipBack()
    {
        Vector3 this2back = this.transform.position - SlipperBackMark.transform.position;
        if (this2back.x * SlipperForwardDirection.x >= 0 && this2back.y * SlipperForwardDirection.y >= 0 && this2back.z* SlipperForwardDirection.z >= 0 )
        {
            this.transform.position += -SlipperForwardDirection * slipperspeed * Time.deltaTime;
        }
        else
        {
            slipperState = 0;
        }
    }

    public void OnRotatePart1ValueChanged(float value)
    {
        rotatePart1 = value;
    }

    public void OnRotatePart2ValueChanged(float value)
    {
        rotatePart2 = value;
    }

    public void OnRotatePart3ValueChanged(float value)
    {
        rotatePart3 = value;
    }

    public void OnRotatePart4ValueChanged(float value)
    {
        rotatePart4 = value;
    }

    public void OnRotatePart5ValueChanged(float value)
    {
        rotatePart5 = value;
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

    public void SetIsGrabing(bool flag)
    {
        isGrabing = flag;
    }

    // 从当前位置前去抓取物体，部分使用到的函数中的值目前用写死的点位生成，有时间的话看能不能根据物体位置自动分析机械臂参数，感觉比较困难
    private void GotoGrab()
    {
        // 收臂
        if (grabingSequential == 0) GrabRetrieveParts(ref grabingSequential);
        // 滑道平移
        else if (grabingSequential == 1 || grabingSequential == 2) Slip2Target(GrabMark, ref grabingSequential);
        // 伸臂
        else if (grabingSequential == 3) GrabStretchParts(ref grabingSequential);
        // 夹取
        else if (grabingSequential == 4) GripperGrab(ref grabingSequential);
    }

    private void GrabRetrieveParts(ref int sequential)
    {
        // 折叠中间臂
        slider1.value = grabRetrieveSlider1Value;
        slider2.value = grabRetrieveSlider2Value;
        slider3.value = grabRetrieveSlider3Value;
        slider4.value = grabRetrieveSlider4Value;
        slider5.value = grabRetrieveSlider5Value;
        sequential = 1;
    }

    private void PutRetrieveParts(ref int sequential)
    {
        // 折叠中间臂
        slider1.value = putRetrieveSlider1Value;
        slider2.value = putRetrieveSlider2Value;
        slider3.value = putRetrieveSlider3Value;
        slider4.value = putRetrieveSlider4Value;
        slider5.value = putRetrieveSlider5Value;
        sequential = 1;
    }

    private void Slip2Target(Transform targetTransform, ref int sequential)
    {
        if (sequential == 1)
        {
            if (Slipper != null)
            {
                if (grabingSequential == 1)
                {
                    if (Slipper.GetComponent<SlipperPartsController>().GetGrabingSequential() == -1) Slipper.GetComponent<SlipperPartsController>().StartGrab();
                    if (Slipper.GetComponent<SlipperPartsController>().GetGrabingSequential() != 2) return;
                }
                else if (putSequential == 1)
                {
                    if (Slipper.GetComponent<SlipperPartsController>().GetPutSequential() == -1) Slipper.GetComponent<SlipperPartsController>().StartPut();
                    if (Slipper.GetComponent<SlipperPartsController>().GetPutSequential() != 2) return;
                }
                else if (resetSequential == 1)
                {
                    if (Slipper.GetComponent<SlipperPartsController>().GetResetSequential() == -1) Slipper.GetComponent<SlipperPartsController>().StartReset();
                    if (Slipper.GetComponent<SlipperPartsController>().GetResetSequential() != 2) return;
                }
            }
            tempTransform = targetTransform;
            Vector3 target2this = targetTransform.position - this.transform.position;
            sequential = 2;
            if (target2this.x * SlipperForwardDirection.x >= 0 && target2this.y * SlipperForwardDirection.y >= 0 && target2this.z * SlipperForwardDirection.z >= 0)
            {
                slipperState = 1;
            }
            else slipperState = 2;
        }
        else if (sequential == 2)
        {
            Vector3 target2this = tempTransform.position - this.transform.position;
            if (slipperState == 1 && target2this.x * SlipperForwardDirection.x <= 0 && target2this.y * SlipperForwardDirection.y <= 0 && target2this.z * SlipperForwardDirection.z <= 0)
            {
                slipperState = 0;
                sequential = 3;
                if (Slipper != null)
                {
                    Slipper.GetComponent<SlipperPartsController>().OverGrab();
                    Slipper.GetComponent<SlipperPartsController>().OverPut();
                    Slipper.GetComponent<SlipperPartsController>().OverReset();
                }
            }
            if (slipperState == 2 && target2this.x * SlipperForwardDirection.x >= 0 && target2this.y * SlipperForwardDirection.y >= 0 && target2this.z * SlipperForwardDirection.z >= 0)
            {
                slipperState = 0;
                sequential = 3;
                if (Slipper != null)
                {
                    Slipper.GetComponent<SlipperPartsController>().OverGrab();
                    Slipper.GetComponent<SlipperPartsController>().OverPut();
                    Slipper.GetComponent<SlipperPartsController>().OverReset();
                }
            }
        }
    }

    private void GrabStretchParts(ref int sequential)
    {
        // 滑槽到达抓取点位，展开机械臂
        slider1.value = grabSlider1Value;
        slider2.value = grabSlider2Value;
        slider3.value = grabSlider3Value;
        slider4.value = grabSlider4Value;
        slider5.value = grabSlider5Value;
        sequential = 4;
    }

    private void PutStretchParts(ref int sequential)
    {
        // 滑槽到达释放点位，展开机械臂
        slider1.value = putSlider1Value;
        slider2.value = putSlider2Value;
        slider3.value = putSlider3Value;
        slider4.value = putSlider4Value;
        slider5.value = putSlider5Value;
        sequential = 4;
    }

    private void ResetStretchParts(ref int sequential)
    {
        // 滑槽到达抓取点位，展开机械臂
        slider1.value = resetSlider1Value;
        slider2.value = resetSlider2Value;
        slider3.value = resetSlider3Value;
        slider4.value = resetSlider4Value;
        slider5.value = resetSlider5Value;
        sequential = -1;
    }


    private void GripperGrab(ref int sequential)
    {
        isGrabing = true;
        t1.Start();
        sequential = -1;
    }

    private void GripperPut(ref int sequential)
    {
        isGrabing = false;
        t2.Start();
        sequential = -1;
    }

    private void GotoPut()
    {
        // 收臂
        if (putSequential == 0)
        {
            // rotatespeed = 10.0f;
            PutRetrieveParts(ref putSequential);
        }
        // 滑道平移
        else if (putSequential == 1 || putSequential == 2)
        {
            if (isRotating)
            {
                putSequential = 0;
                return;
            }
            // rotatespeed = 100.0f;
            Slip2Target(PutMark, ref putSequential);
        }
        // 伸臂
        else if (putSequential == 3)
        {
            rotatespeed = 10.0f;
            PutStretchParts(ref putSequential);
        }
        // 夹取
        else if (putSequential == 4)
        {
            if (isRotating)
            {
                putSequential = 3;
                return;
            }
            rotatespeed = 100.0f;
            GripperPut(ref putSequential);
            return;
        }
    }

    private void GotoReset()
    {
        // 收臂
        if (resetSequential == 0) PutRetrieveParts(ref resetSequential);        
        // 滑道平移
        else if (resetSequential == 1 || resetSequential == 2) Slip2Target(HomeMark, ref resetSequential);
        // 伸臂
        else if (resetSequential == 3) ResetStretchParts(ref resetSequential);
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

    public int CheckState()
    {
        int state = 0;
        if (grabingSequential != -1)
        {
            state = 3;
        }
        else if (putSequential != -1)
        {
            state = 4;
        }
        else if (resetSequential != -1)
        {
            state = 5;
        }
        else
        {
            state = slipperState;
        }
        return state;
    }
}

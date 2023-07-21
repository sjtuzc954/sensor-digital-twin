using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Rack4Manager : RackManager
{
    public GameObject Blocker0;
    public GameObject Blocker1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    public GameObject RobotArm;
    public GameObject Camera;

    private bool Rack4_Blocker0;
    private bool Rack4_Blocker1;
    private bool Rack4_Blocker2;
    private bool Rack4_Blocker3;
    private int Rack4_RobotArmState;// 0:Still, 1:Forward, 2:Back, 3:Grabing, 4:Putting, 5:Reseting
    Texture2D tex;

    private bool GrabUpdate = false;
    private bool PutUpdate = false;
    private bool ResetUpdate = false;
    // Start is called before the first frame update
    void Start()
    {
        Rack4_Blocker0 = false;
        Rack4_Blocker1 = false;
        Rack4_Blocker2 = false;
        Rack4_Blocker3 = false;
        Rack4_RobotArmState = 0;
    }

    // 阻挡气缸控制
    public void SetBlocker0()
    {
        Rack4_Blocker0 = !Rack4_Blocker0;
    }

    public void SetBlocker1()
    {
        Rack4_Blocker1 = !Rack4_Blocker1;
    }

    public void SetBlocker2()
    {
        Rack4_Blocker2 = !Rack4_Blocker2;
    }

    public void SetBlocker3()
    {
        Rack4_Blocker3 = !Rack4_Blocker3;
    }

    private void BlockerControl()
    {
        if (Rack4_Blocker0)
        {
            Blocker0.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker0.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack4_Blocker1)
        {
            Blocker1.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker1.GetComponentInChildren<BlockerController>().Up();
        }

        if (Rack4_Blocker2)
        {
            Blocker2.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker2.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack4_Blocker3)
        {
            Blocker3.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker3.GetComponentInChildren<BlockerController>().Up();
        }
    }

    // 机械臂控制
    public void SetRobotArmSlipperState(int state)
    {
        Rack4_RobotArmState = state;
        RobotArm.GetComponent<RobotArmController>().SetSlipperState(Rack4_RobotArmState);
    }

    public void OnRobotArmRotatePart1ValueChanged(float value)
    {
        RobotArm.GetComponent<RobotArmController>().rotatePart1 = value;
    }

    public void OnRobotArmRotatePart2ValueChanged(float value)
    {
        RobotArm.GetComponent<RobotArmController>().rotatePart2 = value;
    }

    public void OnRobotArmRotatePart3ValueChanged(float value)
    {
        RobotArm.GetComponent<RobotArmController>().rotatePart3 = value;
    }

    public void OnRobotArmRotatePart4ValueChanged(float value)
    {
        RobotArm.GetComponent<RobotArmController>().rotatePart4 = value;
    }

    public void OnRobotArmRotatePart5ValueChanged(float value)
    {
        RobotArm.GetComponent<RobotArmController>().rotatePart5 = value;
    }

    public void RobotArmStartGrab()
    {
        Rack4_RobotArmState = 3;
        RobotArm.GetComponent<RobotArmController>().StartGrab();
    }

    public void RobotArmStartPut()
    {
        Rack4_RobotArmState = 4;
        RobotArm.GetComponent<RobotArmController>().StartPut();
    }

    public void RobotArmReset()
    {
        Rack4_RobotArmState = 5;
        RobotArm.GetComponent<RobotArmController>().Reset();
    }

    public void RobotArmSetIsGrabing(bool flag)
    {
        RobotArm.GetComponent<RobotArmController>().SetIsGrabing(flag);
    }

    private void RobotArmControl()
    {
        Rack4_RobotArmState = RobotArm.GetComponent<RobotArmController>().CheckState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BlockerControl();
        RobotArmControl();
        Rack4UpdateInfo();
    }

    void Rack4UpdateInfo()
    {
        try
        {
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CACHE1BLOCKCYLINDERHOME")) Rack4_Blocker0 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CACHE1BLOCKCYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CACHE2BLOCKCYLINDERHOME")) Rack4_Blocker1 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CACHE2BLOCKCYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CHECKBLOCKCYLINDERHOME")) Rack4_Blocker2 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CHECKBLOCKCYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_DISCHARGEBLOCKCYLINDERHOME")) Rack4_Blocker3 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_DISCHARGEBLOCKCYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_DISCHARGEUPCYLINDERHOME"))
            {
                if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_DISCHARGEUPCYLINDERHOME"])))
                {
                    if (!GrabUpdate)
                    {
                        RobotArmStartGrab();
                        GrabUpdate = true;
                    }
                }
                else GrabUpdate = false;
            }
            /*if (GameManager.MsgDic.ContainsKey("downloadput1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["downloadput1"])))
                {
                    if (!PutUpdate)
                    {
                        RobotArmStartPut();
                        PutUpdate = true;
                    }
                }
                else PutUpdate = false;
            }
            if (GameManager.MsgDic.ContainsKey("downloadreset1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["downloadreset1"])))
                {
                    if (!ResetUpdate)
                    {
                        RobotArmReset();
                        ResetUpdate = true;
                    }
                }
                else ResetUpdate = false;
            }*/
        }
        catch (Exception ex)
        {

        }
    }

    public override string GetInfo(GameObject obj)
    {
        string infostr = "";
        infostr += "机台: 4号机台\n";
        switch (obj.name)
        {
            case "Rack4_Convey":
                infostr += "设备名称: 机台4传送带\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack4_Blocker0":
                infostr += "设备名称: 机台4阻挡气缸0\n";
                infostr += "运行状态: ";
                infostr += (Rack4_Blocker0) ? "Not Blocking" : "Blocking";
                break;
            case "Rack4_Blocker1":
                infostr += "设备名称: 机台4阻挡气缸1\n";
                infostr += "运行状态: ";
                infostr += (Rack4_Blocker1) ? "Not Blocking" : "Blocking";
                break;
            case "Rack4_Blocker2":
                infostr += "设备名称: 机台4阻挡气缸2\n";
                infostr += "运行状态: ";
                infostr += (Rack4_Blocker2) ? "Not Blocking" : "Blocking";
                break;
            case "Rack4_Blocker3":
                infostr += "设备名称: 机台4阻挡气缸3\n";
                infostr += "运行状态: ";
                infostr += (Rack4_Blocker3) ? "Not Blocking" : "Blocking";
                break;
            case "Rack4_Camera":
                infostr += "设备名称: 机台4摄像头\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack4_RobotArm":
                infostr += "设备名称: 机台4机械臂\n";
                infostr += "工作状态: ";
                switch (Rack4_RobotArmState)
                {
                    case 0: infostr += "Still";break;
                    case 1: infostr += "Noraml Forward"; break;
                    case 2: infostr += "Noraml Back"; break;
                    case 3: infostr += "Grabing"; break;
                    case 4: infostr += "Putting"; break;
                    case 5: infostr += "Reseting"; break;
                }
                break;
            default:
                break;
        }
        return infostr;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rack5Manager : RackManager
{
    public GameObject RobotArmSlipper;
    public GameObject RobotArm;

    private int Rack5_RobotArmState;// 0:Still, 1:Forward, 2:Back, 3:Grabing, 4:Putting, 5:Reseting
    private bool GrabUpdate = false;
    private bool PutUpdate = false;
    private bool ResetUpdate = false;
    // 机械臂控制
    public void SetSlipperState(int state)
    {
        Rack5_RobotArmState = state;
        RobotArmSlipper.GetComponent<SlipperPartsController>().SetSlipperState(state);
    }

    /*public void SetRobotArmSlipperState(int state)
    {
        Rack5_RobotArmState = state;
        RobotArm.GetComponent<XYZRobotArmController>().SetSlipperState(Rack5_RobotArmState);
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
    }*/

    public void RobotArmStartGrab()
    {
        Rack5_RobotArmState = 3;
        RobotArm.GetComponent<XYZRobotArmController>().StartGrab();
    }

    public void RobotArmStartPut()
    {
        Rack5_RobotArmState = 4;
        RobotArm.GetComponent<XYZRobotArmController>().StartPut();
    }

    public void RobotArmReset()
    {
        Rack5_RobotArmState = 5;
        RobotArm.GetComponent<XYZRobotArmController>().Reset();
    }

    /*public void RobotArmSetIsGrabing(bool flag)
    {
        RobotArm.GetComponent<XYZRobotArmController>().SetIsGrabing(flag);
    }*/

    private void RobotArmControl()
    {
        Rack5_RobotArmState = RobotArm.GetComponent<XYZRobotArmController>().CheckState();
    }

    // Start is called before the first frame update
    void Start()
    {
        Rack5_RobotArmState = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RobotArmControl();
        Rack5UpdateInfo();
    }

    void Rack5UpdateInfo()
    {
        try
        {
if (GameManager.MsgDic.ContainsKey("RACK5_X_PLACEJACKINGHOME"))
            {
                if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK5_X_PLACEJACKINGHOME"])))
                {
                    if (!GrabUpdate)
                    {
                        RobotArmStartGrab();
                        GrabUpdate = true;
                    }
                }
                else GrabUpdate = false;
            }
            /*if (GameManager.MsgDic.ContainsKey("downloadgrab2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["downloadgrab2"])))
                {
                    if (!GrabUpdate)
                    {
                        RobotArmStartGrab();
                        GrabUpdate = true;
                    }
                }
                else GrabUpdate = false;
            }
            if (GameManager.MsgDic.ContainsKey("downloadput2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["downloadput2"])))
                {
                    if (!PutUpdate)
                    {
                        RobotArmStartPut();
                        PutUpdate = true;
                    }
                }
                else PutUpdate = false;
            }
            if (GameManager.MsgDic.ContainsKey("downloadreset2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["downloadreset2"])))
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
        infostr += "机台: 5号机台\n";
        switch (obj.name)
        {
            case "Rack5_Convey1":
                infostr += "设备名称: 机台5传送带1\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack5_Convey2":
                infostr += "设备名称: 机台5传送带2\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack5_Blocker":
                infostr += "设备名称: 机台5阻挡气缸\n";
                infostr += "运行状态: ";
                infostr += "Blocking";
                break;
            case "Rack5_RobotArm":
                infostr += "设备名称: 机台5机械臂\n";
                infostr += "工作状态: ";
                switch (Rack5_RobotArmState)
                {
                    case 0: infostr += "Still"; break;
                    /*case 1: infostr += "Noraml Forward"; break;
                    case 2: infostr += "Noraml Back"; break;
                    case 3: infostr += "Grabing"; break;
                    case 4: infostr += "Putting"; break;
                    case 5: infostr += "Reseting"; break;*/
                    case 1: infostr += "Grabing"; break;
                    case 2: infostr += "Putting"; break;
                    case 3: infostr += "Reseting"; break;
                }
                break;
            default:
                break;
        }
        return infostr;
    }
}

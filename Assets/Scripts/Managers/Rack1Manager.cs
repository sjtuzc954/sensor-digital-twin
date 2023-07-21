using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rack1Manager : RackManager
{
    public GameObject Blocker1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    public GameObject Blocker4;
    public GameObject Blocker5;
    public GameObject Blocker6;
    public GameObject RobotArm1;
    public GameObject RobotArm2;

    private bool Rack1_Blocker1;
    private bool Rack1_Blocker2;
    private bool Rack1_Blocker3;
    private bool Rack1_Blocker4;
    private bool Rack1_Blocker5;
    private bool Rack1_Blocker6;
    private int Rack1_RobotArmState1;// 0:Still, 1:Grabing, 2:Putting, 3:Reseting
    private int Rack1_RobotArmState2;// 0:Still, 1:Grabing, 2:Putting, 3:Reseting

    private bool Grab1Update = false;
    private bool Put1Update = false;
    private bool Reset1Update = false;
    private bool Grab2Update = false;
    private bool Put2Update = false;
    private bool Reset2Update = false;
    // Start is called before the first frame update
    void Start()
    {
        Rack1_Blocker1 = false;
        Rack1_Blocker2 = false;
        Rack1_Blocker3 = false;
        Rack1_Blocker4 = false;
        Rack1_RobotArmState1 = 0;
        Rack1_RobotArmState2 = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BlockerControl();
        RobotArmControl();
        Rack1UpdateInfo();
    }

    // 阻挡气缸控制
    public void SetBlocker1()
    {
        Rack1_Blocker1 = !Rack1_Blocker1;
    }

    public void SetBlocker2()
    {
        Rack1_Blocker2 = !Rack1_Blocker2;
    }

    public void SetBlocker4()
    {
        Rack1_Blocker4 = !Rack1_Blocker4;
    }

    public void SetBlocker5()
    {
        Rack1_Blocker5 = !Rack1_Blocker5;
    }

    public void RobotArm1StartGrab()
    {
        Rack1_RobotArmState1 = 1;
        RobotArm1.GetComponent<XYZRobotArmController>().StartGrab();
    }

    public void RobotArm1StartPut()
    {
        Rack1_RobotArmState1 = 2;
        RobotArm1.GetComponent<XYZRobotArmController>().StartPut();
    }

    public void RobotArm1Reset()
    {
        Rack1_RobotArmState1 = 3;
        RobotArm1.GetComponent<XYZRobotArmController>().Reset();
    }

    public void RobotArm2StartGrab()
    {
        Rack1_RobotArmState2 = 1;
        RobotArm2.GetComponent<XYZRobotArmController>().StartGrab();
    }

    public void RobotArm2StartPut()
    {
        Rack1_RobotArmState2 = 2;
        RobotArm2.GetComponent<XYZRobotArmController>().StartPut();
    }

    public void RobotArm2Reset()
    {
        Rack1_RobotArmState2 = 3;
        RobotArm2.GetComponent<XYZRobotArmController>().Reset();
    }

    private void RobotArmControl()
    {
        Rack1_RobotArmState1 = RobotArm1.GetComponent<XYZRobotArmController>().CheckState();
        Rack1_RobotArmState2 = RobotArm2.GetComponent<XYZRobotArmController>().CheckState();
    }

    private void BlockerControl()
    {
        if (Rack1_Blocker1)
        {
            Blocker1.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker1.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack1_Blocker2)
        {
            Blocker2.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker2.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack1_Blocker4)
        {
            Blocker4.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker4.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack1_Blocker5)
        {
            Blocker5.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker5.GetComponentInChildren<BlockerController>().Up();
        }
    }

    void Rack1UpdateInfo()
    {
        try
        {
            /*if (NetWorkManager.recvStr.IndexOf("enterHome1") != -1) {
Rack1_Blocker1 = !Convert.ToBoolean(int.Parse(NetWorkManager.recvStr.Substring(NetWorkManager.recvStr.IndexOf("enterHome1") + ("enterHome1").Length + 1, 1)));
}*/
 if (GameManager.MsgDic.ContainsKey("RACK1_X_BLOCKCYLINDERHOME_A1")) {
Rack1_Blocker1 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_BLOCKCYLINDERHOME_A1"]));
}
            if (GameManager.MsgDic.ContainsKey("RACK1_X_BLOCKCYLINDERHOME_A2")) {
Rack1_Blocker2 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_BLOCKCYLINDERHOME_A2"]));
}
            if (GameManager.MsgDic.ContainsKey("RACK1_X_BLOCKCYLINDERHOME_A4")) 
{
Rack1_Blocker4 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_BLOCKCYLINDERHOME_A4"]));
}
            if (GameManager.MsgDic.ContainsKey("RACK1_X_BLOCKCYLINDERHOME_A5")) 
{
Rack1_Blocker5 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_BLOCKCYLINDERHOME_A5"]));
}
            if (GameManager.MsgDic.ContainsKey("RACK1_X_UPCYLINDERHOME_A2"))
            {
                if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_UPCYLINDERHOME_A2"])))
                {
                    if (!Grab1Update)
                    {
                        RobotArm1StartGrab();
                        Grab1Update = true;
                    }
                }
                else Grab1Update = false;              
            }
            /*if (GameManager.MsgDic.ContainsKey("enterput1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["enterput1"])))
                {
                    if (!Put1Update)
                    {
                        RobotArm1StartPut();
                        Put1Update = true;
                    }
                }
                else Put1Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("enterreset1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["enterreset1"])))
                {
                    if (!Reset1Update)
                    {
                        RobotArm1Reset();
                        Reset1Update = true;
                    }
                }
                else Reset1Update = false;
            }*/
            if (GameManager.MsgDic.ContainsKey("RACK1_X_UPCYLINDERHOME_A4"))
            {
                if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK1_X_UPCYLINDERHOME_A4"])))
                {
                    if (!Grab2Update)
                    {
                        RobotArm2StartGrab();
                        Grab2Update = true;
                    }
                }
                else Grab2Update = false;
            }
            /*if (GameManager.MsgDic.ContainsKey("enterput2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["enterput2"])))
                {
                    if (!Put2Update)
                    {
                        RobotArm2StartPut();
                        Put2Update = true;
                    }
                }
                else Put2Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("enterreset2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["enterreset2"])))
                {
                    if (!Reset2Update)
                    {
                        RobotArm2Reset();
                        Reset2Update = true;
                    }
                }
                else Reset2Update = false;
            }*/
        }
        catch (Exception ex)
        {

        }
    }

    public override string GetInfo(GameObject obj)
    {
        string infostr = "";
        infostr += "机台: 1号机台\n";
        switch (obj.name)
        {
            case "Rack1_Convey1":
                infostr += "设备名称: 机台1传送带1\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack1_Convey2":
                infostr += "设备名称: 机台1传送带2\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack1_Blocker1":
                infostr += "设备名称: 机台1阻挡气缸1\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker1) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_Blocker2":
                infostr += "设备名称: 机台1阻挡气缸2\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker2) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_Blocker3":
                infostr += "设备名称: 机台1阻挡气缸3\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker3) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_Blocker4":
                infostr += "设备名称: 机台1阻挡气缸4\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker4) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_Blocker5":
                infostr += "设备名称: 机台1阻挡气缸5\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker5) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_Blocker6":
                infostr += "设备名称: 机台1阻挡气缸6\n";
                infostr += "运行状态: ";
                infostr += (Rack1_Blocker6) ? "Not Blocking" : "Blocking";
                break;
            case "Rack1_RobotArm1":
                infostr += "设备名称: 机台1机械臂1\n";
                infostr += "工作状态: ";
                switch (Rack1_RobotArmState1)
                {
                    case 0: infostr += "Still"; break;
                    case 1: infostr += "Grabing"; break;
                    case 2: infostr += "Putting"; break;
                    case 3: infostr += "Reseting"; break;
                }
                break;
            case "Rack1_RobotArm2":
                infostr += "设备名称: 机台1机械臂2\n";
                infostr += "工作状态: ";
                switch (Rack1_RobotArmState2)
                {
                    case 0: infostr += "Still"; break;
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

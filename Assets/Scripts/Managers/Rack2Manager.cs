using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rack2Manager : RackManager
{
    public GameObject Blocker0;
    public GameObject Blocker1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    public GameObject Blocker4;
    public GameObject Camera1_1;
    public GameObject Camera1_2;
    public GameObject Camera1_3;
    public GameObject Sucker1;
    public GameObject Camera2_1;
    public GameObject Camera2_2;
    public GameObject Camera2_3;
    public GameObject Sucker2;

    private bool Rack2_Blocker0;
    private bool Rack2_Blocker1;
    private bool Rack2_Blocker2;
    private bool Rack2_Blocker3;
    private bool Rack2_Blocker4;
    private bool Rack2_Camera1_1_isCapture;
    private bool Rack2_Camera1_2_isCapture;
    private int Rack2_SuckerState1;
    private bool Rack2_Camera2_1_isCapture;
    private bool Rack2_Camera2_2_isCapture;
    private int Rack2_SuckerState2;

    private bool Rack2Working1_1 = false;
    private bool Rack2Working1_2 = false;
    private bool Rack2Working2_1 = false;
    private bool Rack2Working2_2 = false;

    private bool Camera1HomeUpdate = false;
    private bool Camera1MoveUpdate = false;
    private bool Camera2HomeUpdate = false;
    private bool Camera2MoveUpdate = false;
    private bool Suck1Update = false;
    private bool Check1Update = false;
    private bool Put1Update = false;
    private bool Reset1Update = false;
    private bool Suck2Update = false;
    private bool Check2Update = false;
    private bool Put2Update = false;
    private bool Reset2Update = false;
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Rack2_Blocker0 = false;
        Rack2_Blocker1 = false;
        Rack2_Blocker2 = false;
        Rack2_Blocker3 = true;
        Rack2_Blocker4 = true;
        Rack2_Camera1_1_isCapture = false;
        Rack2_Camera1_2_isCapture = false;
        Rack2_SuckerState1 = 0;
        Rack2_Camera2_1_isCapture = false;
        Rack2_Camera2_2_isCapture = false;
        Rack2_SuckerState2 = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BlockerControl();
        CameraMotionControl();
        SuckerControl();
        Rack2UpdateInfo();
    }

    // 阻挡气缸控制
    public void SetBlocker0()
    {Debug.Log("??");
        Rack2_Blocker0 = !Rack2_Blocker0;
    }

    public void SetBlocker1()
    {
        Rack2_Blocker1 = !Rack2_Blocker1;
    }

    public void SetBlocker2()
    {
        Rack2_Blocker2 = !Rack2_Blocker2;
    }

    public void SetBlocker3()
    {
        Rack2_Blocker3 = !Rack2_Blocker3;
    }

    public void SetBlocker4()
    {
        Rack2_Blocker4 = !Rack2_Blocker4;
    }

    private void BlockerControl()
    {
        /*if (Rack2_Blocker0)
        {
            Blocker0.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker0.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack2_Blocker1)
        {
            Blocker1.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker1.GetComponentInChildren<BlockerController>().Up();
        }*/
        if (Rack2_Blocker2)
        {
            Blocker2.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker2.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack2_Blocker3)
        {
            Blocker3.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker3.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack2_Blocker4)
        {
            Blocker4.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker4.GetComponentInChildren<BlockerController>().Up();
        }
    }

    public void SetCamera1_1(bool flag)
    {
        Rack2_Camera1_1_isCapture = flag;
    }

    public void SetCamera1_2(bool flag)
    {
        Rack2_Camera1_2_isCapture = flag;
    }

    public void SetCamera2_1(bool flag)
    {
        Rack2_Camera2_1_isCapture = flag;
    }

    public void SetCamera2_2(bool flag)
    {
        Rack2_Camera2_2_isCapture = flag;
    }

    private void CameraMotionControl()
    {
        if (Rack2_Camera1_1_isCapture)
        {
            Camera1_1.GetComponentInChildren<CameraMotionController>().Capture();
        }
        else
        {
            Camera1_1.GetComponentInChildren<CameraMotionController>().Home();
        }
        if (Rack2_Camera1_2_isCapture)
        {
            Camera1_2.GetComponentInChildren<CameraMotionController>().Capture();
        }
        else
        {
            Camera1_2.GetComponentInChildren<CameraMotionController>().Home();
        }
        if (Rack2_Camera2_1_isCapture)
        {
            Camera2_1.GetComponentInChildren<CameraMotionController>().Capture();
        }
        else
        {
            Camera2_1.GetComponentInChildren<CameraMotionController>().Home();
        }
        if (Rack2_Camera2_2_isCapture)
        {
            Camera2_2.GetComponentInChildren<CameraMotionController>().Capture();
        }
        else
        {
            Camera2_2.GetComponentInChildren<CameraMotionController>().Home();
        }
    }

    public void Sucker1StartSuck()
    {
        Rack2_SuckerState1 = 1;
        Sucker1.GetComponent<XYZSuckerController>().StartSuck();
    }

    public void Sucker1StartCheck()
    {
        Rack2_SuckerState1 = 2;
        Sucker1.GetComponent<XYZSuckerController>().StartCheck();
    }

    public void Sucker1StartPut()
    {
        Rack2_SuckerState1 = 3;
        Sucker1.GetComponent<XYZSuckerController>().StartPut();
    }

    public void Sucker1Reset()
    {
        Rack2_SuckerState1 = 4;
        Sucker1.GetComponent<XYZSuckerController>().Reset();
    }

    public void Sucker2StartSuck()
    {
        Rack2_SuckerState2 = 1;
        Sucker2.GetComponent<XYZSuckerController>().StartSuck();
    }

    public void Sucker2StartCheck()
    {
        Rack2_SuckerState2 = 2;
        Sucker2.GetComponent<XYZSuckerController>().StartCheck();
    }

    public void Sucker2StartPut()
    {
        Rack2_SuckerState2 = 3;
        Sucker2.GetComponent<XYZSuckerController>().StartPut();
    }

    public void Sucker2Reset()
    {
        Rack2_SuckerState2 = 4;
        Sucker2.GetComponent<XYZSuckerController>().Reset();
    }

    private void SuckerControl()
    {
        Rack2_SuckerState1 = Sucker1.GetComponent<XYZSuckerController>().CheckState();
        Rack2_SuckerState2 = Sucker2.GetComponent<XYZSuckerController>().CheckState();
    }

    void Rack2UpdateInfo()
    {
        try
        {/*if (GameManager.MsgDic.ContainsKey("stickHome1")) Rack2_Blocker1 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickHome1"]));*/
            if (GameManager.MsgDic.ContainsKey("RACK2_X_UPTIGHTCYLINDERHOME_01") && flag) Rack2_Blocker2 = Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK2_X_UPTIGHTCYLINDERHOME_01"]));
            /*if (GameManager.MsgDic.ContainsKey("stickHome3")) Rack2_Blocker3 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickHome3"]));
            if (GameManager.MsgDic.ContainsKey("stickHome4")) Rack2_Blocker4 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickHome4"]));*/

if (GameManager.MsgDic.ContainsKey("RACK2_X_PLACECYLINDERHOME_CCD01")) 
{
	if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK2_X_PLACECYLINDERHOME_CCD01"])))
	{
	  if (!Camera1HomeUpdate) {
	     Camera1HomeUpdate = true;
	     SetCamera1_2(true);
	     flag = true;
	  }
	}
	else {
	  if (Camera1MoveUpdate) {
	     Camera1HomeUpdate = false;
	     Camera1MoveUpdate = false;
	     Rack2Working1_2 = false;
	     Sucker1StartSuck();
	  }
	}
}
if (GameManager.MsgDic.ContainsKey("RACK2_X_PLACECYLINDERMOVE_CCD01")) 
{
	if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK2_X_PLACECYLINDERMOVE_CCD01"])))
	{
	  if (Rack2Working1_2 && !Camera1MoveUpdate) {
	     Camera1MoveUpdate = true;
	     SetCamera1_2(false);
	  }
	}
	else {
	     Rack2Working1_2 = true;
	}
}
if (GameManager.MsgDic.ContainsKey("RACK2_X_PLACECYLINDERHOME_CCD03")) 
{
	if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK2_X_PLACECYLINDERHOME_CCD03"])))
	{
	  if (!Camera2HomeUpdate) {
	     Camera2HomeUpdate = true;
	     SetCamera1_1(true);
	  }
	}
	else {
	  if (Camera2MoveUpdate) {
	     Camera2HomeUpdate = false;
	     Camera2MoveUpdate = false;
	     Rack2Working1_1 = false;
	     Sucker1StartCheck();
	  }
	}
}
if (GameManager.MsgDic.ContainsKey("RACK2_X_PLACECYLINDERMOVE_CCD03")) 
{
	if (!Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK2_X_PLACECYLINDERMOVE_CCD03"])))
	{
	  if (Rack2Working1_1 && !Camera2MoveUpdate) {
	     Camera2MoveUpdate = true;
	     SetCamera1_1(false);
	  }
	}
	else {
	     Rack2Working1_1 = true;
	}
}
            /*if (GameManager.MsgDic.ContainsKey("camera3")) Rack2_Camera2_2_isCapture = Convert.ToBoolean(int.Parse(GameManager.MsgDic["camera3"]));
            if (GameManager.MsgDic.ContainsKey("camera4")) Rack2_Camera2_1_isCapture = Convert.ToBoolean(int.Parse(GameManager.MsgDic["camera4"]));
            if (GameManager.MsgDic.ContainsKey("sticksuck1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["sticksuck1"])))
                {
                    if (!Suck1Update && Rack2Working1_2)
                    {
                        Sucker1StartSuck();
                        Suck1Update = true;
                        Rack2Working1_2 = false;
                    }
                }
                else Suck1Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("stickcheck1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickcheck1"])))
                {
                    if (!Check1Update && Rack2Working1_1)
                    {
                        Sucker1StartCheck();
                        Check1Update = true;
	        Rack2Working1_1 = false;
                    }
                }
                else Check1Update = false;
            }*/
            /*if (GameManager.MsgDic.ContainsKey("stickput1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickput1"])))
                {
                    if (!Put1Update)
                    {
                        Sucker1StartPut();
                        Put1Update = true;
                    }
                }
                else Put1Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("stickreset1"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickreset1"])))
                {
                    if (!Reset1Update)
                    {
                        Sucker1Reset();
                        Reset1Update = true;
                    }
                }
                else Reset1Update = false;
            }*/
            if (GameManager.MsgDic.ContainsKey("sticksuck2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["sticksuck2"])))
                {
                    if (!Suck2Update && Rack2Working2_2)
                    {
                        Sucker2StartSuck();
                        Suck2Update = true;
	        Rack2Working2_2 = false;
                    }
                }
                else Suck2Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("stickcheck2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickcheck2"])))
                {
                    if (!Check2Update && Rack2Working2_1)
                    {
                        Sucker2StartCheck();
                        Check2Update = true;
	        Rack2Working2_1 = false;
                    }
                }
                else Check2Update = false;
            }
            /*if (GameManager.MsgDic.ContainsKey("stickput2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickput2"])))
                {
                    if (!Put2Update)
                    {
                        Sucker2StartPut();
                        Put2Update = true;
                    }
                }
                else Put2Update = false;
            }
            if (GameManager.MsgDic.ContainsKey("stickreset2"))
            {
                if (Convert.ToBoolean(int.Parse(GameManager.MsgDic["stickreset2"])))
                {
                    if (!Reset2Update)
                    {
                        Sucker2Reset();
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
        infostr += "机台: 2号机台\n";
        switch (obj.name)
        {
            case "Rack2_Convey1":
                infostr += "设备名称: 机台2传送带1\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack2_Convey2":
                infostr += "设备名称: 机台2传送带2\n";
                infostr += "运行状态: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "运行速度: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack2_Blocker0":
                infostr += "设备名称: 机台2阻挡气缸0\n";
                infostr += "运行状态: ";
                infostr += (Rack2_Blocker0) ? "Not Blocking" : "Blocking";
                break;
            case "Rack2_Blocker1":
                infostr += "设备名称: 机台2阻挡气缸1\n";
                infostr += "运行状态: ";
                infostr += (Rack2_Blocker1) ? "Not Blocking" : "Blocking";
                break;
            case "Rack2_Blocker2":
                infostr += "设备名称: 机台2阻挡气缸2\n";
                infostr += "运行状态: ";
                infostr += (Rack2_Blocker2) ? "Not Blocking" : "Blocking";
                break;
            case "Rack2_Blocker3":
                infostr += "设备名称: 机台2阻挡气缸3\n";
                infostr += "运行状态: ";
                infostr += (Rack2_Blocker3) ? "Not Blocking" : "Blocking";
                break;
            case "Rack2_Blocker4":
                infostr += "设备名称: 机台2阻挡气缸4\n";
                infostr += "运行状态: ";
                infostr += (Rack2_Blocker4) ? "Not Blocking" : "Blocking";
                break;
            case "Rack2_Camera1_1":
                infostr += "设备名称: 机台2吸阀组1物料检测摄像头\n";
                infostr += "拍摄状态: ";
                infostr += (Rack2_Camera1_1_isCapture) ? "Capturing\n" : "Homing\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera1_1.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Camera1_2":
                infostr += "设备名称: 机台2吸阀组1标签检测摄像头\n";
                infostr += "拍摄状态: ";
                infostr += (Rack2_Camera1_2_isCapture) ? "Capturing\n" : "Homing\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera1_2.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Camera1_3":
                infostr += "设备名称: 机台2吸阀组1吸附检测摄像头\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera1_3.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Sucker1":
                infostr += "设备名称: 机台2吸阀组1\n";
                infostr += "工作状态: ";
                switch (Rack2_SuckerState1)
                {
                    case 0: infostr += "Still"; break;
                    case 1: infostr += "Sucking"; break;
                    case 2: infostr += "Checking"; break;
                    case 3: infostr += "Putting"; break;
                    case 4: infostr += "Reseting"; break;
                }
                break;
            case "Rack2_Camera2_1":
                infostr += "设备名称: 机台2吸阀组2物料检测摄像头\n";
                infostr += "拍摄状态: ";
                infostr += (Rack2_Camera2_1_isCapture) ? "Capturing\n" : "Homing\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera2_1.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Camera2_2":
                infostr += "设备名称: 机台2吸阀组2标签检测摄像头\n";
                infostr += "拍摄状态: ";
                infostr += (Rack2_Camera2_2_isCapture) ? "Capturing\n" : "Homing\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera2_2.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Camera2_3":
                infostr += "设备名称: 机台2吸阀组2吸附检测摄像头\n";
                infostr += "拍摄画面: ";
                GameManager.InfoTex = Camera2_3.GetComponentInChildren<RackCameraController>().CameraCapture();
                GameManager.isCameraInfo = true;
                break;
            case "Rack2_Sucker2":
                infostr += "设备名称: 机台2吸阀组2\n";
                infostr += "工作状态: ";
                switch (Rack2_SuckerState2)
                {
                    case 0: infostr += "Still"; break;
                    case 1: infostr += "Sucking"; break;
                    case 2: infostr += "Checking"; break;
                    case 3: infostr += "Putting"; break;
                    case 4: infostr += "Reseting"; break;
                }
                break;
            default:
                break;
        }
        return infostr;
    }
}

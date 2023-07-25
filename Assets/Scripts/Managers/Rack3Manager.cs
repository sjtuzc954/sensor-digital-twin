using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rack3Manager : RackManager
{
    public GameObject Blocker1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    public GameObject Blocker4;
    public GameObject Blocker5;
    public GameObject Blocker6;
    public GameObject Pressure1;
    public GameObject Pressure2;
    public GameObject Pressure3;

    private bool Rack3_Blocker1;
    private bool Rack3_Blocker2;
    private bool Rack3_Blocker3;
    private bool Rack3_Blocker4;
    private bool Rack3_Blocker5;
    private bool Rack3_Blocker6;
    private bool Rack3_Pressure1;
    private bool Rack3_Pressure2;
    private bool Rack3_Pressure3;

    // Start is called before the first frame update
    void Start()
    {
        Rack3_Blocker1 = false;
        Rack3_Blocker2 = false;
        Rack3_Blocker3 = false;
        Rack3_Blocker4 = false;
        Rack3_Blocker5 = false;
        Rack3_Blocker6 = false;
        Rack3_Pressure3 = false;
    }

    // �赲���׿���
    public void SetBlocker1()
    {
        Rack3_Blocker1 = !Rack3_Blocker1;
    }

    public void SetBlocker2()
    {
        Rack3_Blocker2 = !Rack3_Blocker2;
    }

    public void SetBlocker3()
    {
        Rack3_Blocker3 = !Rack3_Blocker3;
    }

    public void SetBlocker4()
    {
        Rack3_Blocker4 = !Rack3_Blocker4;
    }

    public void SetBlocker5()
    {
        Rack3_Blocker5 = !Rack3_Blocker5;
    }

    public void SetBlocker6()
    {
        Rack3_Blocker6 = !Rack3_Blocker6;
    }

    public void Pressure1Press()
    {
        Rack3_Pressure1 = true;
    }

    public void Pressure1Home()
    {
        Rack3_Pressure1 = false;
    }

    public void Pressure2Press()
    {
        Rack3_Pressure2 = true;
    }

    public void Pressure2Home()
    {
        Rack3_Pressure2 = false;
    }

    public void Pressure3Press()
    {
        Rack3_Pressure3 = true;
    }

    public void Pressure3Home()
    {
        Rack3_Pressure3 = false;
    }

    private void BlockerControl()
    {
        if (Rack3_Blocker1)
        {
            Blocker1.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker1.GetComponentInChildren<BlockerController>().Up();
        }

        if (Rack3_Blocker2)
        {
            Blocker2.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker2.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack3_Blocker3)
        {
            Blocker3.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker3.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack3_Blocker4)
        {
            Blocker4.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker4.GetComponentInChildren<BlockerController>().Up();
        }

        if (Rack3_Blocker5)
        {
            Blocker5.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker5.GetComponentInChildren<BlockerController>().Up();
        }
        if (Rack3_Blocker6)
        {
            Blocker6.GetComponentInChildren<BlockerController>().Down();
        }
        else
        {
            Blocker6.GetComponentInChildren<BlockerController>().Up();
        }
    }
    private void PressureControl()
    {
        if (Rack3_Pressure1)
        {
            Pressure1.GetComponentInChildren<PressureController>().Pressure();
        }
        else
        {
            Pressure1.GetComponentInChildren<PressureController>().Home();
        }
        if (Rack3_Pressure2)
        {
            Pressure2.GetComponentInChildren<PressureController>().Pressure();
        }
        else
        {
            Pressure2.GetComponentInChildren<PressureController>().Home();
        }
        if (Rack3_Pressure3)
        {
            Pressure3.GetComponentInChildren<PressureController>().Pressure();
        }
        else
        {
            Pressure3.GetComponentInChildren<PressureController>().Home();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        BlockerControl();
        PressureControl();
        Rack3UpdateInfo();
    }

    void Rack3UpdateInfo()
    {
        try
        {
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CACHEBLOCK1CYLINDERHOME")) Rack3_Blocker1 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CACHEBLOCK1CYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CACHEBLOCK2CYLINDERHOME")) Rack3_Blocker2 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CACHEBLOCK2CYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_CACHEBLOCK3CYLINDERHOME")) Rack3_Blocker3 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_CACHEBLOCK3CYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK1CYLINDERHOME")) Rack3_Blocker4 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK1CYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK2CYLINDERHOME")) Rack3_Blocker5 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK2CYLINDERHOME"]));
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK3CYLINDERHOME")) Rack3_Blocker6 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_PRESSUREMAINTAININGBLOCK3CYLINDERHOME"]));
            /*if (GameManager.MsgDic.ContainsKey("pressureActive1")) Rack3_Pressure1 = Convert.ToBoolean(int.Parse(GameManager.MsgDic["pressureActive1"]));
            if (GameManager.MsgDic.ContainsKey("pressureActive2")) Rack3_Pressure2 = Convert.ToBoolean(int.Parse(GameManager.MsgDic["pressureActive2"]));*/
            if (GameManager.MsgDic.ContainsKey("RACK34_DI_DATA_PRESSUREHEARTLIFT3CYLINDERHOME")) Rack3_Pressure3 = !Convert.ToBoolean(int.Parse(GameManager.MsgDic["RACK34_DI_DATA_PRESSUREHEARTLIFT3CYLINDERHOME"]));
        }
        catch (Exception ex)
        {

        }
    }

    public override string GetInfo(GameObject obj)
    {
        string infostr = "";
        infostr += "��̨: 3�Ż�̨\n";
        switch (obj.name)
        {
            case "Rack3_Convey1":
                infostr += "�豸����: ��̨3���ʹ�1\n";
                infostr += "����״̬: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "�����ٶ�: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack3_Convey2":
                infostr += "�豸����: ��̨3���ʹ�2\n";
                infostr += "����״̬: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().belton) ? "ON\n" : "OFF\n";
                infostr += "�����ٶ�: ";
                infostr += (obj.GetComponentInChildren<ConveyBeltMove>().speed).ToString();
                break;
            case "Rack3_Blocker1":
                infostr += "�豸����: ��̨3�赲����1\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker1) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Blocker2":
                infostr += "�豸����: ��̨3�赲����2\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker2) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Blocker3":
                infostr += "�豸����: ��̨3�赲����3\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker3) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Blocker4":
                infostr += "�豸����: ��̨3�赲����4\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker4) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Blocker5":
                infostr += "�豸����: ��̨3�赲����5\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker5) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Blocker6":
                infostr += "�豸����: ��̨3�赲����6\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Blocker6) ? "Not Blocking" : "Blocking";
                break;
            case "Rack3_Pressure1":
                infostr += "�豸����: ��̨3��ѹװ��1\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Pressure1) ? "Pressure" : "Home";
                break;
            case "Rack3_Pressure2":
                infostr += "�豸����: ��̨3��ѹװ��2\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Pressure2) ? "Pressure" : "Home";
                break;
            case "Rack3_Pressure3":
                infostr += "�豸����: ��̨3��ѹװ��3\n";
                infostr += "����״̬: ";
                infostr += (Rack3_Pressure3) ? "Pressure" : "Home";
                break;
            default:
                break;
        }
        return infostr;
    }
}

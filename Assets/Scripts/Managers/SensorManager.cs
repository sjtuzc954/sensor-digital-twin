using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : RackManager
{
    public GameObject HumidSensor;
    public GameObject TemperatureSensor;

    public override string GetInfo(GameObject obj)
    {
        string infostr = "";
        switch (obj.name)
        {
            case "HumidSensor":
                infostr = "�豸���ƣ�ʪ�ȴ�����\n";
                break;
            case "TemperatureSensor":
                infostr = "�豸���ƣ��¶ȴ�����\n";
                break;
            default:
                break;
        }
        infostr += "ʾ����" + obj.GetComponent<SensorLogger>().infostr + "\n";
        infostr += "������" + obj.GetComponent<SensorLogger>().alertstr + "\n";
        return infostr;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSensorInfo();
    }

    void UpdateSensorInfo()
    {
        try
        {
            if (GameManager.MsgDic.ContainsKey("Humidity"))
            {
                HumidSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["Humidity"]);
            }
            if (GameManager.MsgDic.ContainsKey("Humidity_Alert"))
            {
                HumidSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["Humidity_Alert"]);
            }
            if (GameManager.MsgDic.ContainsKey("Temperature"))
            {
                TemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["Temperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("Temperature_Alert"))
            {
                TemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["Temperature_Alert"]);
            }
        }
        catch (Exception e)
        {
            
        }
    }
}

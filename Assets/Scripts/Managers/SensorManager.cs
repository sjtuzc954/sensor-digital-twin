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
                infostr = "设备名称：湿度传感器\n";
                break;
            case "TemperatureSensor":
                infostr = "设备名称：温度传感器\n";
                break;
            default:
                break;
        }
        SensorLogger logger = obj.GetComponent<SensorLogger>();
        infostr += "示数：" + logger.infostr + "\n";
        if (logger.isAlert)
        {
            infostr += "状态：异常待处理\n";
            infostr += "报警：" + logger.alertstr + "\n";
        }
        else
        {
            infostr += "状态：正常\n";
        }
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

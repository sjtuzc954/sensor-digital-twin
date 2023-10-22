using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Text;
using System.Net;

public class SensorManager : RackManager
{
    public GameObject HumidSensor;
    public GameObject MachineTemperatureSensor;
    public GameObject VesselTemperatureSensor;
    public GameObject ChassisTemperatureSensor;
    public GameObject CompressorTemperatureSensor;
    public GameObject WaterSensor;
    public GameObject DistanceSensor;

    //邮箱服务器主机,根据使用的发送邮箱,使用其对应的服务器主机 例如 QQ邮箱服务器主机 smtp.qq.com
    private readonly static string host = "smtp.qq.com";
    //邮箱服务器主机端口
    private readonly static int port = 25;
    //发送邮件所用邮箱
    private static string senderEmail = "2694824893@qq.com";
    //发送邮件所用邮箱的密码 （第三方客户端登录授权码）
    private static string password = "rvxvxzrftyquddca";
    private static string subject = "传感器异常提示";
    private static string toEmail = "sjtuzc954@sjtu.edu.cn";

    public override string GetInfo(GameObject obj)
    {
        string infostr = "";
        switch (obj.name)
        {
            case "HumidSensor":
                infostr = "设备名称：湿度传感器\n";
                break;
            case "MachineTemperatureSensor":
                infostr = "设备名称：温度传感器(机台)\n";
                break;
            case "VesselTemperatureSensor":
                infostr = "设备名称：温度传感器(气罐)\n";
                break;
            case "ChassisTemperatureSensor":
                infostr = "设备名称：温度传感器(主机)\n";
                break;
            case "CompressorTemperatureSensor":
                infostr = "设备名称：温度传感器(压缩机)\n";
                break;
            case "WaterSensor":
                infostr = "设备名称：水浸传感器\n";
                break;
            case "DistanceSensor":
                infostr = "设备名称：距离传感器\n";
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
        StartCoroutine(SendEmail());
    }

    IEnumerator SendEmail()
    {
        while (true)
        {
            SensorLogger logger1 = HumidSensor.GetComponent<SensorLogger>();
            SensorLogger logger2 = MachineTemperatureSensor.GetComponent<SensorLogger>();
            SensorLogger logger3 = WaterSensor.GetComponent<SensorLogger>();
            SensorLogger logger4 = DistanceSensor.GetComponent<SensorLogger>();
            SensorLogger logger5 = VesselTemperatureSensor.GetComponent<SensorLogger>();
            SensorLogger logger6 = ChassisTemperatureSensor.GetComponent<SensorLogger>();
            SensorLogger logger7 = CompressorTemperatureSensor.GetComponent<SensorLogger>();
            string body = "";
            if (!logger1.isAlert && !logger2.isAlert && !logger3.isAlert && !logger4.isAlert)
            {
                yield return new WaitForSeconds(60);
                continue;
            }
            if (logger1.isAlert)
            {
                body += GetInfo(HumidSensor);
            }
            if (logger2.isAlert)
            {
                body += GetInfo(MachineTemperatureSensor);
            }
            if (logger3.isAlert)
            {
                body += GetInfo(WaterSensor);
            }
            if (logger4.isAlert)
            {
                body += GetInfo(DistanceSensor);
            }
            if (logger5.isAlert)
            {
                body += GetInfo(VesselTemperatureSensor);
            }
            if (logger6.isAlert)
            {
                body += GetInfo(ChassisTemperatureSensor);
            }
            if (logger7.isAlert)
            {
                body += GetInfo(CompressorTemperatureSensor);
            }
            if (body.Length == 0)
            {
                yield return new WaitForSeconds(5);
                continue;
            }
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = false,
            };
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = host,
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            mailMessage.To.Add(new MailAddress(toEmail));
            smtpClient.SendMailAsync(mailMessage);
            Debug.Log($"发送邮件：接收人 - {toEmail} 主题 - {subject} 内容 - {body}");
            yield return new WaitForSeconds(600);
        }
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
                GameManager.MsgDic.Remove("Humidity_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("MachineTemperature"))
            {
                MachineTemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["MachineTemperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("MachineTemperature_Alert"))
            {
                MachineTemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["MachineTemperature_Alert"]);
                GameManager.MsgDic.Remove("MachineTemperature_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("VesselTemperature"))
            {
                VesselTemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["VesselTemperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("VesselTemperature_Alert"))
            {
                VesselTemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["VesselTemperature_Alert"]);
                GameManager.MsgDic.Remove("VesselTemperature_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("ChassisTemperature"))
            {
                ChassisTemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["ChassisTemperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("ChassisTemperature_Alert"))
            {
                ChassisTemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["ChassisTemperature_Alert"]);
                GameManager.MsgDic.Remove("ChassisTemperature_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("CompressorTemperature"))
            {
                CompressorTemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["CompressorTemperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("CompressorTemperature_Alert"))
            {
                MachineTemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["CompressorTemperature_Alert"]);
                GameManager.MsgDic.Remove("CompressorTemperature_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("Water"))
            {
                WaterSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["Water"]);
            }
            if (GameManager.MsgDic.ContainsKey("Water_Alert"))
            {
                WaterSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["Water_Alert"]);
                GameManager.MsgDic.Remove("Water_Alert");
            }
            if (GameManager.MsgDic.ContainsKey("Distance"))
            {
                DistanceSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["Distance"]);
            }
            if (GameManager.MsgDic.ContainsKey("Distance_Alert"))
            {
                DistanceSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["Distance_Alert"]);
                GameManager.MsgDic.Remove("Distance_Alert");
            }
        }
        catch (Exception e)
        {

        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}

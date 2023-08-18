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
    public GameObject TemperatureSensor;

    //�������������,����ʹ�õķ�������,ʹ�����Ӧ�ķ��������� ���� QQ������������� smtp.qq.com
    private readonly static string host = "smtp.qq.com";
    //��������������˿�
    private readonly static int port = 25;
    //�����ʼ���������
    private static string senderEmail = "2694824893@qq.com";
    //�����ʼ�������������� ���������ͻ��˵�¼��Ȩ�룩
    private static string password = "rvxvxzrftyquddca";
    private static string subject = "�������쳣��ʾ";
    private static string toEmail = "sjtuzc954@sjtu.edu.cn";

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
        SensorLogger logger = obj.GetComponent<SensorLogger>();
        infostr += "ʾ����" + logger.infostr + "\n";
        if (logger.isAlert)
        {
            infostr += "״̬���쳣������\n";
            infostr += "������" + logger.alertstr + "\n";
        }
        else
        {
            infostr += "״̬������\n";
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
            SensorLogger logger2 = TemperatureSensor.GetComponent<SensorLogger>();
            string body = "";
            if (!logger1.isAlert && !logger2.isAlert)
            {
                yield return new WaitForSeconds(5);
                continue;
            }
            if (logger1.isAlert)
            {
                body += GetInfo(HumidSensor);
            }
            if (logger2.isAlert)
            {
                body += GetInfo(TemperatureSensor);
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
            Debug.Log($"�����ʼ��������� - {toEmail} ���� - {subject} ���� - {body}");
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
            if (GameManager.MsgDic.ContainsKey("Temperature"))
            {
                TemperatureSensor.GetComponent<SensorLogger>().UpdateInfo(GameManager.MsgDic["Temperature"]);
            }
            if (GameManager.MsgDic.ContainsKey("Temperature_Alert"))
            {
                TemperatureSensor.GetComponent<SensorLogger>().UpdateAlert(GameManager.MsgDic["Temperature_Alert"]);
                GameManager.MsgDic.Remove("Temperature_Alert");
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
